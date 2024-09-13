using BodyBuilder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Infrastructure.Persistence.Context {
    public class BodyBuilderContext : DbContext {
        public BodyBuilderContext(DbContextOptions options) : base(options) {

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
        public DbSet<Metrics> Metrics { get; set; }
        public DbSet<SubBodyPart> SubBodyPart { get; set; }

        public DbSet<BodyMetrics> BodyMetrics { get; set; }
        public DbSet<WorkoutLog> WorkoutLogs { get; set; }
        public DbSet<WorkoutLogDetail> WorkoutLogDetails { get; set; }
        public DbSet<UserMetric> UserMetrics { get; set; }
        public DbSet<UserMetricValue> UserMetricValues { get; set; }
        public DbSet<UserMetricLog> UserMetricLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Bir programme birden fazla subprogrammeye sahiptir
            modelBuilder.Entity<Programme>()
           .HasMany(p => p.SubProgrammes)
           // Bir subprogrammenin tek bir programmesi olabilir
           .WithOne(sp => sp.Programme)
           // Ayırt edici nokta fk ProgrammeId dir
           .HasForeignKey(sp => sp.ProgrammeId);

            modelBuilder.Entity<SubProgramme>()
                // Bir subprogramme birden fazla spMovementsa sahiptir
                .HasMany(sp => sp.SubProgrammeMovements)
                //fakat bir sp movements tek bir subprogrammeye aittir
                .WithOne(spm => spm.SubProgramme)
                // burada da sp id fk dir
                .HasForeignKey(spm => spm.SubProgrammeId);

            modelBuilder.Entity<SubProgrammeMovement>()
                .HasOne(spm => spm.Movement)
                .WithMany()
                .HasForeignKey(spm => spm.MovementId);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkoutLog>().HasNoKey().ToView(null);
            modelBuilder.Entity<WorkoutLogDetail>().HasNoKey().ToView(null);
            modelBuilder.Entity<UserMetric>().HasNoKey().ToView(null);
            modelBuilder.Entity<UserMetricValue>().HasNoKey().ToView(null);
            modelBuilder.Entity<UserMetricLog>().HasNoKey().ToView(null);

        }



    }
}
