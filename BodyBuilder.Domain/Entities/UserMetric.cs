using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class UserMetric {
        public Guid BodyMetricsId { get; set; }
        public string MetricName { get; set; } = String.Empty;
        public float Value { get; set; }

    }
}
