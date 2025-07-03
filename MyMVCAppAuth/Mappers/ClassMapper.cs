using MyMVCApp.Models;
using MyMVCAppAuth.Entities;

namespace MyMVCAppAuth.Mappers;

public class ClassMapper
{
    public static ClassEntity ToEntity(ClassViewModel model, ICollection<HeroEntity>? allHeroes = null)
    {
        return new ClassEntity
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            CreatedAt = DateTime.Now,
            Heroes = allHeroes
                ?.Where(h => model.HeroIds.Contains(h.Id))
                .ToList() ?? []
        };
    }

    public static ClassEntity ToEntity(ClassEntity entity, ClassViewModel model, ICollection<HeroEntity>? allHeroes = null)
    {
        if (entity.Id != model.Id)
        {
            throw new Exception("Class IDs do not match.");
        }
        
        entity.Description = model.Description;
        entity.Name = model.Name;
        entity.Heroes.Clear();
        entity.Heroes = allHeroes
            ?.Where(h => model.HeroIds.Contains(h.Id))
            .ToList() ?? [];
        
        return entity;
    }

    public static ClassViewModel ToViewModel(ClassEntity entity)
    {
        return new ClassViewModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            HeroIds = entity.Heroes.Select(h => h.Id).ToList() ?? []
        };
    }
}
