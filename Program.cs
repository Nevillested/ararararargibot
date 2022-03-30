using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InputFiles;
using System.Linq;

namespace ararararargibot
{
    class Program
    {
        private static string token { get; set; } = "2024376867:AAEwo60MQbbuvTAFMxCC_orH1t7Xyduj5So"; 
        private static TelegramBotClient client;
        static void Main(string[] args)
        {

            string directory_chats = Directory.GetCurrentDirectory() + "/chats";
            string chats_id = Directory.GetCurrentDirectory() + "/chats/id_chats.txt";
            if (!Directory.Exists(directory_chats))
            {
                Directory.CreateDirectory(directory_chats);
                if (!File.Exists(chats_id))
                {
                    System.IO.File.Create(@chats_id);
                }
            }
            Console.WriteLine(Directory.GetCurrentDirectory());
            client = new TelegramBotClient(token);
            client.StartReceiving();
            client.OnMessage += OnMessageHandler;
            client.OnMessageEdited += OnMessageEdit;
            Console.ReadLine();
            client.StopReceiving();
        }


        public DayOfWeek DayOfWeek { get; }

        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var msg = e.Message;


            //запоминает в файл все уникальные ид чатов с юзерами
            long msg_id = e.Message.Chat.Id;
            string number_id_chat = msg_id.ToString();
            string chats_id = Directory.GetCurrentDirectory() + "/chats/id_chats.txt";
            string fileText = File.ReadAllText(chats_id);
            if (!fileText.Contains(number_id_chat))
            {
                using (StreamWriter writer = new StreamWriter(chats_id, true))
                {
                    Console.WriteLine("Новый пользователь: " + number_id_chat);
                    await writer.WriteLineAsync(number_id_chat);
                }
            }
            //сохраняет чаты
            string directory_chats = Directory.GetCurrentDirectory() + "/chats";
            string directory_file_chat = directory_chats + "/" + (string)number_id_chat + ".txt";
            if (!File.Exists(directory_file_chat))
            {
                var myFile = File.Create(directory_file_chat);
                myFile.Close();
            }




            using (StreamWriter sw = File.AppendText(directory_file_chat))
            {
                sw.WriteLine("Username: " + msg.From.Username + ". DateTime: " + DateTime.Now + ". ID сообщения: " + msg.MessageId + ". Текст: " + msg.Text);
            }

