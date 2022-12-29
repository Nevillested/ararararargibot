using System;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ararararargibot
{
    internal class queries_to_bd
    {
        //добавление первоначального сообщения в историю
        public static void insert_story(string USER_TEXT, string USER_NAME, int MSG_ID, long CHAT_ID, Exception ERROR)
        {
            string constr = "User Id=john;Password=abcd1234;Data Source=localhost:1521/XEPDB1;";

            OracleConnection con = new OracleConnection(constr);
            con.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;

            if (ERROR == null)
            {
                cmd.CommandText = "Insert into TEST_TABLE(USER_TEXT, USER_NAME, MSG_ID, CHAT_ID) VALUES (:1, :2, :3, :4)";
                cmd.Parameters.Add(new OracleParameter("1", OracleDbType.Varchar2, USER_TEXT, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("2", OracleDbType.Varchar2, USER_NAME, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("3", OracleDbType.Int64, MSG_ID, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("4", OracleDbType.Int64, CHAT_ID, ParameterDirection.Input));

            }
            else
            {
                cmd.CommandText = "Insert into TEST_TABLE(USER_TEXT, USER_NAME, MSG_ID, CHAT_ID, ERROR, DT_UPD) VALUES (:1, :2, :3, :4, :5, :6)";
                cmd.Parameters.Add(new OracleParameter("1", OracleDbType.Varchar2, USER_TEXT, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("2", OracleDbType.Varchar2, USER_NAME, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("3", OracleDbType.Int64, MSG_ID, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("4", OracleDbType.Int64, CHAT_ID, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("5", OracleDbType.Varchar2, ERROR.ToString(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("6", OracleDbType.TimeStamp, DateTime.Now, ParameterDirection.Input));
            }

            if (cmd.ExecuteNonQuery() != 0)
            {
                Console.WriteLine("Записано в бд");
            }
            else
            {
                Console.WriteLine("кол-0во добавленных строк = 0, чот не так с бд");
            }
            con.Dispose();
        }
        
        //добавление измененного сообщения в историю
        public static void update_story(string USER_TEXT, int MSG_ID, long CHAT_ID, Exception ERROR)
        {
            string constr = "User Id=john;Password=abcd1234;Data Source=localhost:1521/XEPDB1;";

            OracleConnection con = new OracleConnection(constr);
            con.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;


            cmd.CommandText = " insert into test_table (user_text, user_name, chat_id, msg_id, dt_ins, dt_upd, msg_ver)" +
                              " select :1 , user_name, chat_id, msg_id, dt_ins, current_timestamp, msg_ver + 1" +
                              " from ( select user_name, chat_id, msg_id, dt_ins, msg_ver" +
                                     " from test_table" +
                                     " where chat_id = :2 and msg_id = :3 order by msg_ver desc )" +
                              " where rownum = 1";

            cmd.Parameters.Add(new OracleParameter("1", OracleDbType.Varchar2, USER_TEXT, ParameterDirection.Input));
            cmd.Parameters.Add(new OracleParameter("2", OracleDbType.Int64, CHAT_ID, ParameterDirection.Input));
            cmd.Parameters.Add(new OracleParameter("3", OracleDbType.Int64, MSG_ID, ParameterDirection.Input));

            if (cmd.ExecuteNonQuery() != 0)
            {
                Console.WriteLine("Записано и обновлено в бд");
            }
            else
            {
                Console.WriteLine("кол-0во обновленных строк = 0, чот не так с бд");
            }
            con.Dispose();
        }

        //выдача первоначального сообщения
        public static string get_first_msg(string USER_TEXT, int MSG_ID, long CHAT_ID)
        {
            string constr = "User Id=john;Password=abcd1234;Data Source=localhost:1521/XEPDB1;";

            OracleConnection con = new OracleConnection(constr);
            con.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;

            cmd.CommandText = " select user_text" +
                              " from  ( select user_text, rownum rn" +
                                        " from ( select user_text" +
                                                 " from test_table" +
                                                " where chat_id = :1 and msg_id = :2 order by msg_ver desc)" +
                                     ") " +
                              " where rn = 2";

            cmd.Parameters.Add(new OracleParameter("1", OracleDbType.Int64, CHAT_ID, ParameterDirection.Input));
            cmd.Parameters.Add(new OracleParameter("2", OracleDbType.Int64, MSG_ID, ParameterDirection.Input));

            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            string result = dr.GetString(0);

            con.Dispose();

            return result;
        }

        //выдача анекдота
        public static string get_joke()
        {
            string constr = "User Id=john;Password=abcd1234;Data Source=localhost:1521/XEPDB1;";

            OracleConnection con = new OracleConnection(constr);
            con.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;

            cmd.CommandText = "select text_joke from (select * from jokes order by dbms_random.value) where rownum = 1";

            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            string result = dr.GetString(0);

            con.Dispose();

            return result;
        }

        //выдача всех пользователей
        public static string get_users()
        {
            string constr = "User Id=john;Password=abcd1234;Data Source=localhost:1521/XEPDB1;";

            OracleConnection con = new OracleConnection(constr);
            con.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;

            cmd.CommandText = "select '@'||(LISTAGG(user_name, '\r\n@') WITHIN GROUP (ORDER BY user_name)) as users_name from (select distinct user_name from test_table)";

            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            string result = dr.GetString(0);

            con.Dispose();

            return result;
        }

        //выдача id чата с пользователем
        public static int get_chat_id(string user_name)
        {
            string constr = "User Id=john;Password=abcd1234;Data Source=localhost:1521/XEPDB1;";

            OracleConnection con = new OracleConnection(constr);
            con.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;

            cmd.CommandText = "select distinct chat_id from test_table where lower(user_name)=trim(lower( :1 ))";

            cmd.Parameters.Add(new OracleParameter("1", OracleDbType.Varchar2, user_name, ParameterDirection.Input));

            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            int result = dr.GetInt32(0);

            con.Dispose();

            return result;
        }
    }
}
