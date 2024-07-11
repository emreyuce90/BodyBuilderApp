using BodyBuilderApp.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Interfaces {
    public interface IMetricsService {
        Task<Response> GetUsersMetrics(Guid userId);
        Task<Response> UpdateMetricsByMetricIdAsync(Guid metricId, float newValue);
        Task<Response> CreateUserMetricsAsync(Guid userId, float value,Guid bodymetricId,Guid metricId);
        Task<Response> GetUserMetricLogsAsync(Guid userId,Guid bodymetricId);
        Task<Response> DeleteMetricByMetricIdAsync(Guid metricId);
    }
}
