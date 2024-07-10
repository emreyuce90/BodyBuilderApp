using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class UserMetricLog {
        public string MetricName { get; set; }
        public DateTime CreatedDate { get; set; }
        public float Value { get; set; }
    }
}
