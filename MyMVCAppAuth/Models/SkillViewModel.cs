using System.ComponentModel.DataAnnotations;

namespace MyMVCApp.Models;

public class SkillViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Назва обов'язкова")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Назва повинна бути довжиною від 2 до 50 символів")]
    [Display(Name = "Назва", Description = "Введіть назву навички")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Рівень обов'язковий")]
    [Range(1, 10, ErrorMessage = "Рівень повинен бути від 1 до 10")]
    [Display(Name = "Рівень", Description = "Введіть рівень навички")]
    public int Level { get; set; } = 1;
    
    public ICollection<int> HeroIds { get; set; } = new List<int>();
}