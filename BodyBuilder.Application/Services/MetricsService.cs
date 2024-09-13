using BodyBuilder.Application.Dtos.Metrics;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilder.Infrastructure.Persistence.Context;
using BodyBuilder.Infrastructure.Persistence.Repositories;
using BodyBuilderApp.Communication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Services {

    public class MetricsService : IMetricsService {
        private readonly IMetricsRepository _metricsRepository;
        private readonly BodyBuilderContext _context;
        private readonly IBodyMetrics _bodyMetrics;
        private readonly IUserRepository _userRepository;

        public MetricsService(IMetricsRepository metricsRepository, BodyBuilderContext context, IBodyMetrics bodyMetrics, IUserRepository userRepository) {
            _metricsRepository = metricsRepository;
            _context = context;
            _bodyMetrics = bodyMetrics;
            _userRepository = userRepository;
        }

        public async Task<Response> CreateUserMetricsAsync(Guid userId, float value, Guid bodymetricId, Guid? metricId) {
            try {
                var userExist = await _userRepository.Table.AnyAsync(u => u.Id == userId && !u.IsDeleted && u.IsActive);
                if (!userExist) {
                    return new Response() { Code = 400, Message = "Böyle bir kullanıcı bulunamadı" };
                }
                //check if user has data in today
                int count = await _metricsRepository.CountAsync(m => m.UserId == userId && m.BodyMetricsId == bodymetricId && m.MeasurementDate.Date == DateTime.Now.Date && m.MeasurementDate.Month == DateTime.Now.Month && m.MeasurementDate.Year == DateTime.Now.Year);

                if (count > 0 && metricId.HasValue) {
                    return await UpdateMetricsByMetricIdAsync(metricId.Value, value);
                }

                var userMetric = new Metrics {
                    UserId = userId,
                    Value = value,
                    MeasurementDate = DateTime.Now,
                    BodyMetricsId = bodymetricId,
                    IsActive = true,
                    IsDeleted = false
                };
                await _metricsRepository.CreateAsync(userMetric);
                await _context.SaveChangesAsync();
                var metric = await _context.UserMetricValues.FromSqlRaw("SELECT * FROM GetUserMetricById({0},{1})", userId, bodymetricId).ToListAsync();

                return new Response(userMetric);

            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }

        public async Task<Response> DeleteMetricByMetricIdAsync(Guid metricId) {
            try {
                await _metricsRepository.DeleteAsync(metricId);
                await _metricsRepository.SaveAsync();
                return new Response() { Code = 200, Message = "Kullanıcı metriği silme işlemi başarıyla tamamlandı" };

            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }

        public async Task<Response> GetUserMetricLogsAsync(Guid userId, Guid bodymetricId) {
            try {

                var userExist = await _userRepository.Table.AnyAsync(u => u.Id == userId && !u.IsDeleted && u.IsActive);
                if (!userExist) {
                    return new Response() { Code=400 ,Message="Böyle bir kullanıcı bulunamadı"};
                }

                var userMetrics = await _metricsRepository.Table.Include(m => m.BodyMetrics).Where(m => m.UserId == userId && m.BodyMetricsId == bodymetricId && !m.IsDeleted && m.IsActive).OrderByDescending(c => c.CreatedDate).ToListAsync();

                if (userMetrics.Count == 0) {
                    return new Response() { Code = 200, Message = "Bu kullanıcıya ait kayıt bulunamadı" };
                }

                var metricLogList = new List<MetricLogDto>();

                foreach (var metric in userMetrics) {

                    var temporary = new MetricLogDto {
                        CreatedDate = metric.CreatedDate,
                        MetricId = metric.Id,
                        MetricName = metric.BodyMetrics.MetricName,
                        Value = metric.Value
                    };
                    metricLogList.Add(temporary);
                }

                return new Response(metricLogList);
            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }

        public async Task<Response> GetUsersMetrics(Guid userId) {
            try {
                var userExists = await _userRepository.Table.AnyAsync(u => u.Id == userId && u.IsDeleted == false && u.IsActive == true);
                if (!userExists) {
                    return new Response() { Code = 400, Message = "Kullanıcı bulunamadı" };
                }
                var metrics = await _metricsRepository.Table
                                                      .Include(m => m.BodyMetrics)
                                                      .Where(m => m.UserId == userId && m.IsDeleted == false && m.IsActive == true)
                                                      .GroupBy(m => m.BodyMetricsId) 
                                                      .Select(g => g.OrderByDescending(m => m.MeasurementDate).FirstOrDefault())
                                                      .ToListAsync();
                if (metrics.Count == 0) {
                    var allMetrics = await _bodyMetrics.GetAllAsync().ToListAsync();
                    var emptyMetrics = new List<UserMetric>();
                    foreach (var bm in allMetrics) {
                        UserMetric u = new UserMetric() {
                            BodyMetricsId = bm.Id,
                            Color = bm.Color,
                            Color2 = bm.Color2,
                            MetricName = bm.MetricName,
                            Value = 0
                        };
                        emptyMetrics.Add(u);
                    }
                    return new Response() { Code = 200, Resource = emptyMetrics };
                }

                var allBodyMetrics = await _bodyMetrics.GetAllAsync().ToListAsync();
                var completeMetrics = new List<UserMetric>();

                foreach (var bm in allBodyMetrics) {

                    var existingMetric = metrics.FirstOrDefault(m => m.BodyMetricsId == bm.Id);
                    if (existingMetric != null) {
                        var um = new UserMetric() {
                            BodyMetricsId = existingMetric.BodyMetricsId,
                            Color = existingMetric.BodyMetrics.Color,
                            Color2 = existingMetric.BodyMetrics.Color2,
                            MetricId = existingMetric.Id,
                            Value = existingMetric.Value,
                            MetricName = existingMetric.BodyMetrics.MetricName,
                        };
                        completeMetrics.Add(um);
                    } else {

                        UserMetric u = new UserMetric() {
                            BodyMetricsId = bm.Id,
                            Color = bm.Color,
                            Color2 = bm.Color2,
                            MetricName = bm.MetricName,
                            Value = 0
                        };
                        completeMetrics.Add(u);
                    }
                }
                return new Response() { Code = 200, Resource = completeMetrics };
            } catch (Exception ex) {
                return new Response(ex.Message);
                throw;
            }
        }

        public async Task<Response> UpdateMetricsByMetricIdAsync(Guid metricId, float newValue) {
            try {
                //find the data from db
                var metric = await _metricsRepository.GetSingle(m => m.Id == metricId && m.IsDeleted == false && m.IsActive == true);

                if (metric == null) {
                    return new Response() { Code = 200, Message = "Bu kayda ait metric bilgileri bulunamadı" };
                }

                metric.Value = newValue;
                _metricsRepository.UpdateAsync(metric);
                await _metricsRepository.SaveAsync();
                return new Response() { Code = 200, Resource = newValue };

            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }
    }
}
