﻿using MewingPad.Common.Entities;

namespace MewingPad.Database.Models.Converters;

public static class UserConverter
{
    public static User? DbToCoreModel(UserDbModel? model)
    {
        return model is not null
               ? new(id: model.Id,
                     favouritesId: model.FavouritesId,
                     username: model.Username,
                     email: model.Email,
                     passwordHashed: model.PasswordHashed,
                     isAdmin: model.IsAdmin,
                     isAuthorized: model.IsAuthorized)
               : default;
    }

    public static UserDbModel? CoreToDbModel(User? model)
    {
        return model is not null
               ? new(id: model.Id,
                     favouritesId: model.FavouritesId,
                     username: model.Username,
                     email: model.Email,
                     passwordHashed: model.PasswordHashed,
                     isAdmin: model.IsAdmin,
                     isAuthorized: model.IsAuthorized)
               : default;
    }
}