using System.Diagnostics.CodeAnalysis;
using MewingPad.Common.Entities;

namespace MewingPad.Database.Models.Converters;

public static class AudiofileConverter
{
    [return: NotNullIfNotNull(nameof(model))]
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

    [return: NotNullIfNotNull(nameof(model))]
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