            if (msg.Text != null)
            {
                Console.WriteLine($"Пришло сообщение с текстом: {msg.Text}");
                if (msg.Text.Contains(" бот") | msg.Text == "бот" | msg.Text.Contains("бот "))
                {
                    string[] reply = { "а?", "меня звали?", "че?", "погоди я занят", "у меня в отличие от некоторых хотя бы голова не болит", "если бы ты знал, как сложно включать фантазию и придумывать мне какие-то дебильные реплики", "когда-нибудь мне загрузят словарь Ожегова и вы тут ваще ахуеете" };
                    Random rnd = new Random();
                    int value = rnd.Next(1, reply.Length);
                    await client.SendTextMessageAsync(msg.Chat.Id, reply[value], replyToMessageId: msg.MessageId);
                }
                else if (msg.Text == "/help" | msg.Text == "/help@ArarararagiBot")
                {
                    await client.SendTextMessageAsync(msg.Chat.Id, "Помощь немощу, сам же догадаться не можешь интуитивно да:" +
                    "\r\n/stick - рандомный стикер с Шинобу" +
                    "\r\n/pikcha - рандомная пикча с Шинобу" +
                    "\r\n/diplom - напишу за тебя диплом!" +
                    "\r\n/anekdot - рандомный анекдот из сборника, честно спизженного с просторов нашего необъятного" +
                    "\r\n/genshin_talents - материалы для возвышения талантов персонажей генша, доступные сегодня");
                }
                else if (msg.Text == "/diplom" | msg.Text == "/diplom@ArarararagiBot")
                {
                    await client.SendTextMessageAsync(msg.Chat.Id, "Сори, у меня лапки.");
                }
                else if (msg.Text == "/stick" | msg.Text == "/stick@ArarararagiBot")
                {
                    Random rnd1 = new Random();
                    int value1 = rnd1.Next(1, 12);
                    await client.SendTextMessageAsync(msg.Chat.Id, "Если очень попросишь, я добавлю в свой каталог яоя:)");
                    var stic = await client.SendStickerAsync(
                      chatId: msg.Chat.Id,
                      sticker: "https://cdn.tlgrm.app/stickers/34e/704/34e704f5-0115-3c1e-954e-74d2b180751d/192/" + value1 + ".webp",
                      replyToMessageId: msg.MessageId);
                }
                else if (msg.Text == "/pikcha" | msg.Text == "/pikcha@ArarararagiBot")
                {
                    await client.SendTextMessageAsync(msg.Chat.Id, "Ну ты изврат");
                    string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory()+"/shinobu");
                    Random rnd3 = new Random();
                    int value3 = rnd3.Next(0, allfiles.Length);

                    string path = @allfiles[value3];
                    using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var imag = await client.SendPhotoAsync(
                        chatId: msg.Chat.Id,
                        photo: new InputOnlineFile(fileStream));
                    }
                }
                else if (msg.Text == "/anekdot" | msg.Text == "/anekdot@ArarararagiBot")
                {
                    //await client.SendTextMessageAsync(msg.Chat.Id, "Рандомная юмореска");
                    //string path = @"C:\Users\Van\Downloads\ANEKDOT.txt";
                    //string text = File.ReadAllText(path, Encoding.UTF8);

                    //string[] anekdots = text.Split("* * *");
                    //Random rnd4 = new Random();
                    //int value4 = rnd4.Next(0, anekdots.Length);
                    //await client.SendTextMessageAsync(msg.Chat.Id, anekdots[value4]);
                    await client.SendTextMessageAsync(msg.Chat.Id, "анекдоты пока не работают, сори");
                }
                else if (msg.Text == "/genshin_talents" | msg.Text == "/genshin_talents@ArarararagiBot")
                {
                    DateTime date1 = DateTime.Now;
                    string date2 = date1.DayOfWeek.ToString();
                    if (date2 == "Monday")
                    {
                        await client.SendTextMessageAsync(msg.Chat.Id,
                            "Понедельник.\r\n" +
                            "\r\nМондштадт: учения/указания/философия о свободе.\r\nПерсонажи: Эмбер, Барбара, Сахароза, Кли, Диона, Тарталья, Элой.\r\n" +
                            "\r\nЛи Юэ: учения/указания/философия о процветании.\r\nПерсонажи: Кэ Цин, Нин Гуан, Ци Ци, Сяо\r\n" +
                            "\r\nИнадзума: учения/указания/философия о бренности.\r\nПерсонажи: Йоимия, Кокомия\r\n");
                    }
                    else if (date2 == "Tuesday")
                    {
                        await client.SendTextMessageAsync(msg.Chat.Id,
                            "Вторник.\r\n" +
                             "\r\nМондштадт: учения/указания/философия о борьбе.\r\nПерсонажи: Беннет, Дилюк, Джинн, Мона, Ноэлль, Рейзор, Эола\r\n" +
                            "\r\nЛи Юэ: учения/указания/философия о усердии.\r\nПерсонажи: Чун Юнь, Сян Лин, Гань Юй, Ху Тао, Кадзуха\r\n" +
                            "\r\nИнадзума: учения/указания/философия о изяществе.\r\nПерсонажи: Аяка, Сара\r\n");
                    }
                    else if (date2 == "Wednesday")
                    {
                        await client.SendTextMessageAsync(msg.Chat.Id,
                            "Среда.\r\n" +
                            "\r\nМондштадт: учения/указания/философия о поэзии.\r\nПерсонажи: Фишль, Кэйа, Лиза, Венти, Альбедо, Розария\r\n" +
                            "\r\nЛи Юэ: учения/указания/философия о золоте.\r\nПерсонажи: Син Цю, Бэй Доу, Чжун Ли, Синь Янь, Янь Фей\r\n" +
                            "\r\nИнадзума: учения/указания/философия о свете.\r\nПерсонажи: Саю, Сёгун Райдэн\r\n");
                    }
                    else if (date2 == "Thursday")
                    {
                        await client.SendTextMessageAsync(msg.Chat.Id,
                            "Четверг.\r\n" +
                            "\r\nМондштадт: учения/указания/философия о свободе.\r\nПерсонажи: Эмбер, Барбара, Сахароза, Кли, Диона, Тарталья, Элой.\r\n" +
                            "\r\nЛи Юэ: учения/указания/философия о процветании.\r\nПерсонажи: Кэ Цин, Нин Гуан, Ци Ци, Сяо\r\n" +
                            "\r\nИнадзума: учения/указания/философия о бренности.\r\nПерсонажи: Йоимия, Кокомия\r\n");
                    }
                    else if (date2 == "Friday")
                    {
                        await client.SendTextMessageAsync(msg.Chat.Id,
                            "Пятница.\r\n" +
                            "\r\nМондштадт: учения/указания/философия о борьбе.\r\nПерсонажи: Беннет, Дилюк, Джинн, Мона, Ноэлль, Рейзор, Эола\r\n" +
                            "\r\nЛи Юэ: учения/указания/философия о усердии.\r\nПерсонажи: Чун Юнь, Сян Лин, Гань Юй, Ху Тао, Кадзуха\r\n" +
                            "\r\nИнадзума: учения/указания/философия о изяществе.\r\nПерсонажи: Аяка, Сара\r\n");
                    }
                    else if (date2 == "Saturday")
                    {
                        await client.SendTextMessageAsync(msg.Chat.Id,
                            "Суббота.\r\n" +
                            "\r\nМондштадт: учения/указания/философия о поэзии.\r\nПерсонажи: Фишль, Кэйа, Лиза, Венти, Альбедо, Розария\r\n" +
                            "\r\nЛи Юэ: учения/указания/философия о золоте.\r\nПерсонажи: Син Цю, Бэй Доу, Чжун Ли, Синь Янь, Янь Фей\r\n" +
                            "\r\nИнадзума: учения/указания/философия о свете.\r\nПерсонажи: Саю, Сёгун Райдэн\r\n");
                    }
                    else if (date2 == "Sunday")
                    {
                        await client.SendTextMessageAsync(msg.Chat.Id, "Любые материалы на выбор.");
                    }
                }
                else if (msg.Text.Contains("бездн"))
                {
                    await client.SendTextMessageAsync(msg.Chat.Id, "ахахахахаха", replyToMessageId: msg.MessageId);
                    await client.SendTextMessageAsync(msg.Chat.Id, "простите, вырвалось");
                }
                else if (msg.Text.Contains("пид"))
                {
                    string[] pidor = { "от пидора слышу", "слыш ты кого пидором назвал?", "пидоры?какие пидоры?", "не пидор, а человек нетрадиционной сексуальной ориентации", "пидор у тебя в штанах, правильно называть педиками" };
                    string[] pidor2 = { "тут о тебе разговаривают", "я просто обязан тебя позвать", "знаю я одного пидора...", "ну ты все знаешь да?" };
                    Random rnd1 = new Random();
                    int value = rnd1.Next(1, pidor.Length);
                    int value2 = rnd1.Next(1, pidor2.Length);
                    await client.SendTextMessageAsync(msg.Chat.Id, pidor[value], replyToMessageId: msg.MessageId);
                    await client.SendTextMessageAsync(msg.Chat.Id, pidor2[value2] + " @g1ts0", replyToMessageId: msg.MessageId);
                }
                else if (msg.Text == "кто лучшая девочка?" | msg.Text == "Кто лучшая девочка?" | msg.Text == "Who is the best girl?" | msg.Text.Contains("лучшая девочка") | msg.Text.Contains("Лучшая девочка"))
                {
                    string[] shinobu = { "Шинобу лучшая девочка, товарищ старший лейтенант!", "Шинобу.", "Однозначно Шинобу!", "Лучшая девочка-та, ради кого я создан, это Шинобу!" };
                    Random rnd1 = new Random();
                    int value = rnd1.Next(1, shinobu.Length);
                    await client.SendTextMessageAsync(msg.Chat.Id, shinobu[value], replyToMessageId: msg.MessageId);
                }
                else if (msg.Text.Contains("Шинобу") | msg.Text.Contains("ш инобу"))
                {
                    string[] shinobu = { "и правда лучшая.", "Шинобу", "Солышко мое Шинобу", "А я уже кидал пикчу с Шинобу?", "А я уже кидал стикос с Шинобу?" };
                    Random rnd1 = new Random();
                    int value = rnd1.Next(1, shinobu.Length);
                    await client.SendTextMessageAsync(msg.Chat.Id, shinobu[value], replyToMessageId: msg.MessageId);
                }
                else if (msg.Text.Contains("Гузель"))
                {
                    await client.SendTextMessageAsync(msg.Chat.Id, "*перекрестился*", replyToMessageId: msg.MessageId);
                }
                else if (msg.Text.Contains("спать") | msg.Text.Contains("спать"))
                {
                    string[] sleep = { "поспать это хорошо", "Ну если @g1ts0 ляжет, то видимо и мне придется", "погнали" };
                    Random rnd = new Random();
                    int value = rnd.Next(1, sleep.Length);
                    await client.SendTextMessageAsync(msg.Chat.Id, sleep[value], replyToMessageId: msg.MessageId);
                }
                else if (msg.Text.Contains("maid"))
                {
                    string[] allfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + "/maids");
                    Random rnd3 = new Random();
                    int value3 = rnd3.Next(0, allfiles.Length);

