from pprint import pprint
data = [
    [["Search", "Поиск аудиотрека"],
     [
        ["SearchByTitle", "По названию"],
        ["SearchByTags", "По тегу"]
     ]],
    [["AudiotrackActions", " Действия с аудиотреками"],
     [
         ["ViewAllAudiotracks", "Просмотреть все аудиотреки"],
         ["DownloadAudiotrack", "Скачать"],
         ["AddScore", "Поставить оценку"],
         ["ViewAudiotrackCommentaries", "Просмотреть комментарии"],
         ["AddCommentary", "Написать комментарий"],
         ["EditCommentary", "Изменить комментарий"],
         ["DeleteCommentary", "Удалить комментарий"],
         ["ReportAudiotrack", "Пожаловаться"],
         ["UploadAudiotrack", "Добавить новый"],
         ["ChangeAudiotrack", "Изменить"],
         ["DeleteAudiotrack", "Удалить"]
     ]],
    # [[" Действия с плейлистами"],
    #  [
    #      "Просмотреть свои плейлисты",
    #      "Создать",
    #      "Переименовать",
    #      "Удалить",
    #      "Добавить аудиотрек в плейлист",
    #      "Удалить аудиотрек(и) из плейлиста"
    #  ]],
    [["ReportActions", " Действия с жалобами"],
     [
        ["ViewAllReports", "Просмотреть все жалобы"],
        ["ChangeReportStatus", "Изменить статус жалобы"]
     ]],
    [["TagActions", " Действия с тегами"],
     [
        ["ViewAllTags", "Просмотреть все теги"],
        ["AddTag", "Добавить"],
        ["ChangeTag", "Изменить"],
        ["DeleteTag", "Удалить"]
     ]],
]

import os
def format_command(label, name, desc):
    return f'''
using MewingPad.TechnicalUI.BaseMenu;

namespace MewingPad.TechnicalUI.AdminMenu.{label};

public class {name}Command : Command
{{
    public override string? Announce()
    {{
        return "{desc}";
    }}

    public override Task Execute(Context context)
    {{
        throw new NotImplementedException();
    }}
}}
'''

for dir_ in data:
    dir_eng, dir_ru = dir_[0]
    for entr_eng, entr_ru in dir_[1]:
        # print(f'{dir_eng}/{entr_eng}Command.cs')
        f = open(f'{dir_eng}/{entr_eng}Command.cs', 'w', encoding='UTF-8')
        print(format_command(dir_eng.strip(), entr_eng.strip(), entr_ru.strip()), file=f)
        f.close()
# pprint(data)
