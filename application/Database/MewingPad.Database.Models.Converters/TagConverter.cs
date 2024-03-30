using MewingPad.Common.Entities;

namespace MewingPad.Database.Models.Converters;

public static class TagConverter
{
    public static Tag? DbToCoreModel(TagDbModel? model)
    {
        return model is not null
               ? new(id: model.Id,
                     authorId: model.AuthorId,
                     name: model.Name)
               : default;
    }

    public static TagDbModel? CoreToDbModel(Tag? model)
    {
        return model is not null
               ? new(id: model.Id,
                     authorId: model.AuthorId,
                     name: model.Name)
               : default;
    }
}