using System.ComponentModel.DataAnnotations;

namespace MyMVCAppAuth.Models;

public class HeroViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Ім'я обов'язкове")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Ім'я повинно бути довжиною від 2 до 50 символів")]
    [Display(Name = "Ім'я", Description = "Введіть ім'я героя")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Клас обов'язковий")]
    [Display(Name = "Клас", Description = "Оберіть клас героя")]
    public int ClassId { get; set; }
    
    [Display(Name = "Зображення")]
    public string? ImageUrl { get; set; }
    
    [Display(Name = "Зображення", Description = "Завантажте зображення героя")]
    public IFormFile? HeroImageFile { get; set; }
    
    [Display(Name="Навички", Description = "Оберіть навички героя")]
    public ICollection<int> SkillIds { get; set; } = new List<int>();
}