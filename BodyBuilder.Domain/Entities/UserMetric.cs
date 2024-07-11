using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class UserMetric {
        public Guid BodyMetricsId { get; set; }
        public Guid MetricId { get; set; }
        public string MetricName { get; set; } = String.Empty;
        public float Value { get; set; }
        public string Color { get; set; } = String.Empty;
        public string Color2 { get; set; } = String.Empty;


    }
}
