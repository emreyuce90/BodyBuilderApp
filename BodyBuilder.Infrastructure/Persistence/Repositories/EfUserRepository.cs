using BodyBuilder.Domain.Entities;
using BodyBuilder.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Infrastructure.Persistence.Repositories {
    public class EfUserRepository : EfGenericRepository<User>, IUserRepository {
        public EfUserRepository(DbContext context) : base(context) {
        }
    }
}
