using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class BodyMetrics:BaseEntity {
        public string MetricName { get; set; }=string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Color2 { get; set; } = string.Empty;

    }
}
