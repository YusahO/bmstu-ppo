using MewingPad.Common.Entities;

namespace MewingPad.Database.Models.Converters;

public static class CommentaryConverter
{
    public static Commentary? DbToCoreModel(CommentaryDbModel? model)
    {
        return model is not null
               ? new(id: model.Id,
                     authorId: model.AuthorId,
                     audiofileId: model.AudiofileId,
                     text: model.Text)
               : default;
    }

    public static CommentaryDbModel? CoreToDbModel(Commentary? model)
    {
        return model is not null
               ? new(id: model.Id,
                     authorId: model.AuthorId,
                     audiofileId: model.AudiofileId,
                     text: model.Text)
               : default;
    }
}