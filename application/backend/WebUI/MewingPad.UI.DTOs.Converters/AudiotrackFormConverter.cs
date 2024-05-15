using System.Diagnostics.CodeAnalysis;
using MewingPad.Common.Entities;

namespace MewingPad.UI.DTOs.Converters;

public static class AudiotrackFormConverter
{
    [return: NotNullIfNotNull(nameof(model))]
    public static Audiotrack? DtoToCoreModel(AudiotrackFormDto? model)
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
    public static AudiotrackFormDto? CoreModelToDto(Audiotrack? model)
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