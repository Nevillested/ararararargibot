using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace ararararargibot
{
    internal class cezar
    {
        /*эта страшная шутка внизу - Tuple, это метод, возвращающий два значения. В данном случае 2 переменные типов OracleCommand и OracleConnection соответственно.
        конкретно этот метод открывает новое подключение к oracle db*/
        public static Tuple<OracleCommand, OracleConnection> new_conection()
        {
            string constr = "User Id=john;Password=abcd1234;Data Source=localhost:1521/XEPDB1;";
            OracleConnection con = new OracleConnection(constr);
            con.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;
            return Tuple.Create(cmd, con);
        }

        //Сохраняем прилетевшее сообщение для шифровки / дешифровки
        public static void save_user_msg(long chat_id, string user_msg)
        {
            var new_con = new_conection();
            new_con.Item1.CommandText = "Insert into cezar(chat_id, msg) VALUES (:1, :2)";
            new_con.Item1.Parameters.Add(new OracleParameter("1", OracleDbType.Int32, chat_id, ParameterDirection.Input));
            new_con.Item1.Parameters.Add(new OracleParameter("2", OracleDbType.Varchar2, user_msg, ParameterDirection.Input));
            new_con.Item1.ExecuteNonQuery();
            new_con.Item2.Dispose();
        }

        //Сохраняем ключ для шифровки / дешифровки
        public static void save_key(long chat_id, int user_key)
        {
            var new_con = new_conection();
            new_con.Item1.CommandText = "merge into cezar a using (select id from(select * from cezar where chat_id = :1 order by id desc) where rownum = 1 ) b on(a.id = b.id) when matched then update set a.key = :2 ";
            new_con.Item1.Parameters.Add(new OracleParameter("1", OracleDbType.Int32, chat_id, ParameterDirection.Input));
            new_con.Item1.Parameters.Add(new OracleParameter("2", OracleDbType.Int32, user_key, ParameterDirection.Input));
            new_con.Item1.ExecuteNonQuery();
            new_con.Item2.Dispose();
        }

        //шифруем сообщение
        public static string encrypt (long msg_chat_id)
        {
            var new_con = new_conection();

            new_con.Item1.CommandText = "select msg, key from (select * from cezar where chat_id = :1 order by id desc) where rownum = 1";
            new_con.Item1.Parameters.Add(new OracleParameter("1", OracleDbType.Int32, msg_chat_id, ParameterDirection.Input));

            OracleDataReader dr = new_con.Item1.ExecuteReader();
            dr.Read();
            string msg_db = dr.GetString(0);
            int key_db = dr.GetInt32(1);

            new_con.Item2.Dispose();

            //шифруем сообщение
            const string alfabet = "ЮЬНЕЛХЙМЭДЁШОУКЖСИЗГРЫЪЩВПЧЦФАЯТБ";

                //добавляем в алфавит маленькие буквы
                var fullAlfabet = alfabet.ToLower() + alfabet.ToLower();
                var letterQty = fullAlfabet.Length;
                var retVal = "";
                for (int i = 0; i < msg_db.Length; i++)
                {
                    var c = msg_db.ToLower()[i];
                    var index = fullAlfabet.IndexOf(c);
                    if (index < 0)
                    {
                        //если символ не найден, то добавляем его в неизменном виде
                        retVal += c.ToString().ToLower();
                    }
                    else
                    {
                        var codeIndex = (letterQty + index + key_db) % letterQty;
                        retVal += fullAlfabet.ToLower()[codeIndex];
                    }
                }

                return retVal;

        }

        //дешифруем сообщение
        public static string decrypt(long msg_chat_id)
        {
            var new_con = new_conection();

            new_con.Item1.CommandText = "select msg, key from (select * from cezar where chat_id = :1 order by id desc) where rownum = 1";
            new_con.Item1.Parameters.Add(new OracleParameter("1", OracleDbType.Int32, msg_chat_id, ParameterDirection.Input));

            OracleDataReader dr = new_con.Item1.ExecuteReader();
            dr.Read();
            string msg_db = dr.GetString(0);
            int key_db = dr.GetInt32(1);

            new_con.Item2.Dispose();
            //дешифруем сообщение
            const string alfabet = "ЮЬНЕЛХЙМЭДЁШОУКЖСИЗГРЫЪЩВПЧЦФАЯТБ";

            //добавляем в алфавит маленькие буквы
            var fullAlfabet = alfabet.ToLower() + alfabet.ToLower();
            var letterQty = fullAlfabet.Length;
            var retVal = "";
            for (int i = 0; i < msg_db.Length; i++)
            {
                var c = msg_db.ToLower()[i];
                var index = fullAlfabet.IndexOf(c);
                if (index < 0)
                {
                    //если символ не найден, то добавляем его в неизменном виде
                    retVal += c.ToString().ToLower();
                }
                else
                {
                    var codeIndex = (letterQty + index - key_db) % letterQty;
                    retVal += fullAlfabet.ToLower()[codeIndex];
                }
            }
            return retVal;
        }

    }
}
