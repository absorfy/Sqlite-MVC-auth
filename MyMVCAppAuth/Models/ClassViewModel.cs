using System.ComponentModel.DataAnnotations;

namespace MyMVCApp.Models;

public class ClassViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Назва обов'язкова")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Назва повинна бути довжиною від 2 до 50 символів")]
    [Display(Name = "Назва", Description = "Введіть назву класу")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Назва обов'язкова")]
    [StringLength(200, MinimumLength = 10, ErrorMessage = "Опис повиннен бути довжиною від 10 до 200 символів")]
    [Display(Name = "Опис", Description = "Введіть опис класу")]
    public string Description { get; set; } = string.Empty;
    
    public ICollection<int> HeroIds { get; set; } = new List<int>();
}