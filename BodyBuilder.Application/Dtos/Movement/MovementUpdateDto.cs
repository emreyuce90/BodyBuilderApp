using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Dtos.Movement {
    public class MovementUpdateDto {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string Tip { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public Guid BodyPartId { get; set; }
    }
}
