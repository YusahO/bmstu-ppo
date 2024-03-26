using MewingPad.Common.Entities;
using MewingPad.Database.Models;

namespace MewingPad.Utils.Converters;

public static class ReportConverter
{
    public static Report DbToCoreModel(ReportDbModel? model)
    {
        return model is not null
               ? new(id: model.Id,
                     authorId: model.AuthorId,
                     audiofileId: model.AudiofileId,
                     text: model.Text,
                     status: model.Status)
               : default!;
    }

    public static ReportDbModel CoreToDbModel(Report? model)
    {
        return model is not null
               ? new(id: model.Id,
                     authorId: model.AuthorId,
                     audiofileId: model.AudiofileId,
                     text: model.Text,
                     status: model.Status)
               : default!;
    }
}