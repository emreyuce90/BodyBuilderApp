using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Dtos.Metrics {
    public class MetricLogDto {
        public DateTime CreatedDate { get; set; }
        public float Value { get; set; }
        public Guid MetricId { get; set; }
        public string MetricName { get; set; }
    }
}
