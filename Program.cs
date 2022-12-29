using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace ararararargibot
{
    public static class Program
    {
        private static string token { get; set; } = "2024376867:AAEwo60MQbbuvTAFMxCC_orH1t7Xyduj5So";
        public  static TelegramBotClient client;
        public  static DateTime Now { get; }
        
        //основной работающий метод
        static void Main(string[] args)
        {
            try
            {
                client = new TelegramBotClient(token);
                client.StartReceiving();
                client.OnMessage += OnMessageHandler;
                client.OnMessageEdited += OnMessageEdit;
                Console.ReadLine();
                client.StopReceiving();
            }
            catch (Exception ex)
            {
                //пишем ошибки
                queries_to_bd.insert_story("null", "null", -1, -1, ex);
            }
        }

        //действия на все прилетающие сообщения
        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            //пересылка всех входящих сообщений только мне
            resend_to_owner.sending(e);

            try
            {
                //условия отправки ответов и сама отправка на прилетевшее текстовое сообщение
                text_cases.send_answer_for_cur_text(e);

                //триггеры на реплайные сообщения
                reply_triggers.send_answer_for_reply(e);
            }
            catch (Exception ex)
            {
                //пишем ошибки
                queries_to_bd.insert_story(e.Message.Text, e.Message.From.Username, e.Message.MessageId, e.Message.Chat.Id, ex);
            }
        }

        //действия на редактированные сообщения
        private static async void OnMessageEdit(object sender, MessageEventArgs e)
        {
            edit_msg.send_answer_for_edit_msg(e);
        }
    }
}