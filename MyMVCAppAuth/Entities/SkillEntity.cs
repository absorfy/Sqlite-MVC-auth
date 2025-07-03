using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMVCAppAuth.Entities;

public class SkillEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Skill name must be between 2 and 50 characters.")]
    public string Name { get; set; }
    
    [Required]
    [Range(1, 10)]
    public int Level { get; set; }
    
    public ICollection<HeroEntity> Heroes { get; set; }
}