using MewingPad.Common.Entities;

namespace MewingPad.Database.Models.Converters;

public static class ScoreConverter
{
    public static Score? DbToCoreModel(ScoreDbModel? model)
    {
        return model is not null
               ? new(audiofileId: model.AudiofileId,
                     authorId: model.AuthorId,
                     value: model.Value)
               : default;
    }

    public static ScoreDbModel? CoreToDbModel(Score? model)
    {
        return model is not null
               ? new(audiofileId: model.AudiofileId,
                     authorId: model.AuthorId,
                     value: model.Value)
               : default;
    }
}