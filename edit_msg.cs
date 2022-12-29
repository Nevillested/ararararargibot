using System;
using Telegram.Bot.Args;

namespace ararararargibot
{
    internal class edit_msg
    {
        public static async void send_answer_for_edit_msg(MessageEventArgs e)
        {
            var msg = e.Message;
            try
            {
                //добавляем в бд изменное сообщение
                queries_to_bd.update_story(msg.Text, msg.MessageId, msg.Chat.Id, null);

                //забираем из бд первоначальное сообщение
                string old_msg = queries_to_bd.get_first_msg(msg.Text, msg.MessageId, msg.Chat.Id);

                //добавляем к сообщению спойлер, который распарсится через MarkdownV2
                old_msg = "|| '" + old_msg + "' ||";
                await Program.client.SendTextMessageAsync(msg.Chat.Id,
                              "Ты что думаешь, я ничего не видел?\r\n" + old_msg,
                              parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                              replyToMessageId: msg.MessageId
                              );
            }
            catch (Exception ex)
            {
                //пишем ошибки
                queries_to_bd.insert_story(msg.Text, msg.From.Username, msg.MessageId, msg.Chat.Id, ex);
            }
        }
    }
}
