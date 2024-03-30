using MewingPad.Common.Entities;
using MewingPad.Database.Models;

namespace MewingPad.Database.Models.Converters;

public static class AudiofileConverter
{
    public static Audiofile? DbToCoreModel(AudiofileDbModel? model)
    {
        return model is not null
               ? new(id: model.Id,
                     title: model.Title,
                     duration: model.Duration,
                     authorId: model.AuthorId,
                     filepath: model.Filepath)
               : default;
    }

    public static AudiofileDbModel? CoreToDbModel(Audiofile? model)
    {
        return model is not null
               ? new(id: model.Id,
                     title: model.Title,
                     duration: model.Duration,
                     authorId: model.AuthorId,
                     filepath: model.Filepath)
               : default;
    }
}