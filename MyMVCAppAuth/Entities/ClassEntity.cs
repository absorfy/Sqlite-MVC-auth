using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMVCAppAuth.Entities;

public class ClassEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Class name must be between 2 and 50 characters.")]
    public string Name { get; set; }
    
    [Required]
    [StringLength(200, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 200 characters.")]
    public string Description { get; set; }
    
    public ICollection<HeroEntity> Heroes { get; set; } = new List<HeroEntity>();
    
}