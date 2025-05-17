﻿using System;
using System.Collections.Generic;
using DAL.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class GymWebServiceContext : IdentityDbContext<
    User, 
    IdentityRole<int>, 
    int, 
    IdentityUserClaim<int>,
    IdentityUserRole<int>,
    IdentityUserLogin<int>,
    IdentityRoleClaim<int>,
    IdentityUserToken<int>>
{
    public GymWebServiceContext()
    {
    }

    public GymWebServiceContext(DbContextOptions<GymWebServiceContext> options)
        : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Workout> Workouts { get; set; }

    public virtual DbSet<WorkoutExercise> WorkoutExercises { get; set; }

    public virtual DbSet<WorkoutTemplate> WorkoutTemplates { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?Linkid=723263.
    //    => optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Database=GymWebService;Username=pgadmin;Password=pgadmin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("exercises_pkey");

            entity.ToTable("exercises");

            entity.Property(e => e.ExerciseId).HasColumnName("exerciseid");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.MuscleGroups)
                .HasMaxLength(255)
                .HasColumnName("musclegroups");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Exercises)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("exercises_userid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.Height)
                .HasPrecision(10, 2)
                .HasColumnName("height");
            entity.Property(e => e.Level)
                .HasMaxLength(50)
                .HasColumnName("level");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Weight)
                .HasPrecision(10, 2)
                .HasColumnName("weight");
        });

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(e => e.WorkoutId).HasName("workouts_pkey");

            entity.ToTable("workouts");

            entity.Property(e => e.WorkoutId).HasColumnName("workoutid");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("date");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UserId).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Workouts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("workouts_userid_fkey");
        });

        modelBuilder.Entity<WorkoutExercise>(entity =>
        {
            entity.HasKey(e => e.WorkoutExerciseId).HasName("workoutexercises_pkey");

            entity.ToTable("workoutexercises");

            entity.Property(e => e.WorkoutExerciseId).HasColumnName("workoutexerciseid");
            entity.Property(e => e.Distance)
                .HasPrecision(10, 2)
                .HasColumnName("distance");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.ExerciseId).HasColumnName("exerciseid");
            entity.Property(e => e.Reps).HasColumnName("reps");
            entity.Property(e => e.Sets).HasColumnName("sets");
            entity.Property(e => e.Weight)
                .HasPrecision(10, 2)
                .HasColumnName("weight");
            entity.Property(e => e.WorkoutId).HasColumnName("workoutid");

            entity.HasOne(d => d.Exercise).WithMany(p => p.WorkoutExercises)
                .HasForeignKey(d => d.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("workoutexercises_exerciseid_fkey");

            entity.HasOne(d => d.Workout).WithMany(p => p.WorkoutExercises)
                .HasForeignKey(d => d.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("workoutexercises_workoutid_fkey");
        });

        modelBuilder.Entity<WorkoutTemplate>(entity =>
        {
            entity.HasKey(e => e.TemplateId).HasName("workouttemplates_pkey");

            entity.ToTable("workouttemplates");

            entity.Property(e => e.TemplateId).HasColumnName("templateid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.WorkoutTemplates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("workouttemplates_userid_fkey");
        });
        
        modelBuilder.Entity<IdentityRole<int>>().HasData(new List<IdentityRole<int>>
        {
            new IdentityRole<int> {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole<int> {
                Id = 2,
                Name = "User",
                NormalizedName = "USER"
            }
        });
        
        var hasher = new PasswordHasher<User>();
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1, // primary key
                FirstName = "main",
                LastName = "admin",
                UserName = "admin",
                Email = "admin@example.com",
                PasswordHash = hasher.HashPassword(null, "admin")
            },
            new User
            {
                Id = 2, // primary key
                FirstName = "default",
                LastName = "user",
                UserName = AuthorizationConst.default_username,
                Email = AuthorizationConst.default_email,
                PasswordHash = hasher.HashPassword(null, AuthorizationConst.default_password)
            }
        );
        
        modelBuilder.Entity<IdentityUserRole<int>>().HasData(
            new IdentityUserRole<int>
            {
                RoleId = 1, // for admin username
                UserId = 1,// for admin role
            },
            new IdentityUserRole<int>
            {
                RoleId = 2, // for admin username
                UserId = 2, // for admin role
            }
        );

        modelBuilder.Entity<Exercise>().HasData(
            new Exercise{
                ExerciseId = 1,
                UserId = null,
                Name = "Pull ups",
                Category = "Strength",
                MuscleGroups = "back, arms",
                Description = ""
            },
            new Exercise{
                ExerciseId = 2,
                UserId = null,
                Name = "Squats",
                Category = "Strength",
                MuscleGroups = "Quadriceps, Glutes",
                Description = ""
            },
            new Exercise{
                ExerciseId = 3,
                UserId = null,
                Name = "Dips",
                Category = "Strength",
                MuscleGroups = "Triceps, Chest, Shoulders",
                Description = ""
            },
            new Exercise{
                ExerciseId = 4,
                UserId = null,
                Name = "Biceps curls",
                Category = "Strength",
                MuscleGroups = "Biceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 5,
                UserId = null,
                Name = "Deadlift",
                Category = "Strength",
                MuscleGroups = "Hamstring, Glutes, Quadriceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 6,
                UserId = null,
                Name = "Running",
                Category = "Cardio",
                MuscleGroups = "legs",
                Description = ""
            },
            new Exercise{
                ExerciseId = 7,
                UserId = null,
                Name = "Swimming",
                Category = "Cardio",
                MuscleGroups = "Upper body",
                Description = ""
            },
            new Exercise{
                ExerciseId = 8,
                UserId = null,
                Name = "Bicycle",
                Category = "Cardio",
                MuscleGroups = "Quadriceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 9,
                UserId = null,
                Name = "Walking",
                Category = "Cardio",
                MuscleGroups = "Whole body",
                Description = ""
            },
            new Exercise{
                ExerciseId = 10,
                UserId = null,
                Name = "Rowing",
                Category = "Cardio",
                MuscleGroups = "Whole body",
                Description = ""
            },
            new Exercise{
                ExerciseId = 11,
                UserId = null,
                Name = "Bench press",
                Category = "Strength",
                MuscleGroups = "Chest, Triceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 12,
                UserId = null,
                Name = "Bent-over row",
                Category = "Strength",
                MuscleGroups = "Chest, Triceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 13,
                UserId = null,
                Name = "Romanian deadlifts",
                Category = "Strength",
                MuscleGroups = "Hamstring, Glutes",
                Description = ""
            },
            new Exercise{
                ExerciseId = 14,
                UserId = null,
                Name = "Split squats",
                Category = "Strength",
                MuscleGroups = "Quadriceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 15,
                UserId = null,
                Name = "Bulgarian split squats",
                Category = "Strength",
                MuscleGroups = "Quadriceps, Glutes",
                Description = ""
            },
            new Exercise{
                ExerciseId = 16,
                UserId = null,
                Name = "Standing overhead press",
                Category = "Strength",
                MuscleGroups = "Shoulders, triceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 17,
                UserId = null,
                Name = "Hammer curls",
                Category = "Strength",
                MuscleGroups = "Shoulders, triceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 18,
                UserId = null,
                Name = "Pistol squats",
                Category = "Strength",
                MuscleGroups = "Quadriceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 19,
                UserId = null,
                Name = "Wrist curls",
                Category = "Strength",
                MuscleGroups = "Forearm",
                Description = ""
            },
            new Exercise{
                ExerciseId = 20,
                UserId = null,
                Name = "Muscle ups",
                Category = "Strength",
                MuscleGroups = "Upper body",
                Description = ""
            }
        );
        
        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
