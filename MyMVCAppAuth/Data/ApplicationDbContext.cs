using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyMVCAppAuth.Entities;

namespace MyMVCAppAuth.Data;

public class ApplicationDbContext : IdentityDbContext
{
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

        base.OnModelCreating(modelBuilder);
    }
    
    public Task<List<HeroEntity>> GetHeroesAsync()
    {
        return Heroes
            .Include(h => h.Class)
            .Include(h => h.Skills).ToListAsync();
    }

    public Task<List<ClassEntity>> GetClassesAsync()
    {
        return Classes
            .Include(c => c.Heroes)
            .ToListAsync();
    }
    
    public Task<List<SkillEntity>> GetSkillsAsync()
    {
        return Skills
            .Include(s => s.Heroes)
            .ToListAsync();
    }
}