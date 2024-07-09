using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Dtos.Metrics {
    public class UserMetrics {
        public Guid MetricId { get; set; } //7aaf453f-56ea-4f7d-8877-4cec29072bfe
        public string MetricName { get; set; } = String.Empty; //Göğüs
        public float Value { get; set; } //122.50cm
    }
}
