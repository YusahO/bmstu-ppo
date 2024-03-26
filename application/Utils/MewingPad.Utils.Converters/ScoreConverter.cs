using MewingPad.Common.Entities;
using MewingPad.Database.Models;

namespace MewingPad.Utils.Converters;

public static class ScoreConverter
{
    public static Score DbToCoreModel(ScoreDbModel? model)
    {
        return model is not null
               ? new(audiofileId: model.AudiofileId,
                     authorId: model.AuthorId,
                     value: model.Value)
               : default!;
    }

    public static ScoreDbModel CoreToDbModel(Score? model)
    {
        return model is not null
               ? new(audiofileId: model.AudiofileId,
                     authorId: model.AuthorId,
                     value: model.Value)
               : default!;
    }
}