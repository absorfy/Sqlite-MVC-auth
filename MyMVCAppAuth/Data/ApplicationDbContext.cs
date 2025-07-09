using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyMVCAppAuth.Entities;

namespace MyMVCAppAuth.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<UserProfileEntity> UserProfiles { get; set; }
    public DbSet<HeroEntity> Heroes { get; set; }
    public DbSet<ClassEntity> Classes { get; set; }
    public DbSet<SkillEntity> Skills { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SkillEntity>()
            .HasIndex(s => s.Name)
            .IsUnique();
        
        modelBuilder.Entity<HeroEntity>()
            .HasIndex(h => h.Name)
            .IsUnique();
        
        modelBuilder.Entity<ClassEntity>()
            .HasIndex(c => c.Name)
            .IsUnique();
        
        modelBuilder.Entity<HeroEntity>()
            .HasOne(h => h.Class)
            .WithMany(c => c.Heroes)
            .HasForeignKey(h => h.ClassId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<SkillEntity>()
            .HasMany(s => s.Heroes)
            .WithMany(h => h.Skills)
            .UsingEntity(j => j.ToTable("HeroSkills"));

        // modelBuilder.Entity<UserProfileEntity>()
        //     .HasKey(up => up.UserId);
        //
        // modelBuilder.Entity<UserProfileEntity>()
        //     .HasOne<IdentityUser>(up => up.User)
        //     .WithOne()
        //     .HasForeignKey<UserProfileEntity>(up => up.UserId)
        //     .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
    
    public Task<List<HeroEntity>> GetHeroesAsync() =>
        Heroes
            .Include(h => h.Class)
            .Include(h => h.Skills).ToListAsync();

    public Task<List<ClassEntity>> GetClassesAsync() =>
        Classes
            .Include(c => c.Heroes)
            .ToListAsync();

    public Task<List<SkillEntity>> GetSkillsAsync() =>
        Skills
            .Include(s => s.Heroes)
            .ToListAsync();

    public Task<HeroEntity?> GetHeroById(int id)
    {
        return Heroes
            .Include(h => h.Class)
            .Include(h => h.Skills)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public Task<ClassEntity?> GetClassById(int id)
    {
        return Classes
            .Include(c => c.Heroes)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task<SkillEntity?> GetSkillById(int id)
    {
        return Skills
            .Include(s => s.Heroes)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}