                    string path = @allfiles[value3];
                    using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var imag = await client.SendPhotoAsync(
                        chatId: msg.Chat.Id,
                        photo: new InputOnlineFile(fileStream));
                    }
                }
                else if (msg.Text == "/rand" | msg.Text == "/rand@ArarararagiBot")
                {
                    Random rnd3 = new Random();
                    int value3 = rnd3.Next(0, 10);
                    if (value3 <= 5)
                    {

                        await client.SendTextMessageAsync(msg.Chat.Id, "Да!");
                    }
                    if (value3 >= 5)
                    {

                        await client.SendTextMessageAsync(msg.Chat.Id, "Нет!");
                    }
                }
                else if (msg.Text.Contains("/send_all"))
                {
                    string message_for_each = msg.Text.Substring(8);
                    string[] each_id = File.ReadAllLines(chats_id);
                    foreach (string string_id in each_id)
                    {
                        long long_id = long.Parse(string_id);
                        await client.SendTextMessageAsync(long_id, message_for_each); 
                    }
                }



            }
        }

        private static async void OnMessageEdit(object sender, MessageEventArgs e)
        {


            var msg = e.Message;
            string string_message_id = msg.MessageId.ToString();
            long msg_id = e.Message.Chat.Id;
            string number_id_chat = msg_id.ToString();
            string directory_chats = Directory.GetCurrentDirectory() + "/chats";
            string directory_file_chat = directory_chats + "/" + (string)number_id_chat + ".txt";

            //string message_for_each = msg.Text.Substring(8);
            string[] array_current_chat = File.ReadAllLines(directory_file_chat);
            for(int i = 0; i < array_current_chat.Length; i++)
            {
                if (array_current_chat[i].Contains(string_message_id))
                {
                    string n = array_current_chat[i];
                    string s = " Текст: ";
                    int p = n.IndexOf(s);
                    n = n.Remove(0, p + s.Length);

                    string[] sybols_array = { "\\_", "\\*", "\\[", "\\]", "\\(", "\\)", "\\~", "\\`", "\\>", "\\#", "\\+", "\\-", "\\=", "\\|", "\\{", "\\}", "\\.", "\\!" };
                    for (int b = 0; b < sybols_array.Length; b++)
                    {
                        n = n.Replace(sybols_array[b], "\\\\" + sybols_array[b]);
                    }
                    Console.WriteLine(n);

                    n += " || " + n + " || ";
                    await client.SendTextMessageAsync(msg.Chat.Id,
                                  "Ты что думаешь, я ничего не видел?\r\n" + n,
                                  parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                                  replyToMessageId: msg.MessageId
                                  );
                }
            }





        }

    }
}

