using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ararararargibot
{
    internal class reply_triggers
    {
        public static async void send_answer_for_reply(MessageEventArgs e)
        {
            var msg = e.Message;
            string msg_text = "";

            if (msg.Type == MessageType.Text)
            {
                msg_text = msg.Text.ToLower();
                msg_text = msg_text.Replace("@ararararagibot", "");
            }

            if (msg.ReplyToMessage != null)
            {
                //триггер на удаление пробелов
                if (msg.ReplyToMessage.Text.Contains("Введи то, где надо убрать все пробелы"))
                {
                    string delete_space = msg.Text.Replace(" ", "");

                    await Program.client.SendTextMessageAsync(msg.Chat.Id, delete_space);
                }

                //триггер на шифрование по Цезарю
                if (msg.ReplyToMessage.Text.Contains("Введите шифруемый текст на кириллице") || msg.ReplyToMessage.Text.Contains("Введите числовой ключ для шифровки"))
                {
                    if (msg.ReplyToMessage.Text.Contains("Введите шифруемый текст на кириллице"))
                    {
                        //вставка шифруемого текста
                        cezar.save_user_msg(msg.Chat.Id, msg_text);
                        await Program.client.SendTextMessageAsync(msg.Chat.Id, "Введите числовой ключ для шифровки", replyMarkup: new ForceReplyMarkup { Selective = true });
                    }
                    else if (msg.ReplyToMessage.Text.Contains("Введите числовой ключ для шифровки"))
                    {
                        cezar.save_key(msg.Chat.Id, Convert.ToInt32(msg_text));
                        //отдача результата
                        await Program.client.SendTextMessageAsync(msg.Chat.Id, "Зашифрованный текст:\r\n" + cezar.encrypt(msg.Chat.Id), replyToMessageId: msg.MessageId);
                    }
                }

                //триггер на дешифрование по Цезарю
                if (msg.ReplyToMessage.Text.Contains("Введите дешифруемый текст на кириллице") || msg.ReplyToMessage.Text.Contains("Введите числовой ключ для дешифровки"))
                {
                    if (msg.ReplyToMessage.Text.Contains("Введите дешифруемый текст на кириллице"))
                    {
                        //вставка шифруемого текста
                        cezar.save_user_msg(msg.Chat.Id, msg_text);
                        await Program.client.SendTextMessageAsync(msg.Chat.Id, "Введите числовой ключ для дешифровки", replyMarkup: new ForceReplyMarkup { Selective = true });
                    }
                    else if (msg.ReplyToMessage.Text.Contains("Введите числовой ключ для дешифровки"))
                    {
                        cezar.save_key(msg.Chat.Id, Convert.ToInt32(msg_text));
                        //отдача результата
                        await Program.client.SendTextMessageAsync(msg.Chat.Id, "Расшифрованный текст:\r\n" + cezar.decrypt(msg.Chat.Id), replyToMessageId: msg.MessageId);
                    }
                }
            }
        }
    }
}
