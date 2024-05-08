using BodyBuilder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Infrastructure.Persistence.Context {
    public class BodyBuilderContext :DbContext{
        public BodyBuilderContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<BodyPart> BodyParts { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<Programme> Programmes { get; set; }
        public DbSet<SubProgramme> SubProgrammes { get; set; }
        public DbSet<SubProgrammeMovement> SubProgrammeMovements { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutMovement> WorkoutMovements { get; set; }
        public DbSet<WorkoutMovementSet> WorkoutMovementSets { get; set; }
    }
}
