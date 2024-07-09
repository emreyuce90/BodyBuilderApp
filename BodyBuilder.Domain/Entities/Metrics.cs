using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class Metrics:BaseEntity {
        public Guid UserId { get; set; }

        public DateTime MeasurementDate { get; set; }
        public float Value { get; set; }

        public Guid BodyMetricsId { get; set; } 
        public virtual BodyMetrics BodyMetrics { get; set; }
    }
}
