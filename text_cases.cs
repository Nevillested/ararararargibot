using System;
using System.IO;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace ararararargibot
{
    internal class text_cases
    {
        public static async void send_answer_for_cur_text(MessageEventArgs e)
        {
            var msg = e.Message;
            string msg_text = "";
            if (msg.Type == MessageType.Text)
            {
                msg_text = msg.Text.ToLower();
                msg_text = msg_text.Replace("@ararararagibot", "");
            }

            //пишем историю
            queries_to_bd.insert_story(msg.Text, msg.From.Username, msg.MessageId, msg.Chat.Id, null);

            //условия
            if (msg_text != null)
            {
                Console.WriteLine($"Пришло сообщение от {msg.From.Username} с текстом: {msg.Text}");

                if (msg_text.Contains("/help") || msg_text.Contains("/start"))
                {
                    await Program.client.SendTextMessageAsync(msg.Chat.Id, "Помощь немощу, сам же догадаться не можешь интуитивно да:" +
                    "\r\n/stick - рандомный стикер с Шинобу" +
                    "\r\n/pikcha - рандомная пикча с Шинобу" +
                    "\r\n/maid - шикарные несколько артов горничных" +
                    "\r\n/encrypt - зашифруй данне по ключу" +
                    "\r\n/decrypt - расшифруй данные по ключу" +
                    "\r\n/rand - реши свою судьбу: да/нет" +
                    "\r\n/anekdot - рандомный анекдот из сборника, честно спизженного с просторов нашего необъятного" +
                    "\r\n/delete_space - я не знаю зачем, но пусть будет - удаляет пробелы");
                }

                else if (msg_text == "/stick")
                {
                    Random rnd1 = new Random();
                    int value1 = rnd1.Next(1, 12);
                    var stic = await Program.client.SendStickerAsync(chatId: msg.Chat.Id, sticker: "https://cdn.tlgrm.app/stickers/34e/704/34e704f5-0115-3c1e-954e-74d2b180751d/192/" + value1 + ".webp", replyToMessageId: msg.MessageId);
                }

                else if (msg_text == "/pikcha")
                {
                    await Program.client.SendTextMessageAsync(msg.Chat.Id, "Ну ты изврат");
                    string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "/data/arts/shinobu");
                    Random rnd3 = new Random();
                    int value3 = rnd3.Next(0, allfiles.Length);
                    string path = @allfiles[value3];
                    try
                    {
                        using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            var imag = await Program.client.SendPhotoAsync(
                            chatId: msg.Chat.Id,
                            photo: new InputOnlineFile(fileStream));
                        }
                    }
                    catch
                    {
                        Exception exception = null;
                        queries_to_bd.insert_story("текст: " + msg.Text + "адрес выдаваемой пикчи:" + path, msg.From.Username, msg.MessageId, msg.Chat.Id, exception);
                    }
                }

                else if (msg_text == "/maid")
                {
                    string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "/data/arts/maids");
                    Random rnd3 = new Random();
                    int value3 = rnd3.Next(0, allfiles.Length);
                    string path = @allfiles[value3];
                    using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var imag = await Program.client.SendPhotoAsync(
                        chatId: msg.Chat.Id,
                        photo: new InputOnlineFile(fileStream));
                    }
                }

                else if (msg_text == "/rand")
                {
                    Random rnd3 = new Random();
                    int value3 = rnd3.Next(0, 10);
                    if (value3 < 5)
                    {
                        await Program.client.SendTextMessageAsync(msg.Chat.Id, "Да!");
                    }
                    else
                    {
                        await Program.client.SendTextMessageAsync(msg.Chat.Id, "Нет!");
                    }
                }

                else if (msg_text == "/anekdot")
                {
                    string joke = queries_to_bd.get_joke();
                    await Program.client.SendTextMessageAsync(msg.Chat.Id, joke.Replace("\\r\\n", "\r\n"));
                }

                else if (msg.Text == "/delete_space")
                {
                    await Program.client.SendTextMessageAsync(msg.Chat.Id, "Введи то, где надо убрать все пробелы", replyMarkup: new ForceReplyMarkup { Selective = true });
                }

                else if (msg_text == "/encrypt")
                {
                    await Program.client.SendTextMessageAsync(msg.Chat.Id, "Введите шифруемый текст на кириллице", replyMarkup: new ForceReplyMarkup { Selective = true });
                }

                else if (msg_text == "/decrypt")
                {
                    await Program.client.SendTextMessageAsync(msg.Chat.Id, "Введите дешифруемый текст на кириллице", replyMarkup: new ForceReplyMarkup { Selective = true });
                }

                else if (msg_text.Contains("лучшая девочка"))
                {
                    string[] shinobu = { "Шинобу лучшая девочка, товарищ старший лейтенант!", "Шинобу.", "Однозначно Шинобу!", "Лучшая девочка-та, ради кого я создан, это Шинобу!", "Шинобу", "Солышко мое Шинобу", "А я уже кидал пикчу с Шинобу?", "А я уже кидал стикос с Шинобу?" };
                    Random rnd1 = new Random();
                    int value = rnd1.Next(1, shinobu.Length);
                    await Program.client.SendTextMessageAsync(msg.Chat.Id, shinobu[value], replyToMessageId: msg.MessageId);
                }

                else if (msg_text.Contains("спать"))
                {
                    string[] sleep = { "поспать это хорошо", "ну если @g1ts0 ляжет, то видимо и мне придется", "погнали", "ура, мы идем спать" };
                    Random rnd = new Random();
                    int value = rnd.Next(1, sleep.Length);
                    await Program.client.SendTextMessageAsync(msg.Chat.Id, sleep[value], replyToMessageId: msg.MessageId);
                }

                //выдает логины всех пользователей
                else if (msg_text == "/get_users" && msg.Chat.Id == 1275894304)
                {
                    string users_name = queries_to_bd.get_users();
                    users_name = users_name.Replace("\\r\\n", "\r\n");
                    await Program.client.SendTextMessageAsync(msg.Chat.Id, users_name, replyToMessageId: msg.MessageId);
                }
                //принимает логин пользователя, ищет ид его чата и отправляет сообщение
                else if (msg_text.Contains("/send_@") && msg.Chat.Id == 1275894304)
                {
                    msg_text = msg_text.Replace("/send_@", ""); //обрезаем сообщение, оставляя ник пользователя без "@" и само сообщение
                    int idx_end_nickname = msg_text.IndexOf(' '); //ищем индекс пробела, чтобы понять, где кончается ник и начинается сообщение
                    string rst_nikname = msg_text.Substring(0, msg_text.Length - (msg_text.Length - idx_end_nickname)); //вытаскиваем ник, кому отправляем 
                    int msg_chat_id = queries_to_bd.get_chat_id(rst_nikname); //по нику ищем в бд id с чатом пользователя 
                    string rst_msg = msg_text.Substring(idx_end_nickname); //вытаскиваем сообщение
                    await Program.client.SendTextMessageAsync(msg_chat_id, rst_msg); //отправляем
                }

            }

        }
    }
}
