using BodyBuilder.Application.Dtos.Metrics;
using BodyBuilder.Application.Interfaces;
using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilder.Infrastructure.Persistence.Context;
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

        public MetricsService(IMetricsRepository metricsRepository, BodyBuilderContext context) {
            _metricsRepository = metricsRepository;
            _context = context;
        }

        public async Task<Response> CreateUserMetricsAsync(Guid userId, float value, Guid bodymetricId) {
            try {
                var userMetric = new Metrics {
                    UserId = userId,
                    Value = value,
                    MeasurementDate= DateTime.Now,
                    BodyMetricsId= bodymetricId,
                    IsActive= true,
                    IsDeleted= false
                };
                await _metricsRepository.CreateAsync(userMetric);
                await _context.SaveChangesAsync();
                var metric = await _context.UserMetricValues.FromSqlRaw("SELECT * FROM GetUserMetricById({0},{1})", userId,bodymetricId).ToListAsync();
                return new Response(metric[0]);

            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }

        public async Task<Response> GetUsersMetrics(Guid userId) {
            try {
                //var metrics = await _metricsRepository.Table.Include(m => m.BodyMetrics).Where(m => m.UserId == userId && m.IsDeleted == false && m.IsActive == true).ToListAsync();
                var metrics =await _context.UserMetrics.FromSqlRaw("SELECT * FROM GetUserMetricsByUserId({0})", userId).ToListAsync();
                if (metrics.Count == 0) {
                    return new Response() { Code = 200, Message = "Bu kullanıcıya ait ölçüm verileri bulunamadı" };
                }
                return new Response() { Code = 200, Resource = metrics };
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
                return new Response() { Code = 200, Resource = metric };

            } catch (Exception ex) {
                return new Response(ex);
                throw;
            }
        }
    }
}
