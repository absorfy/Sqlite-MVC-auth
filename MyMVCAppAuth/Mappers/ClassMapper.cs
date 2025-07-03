using MyMVCApp.Models;
using MyMVCAppAuth.Entities;

namespace MyMVCApp.Mappers;

public class ClassMapper
{
    public static ClassEntity ToEntity(ClassViewModel model, ICollection<HeroEntity>? allHeroes = null)
    {
        return new ClassEntity
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Heroes = allHeroes
                ?.Where(h => model.HeroIds.Contains(h.Id))
                .ToList() ?? []
        };
    }

    public static ClassViewModel ToViewModel(ClassEntity entity)
    {
        return new ClassViewModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            HeroIds = entity.Heroes?.Select(h => h.Id).ToList() ?? []
        };
    }
}
