using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class MovementDetail {
        public MovementDetail()
        {
            MovementSets = new List<MovementSets>();
        }
        public string MovementName { get; set; } = string.Empty;

        public List<MovementSets> MovementSets { get; set; }
    }
}
