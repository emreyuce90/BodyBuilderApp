using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using BodyBuilder.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Infrastructure.Persistence.Repositories {
    public class EfSubProgrammeRepository : EfGenericRepository<SubProgramme>, ISubProgrammeRepository {
        public EfSubProgrammeRepository(BodyBuilderContext context) : base(context) {
        }
    }
}
