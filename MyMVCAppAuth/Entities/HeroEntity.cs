using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMVCAppAuth.Entities;

public class HeroEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Model must be between 2 and 50 characters.")]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public int ClassId { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    public string? ImageUrl { get; set; }

    [ForeignKey(nameof(ClassId))] public ClassEntity? Class { get; set; }
    
    public ICollection<SkillEntity> Skills { get; set; } = new List<SkillEntity>();
    
}