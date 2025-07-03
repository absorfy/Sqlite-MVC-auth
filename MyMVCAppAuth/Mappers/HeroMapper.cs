using MyMVCApp.Models;
using MyMVCAppAuth.Entities;
using MyMVCAppAuth.Models;

namespace MyMVCApp.Mappers;

public class HeroMapper
{
    public static HeroEntity ToEntity(HeroViewModel model, ICollection<SkillEntity>? allSkills = null)
    {
        return new HeroEntity()
        {
            Id = model.Id,
            Name = model.Name,
            ClassId = model.ClassId,
            ImageUrl = model.ImageUrl,
            Skills = allSkills
                ?.Where(skill => model.SkillIds.Contains(skill.Id))
                .ToList() ?? []
        };
    }

    public static HeroViewModel ToViewModel(HeroEntity entity)
    {
        return new HeroViewModel()
        {
            Id = entity.Id,
            Name = entity.Name,
            ClassId = entity.ClassId,
            ImageUrl = entity.ImageUrl,
            SkillIds = entity.Skills?.Select(skill => skill.Id).ToList() ?? []
        };
    }
}