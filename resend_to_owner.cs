using System;
using System.IO;
using System.Reflection.Metadata;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using static System.Net.WebRequestMethods;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading.Tasks;
using File = System.IO.File;
using System.IO.Pipes;

namespace ararararargibot
{
    internal class resend_to_owner
    {
        public static async void sending(MessageEventArgs e)
        {
            var msg = e.Message;
            long id_owner = 1275894304;
            if (msg.Chat.Id != id_owner)
            {
                await Program.client.SendTextMessageAsync(id_owner, msg.From.Username + ":");

                if (msg.Type == MessageType.Text) //пересылка текста
                {
                    await Program.client.SendTextMessageAsync(id_owner, "'" + msg.Text + "'");
                }
                else if (msg.Type == MessageType.Sticker) //пересылка стикера
                {
                    await Program.client.SendStickerAsync(chatId: id_owner, sticker: msg.Sticker.FileId);
                }
                else if (msg.Type == MessageType.Voice) //пересылка войса
                {
                    await Program.client.SendVoiceAsync(chatId: id_owner, voice: msg.Voice.FileId);
                }
                else if (msg.Type == MessageType.Photo) //пересылка фото
                {
                    /*чтобы переслать фото, его надо сначала сохранить, затем отправить*/
                    string path = Directory.GetCurrentDirectory() + "/data/temp_photo";
                    /*сохраняем*/
                    var download_img = await Program.client.GetFileAsync(msg.Photo[msg.Photo.Length - 1].FileId);
                    FileStream fs = new FileStream(path, FileMode.Create);
                    await Program.client.DownloadFileAsync(download_img.FilePath, fs);
                    fs.Close();
                    fs.Dispose();

                    /*отправляем*/
                    using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        await Program.client.SendPhotoAsync(chatId: id_owner, photo: new InputOnlineFile(fileStream));
                    }

                    /*удаляем*/
                    File.Delete(path);
                }
                else if (msg.Type == MessageType.VideoNote)
                {
                    /*чтобы переслать видео круг, его надо сначала сохранить, затем отправить*/
                    string path = Directory.GetCurrentDirectory() + "/data/temp_video_note";

                    /*сохраняем*/
                    var download_vn = await Program.client.GetFileAsync(msg.VideoNote.FileId);
                    FileStream fs = new FileStream(path, FileMode.Create);
                    await Program.client.DownloadFileAsync(download_vn.FilePath, fs);
                    fs.Close();
                    fs.Dispose();

                    /*отправляем*/
                    using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        await Program.client.SendVideoNoteAsync(chatId: id_owner, videoNote: fileStream);
                    }

                    /*удаляем*/
                    File.Delete(path);
                }
                else if (msg.Type == MessageType.Video)
                {
                    /*чтобы переслать видео, его надо сначала сохранить, затем отправить*/
                    string path = Directory.GetCurrentDirectory() + "/data/" + msg.Video.FileName;

                    /*сохраняем*/
                    var download_vn = await Program.client.GetFileAsync(msg.Video.FileId);
                    FileStream fs = new FileStream(path, FileMode.Create);
                    await Program.client.DownloadFileAsync(download_vn.FilePath, fs);
                    fs.Close();
                    fs.Dispose();

                    /*отправляем*/
                    using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        await Program.client.SendVideoAsync(chatId: id_owner, video: fileStream);
                    }

                    /*удаляем*/
                    File.Delete(path);
                }
                else if (msg.Type == MessageType.Document)
                {
                    /*чтобы переслать документ, его надо сначала сохранить, затем отправить*/
                    string path = Directory.GetCurrentDirectory() + "/data/" + msg.Document.FileName;

                    /*сохраняем*/
                    var download_vn = await Program.client.GetFileAsync(msg.Document.FileId);
                    FileStream fs = new FileStream(path, FileMode.Create);
                    await Program.client.DownloadFileAsync(download_vn.FilePath, fs);
                    fs.Close();
                    fs.Dispose();

                    /*отправляем*/
                    using (var stream = File.OpenRead(path))
                    {
                        Telegram.Bot.Types.InputFiles.InputOnlineFile iof = new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream);
                        iof.FileName = msg.Document.FileName;
                        await Program.client.SendDocumentAsync(id_owner, iof);
                    }

                    /*удаляем*/
                    File.Delete(path);
                }
            }
        }
    }
}
