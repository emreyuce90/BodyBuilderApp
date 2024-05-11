using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public abstract class BaseEntity {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }= DateTime.Now;
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } =false;
        public bool IsActive { get; set; } = true;
    }
}
