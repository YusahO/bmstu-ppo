using MewingPad.Common.Entities;

namespace MewingPad.Database.Models.Converters;

public static class ReportConverter
{
    public static Report? DbToCoreModel(ReportDbModel? model)
    {
        return model is not null
               ? new(id: model.Id,
                     authorId: model.AuthorId,
                     audiofileId: model.AudiofileId,
                     text: model.Text,
                     status: model.Status)
               : default;
    }

    public static ReportDbModel? CoreToDbModel(Report? model)
    {
        return model is not null
               ? new(id: model.Id,
                     authorId: model.AuthorId,
                     audiofileId: model.AudiofileId,
                     text: model.Text,
                     status: model.Status)
               : default;
    }
}