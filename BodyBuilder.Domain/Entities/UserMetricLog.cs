using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class UserMetricLog {
        public Guid MetricId { get; set; }
        public string MetricName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public float Value { get; set; }
    }
}
