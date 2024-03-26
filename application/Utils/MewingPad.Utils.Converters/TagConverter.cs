using MewingPad.Common.Entities;
using MewingPad.Database.Models;

namespace MewingPad.Utils.Converters;

public static class TagConverter
{
    public static Tag DbToCoreModel(TagDbModel? model)
    {
        return model is not null
               ? new(id: model.Id,
                     authorId: model.AuthorId,
                     name: model.Name)
               : default!;
    }

    public static TagDbModel CoreToDbModel(Tag? model)
    {
        return model is not null
               ? new(id: model.Id,
                     authorId: model.AuthorId,
                     name: model.Name)
               : default!;
    }
}