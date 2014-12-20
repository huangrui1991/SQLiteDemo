using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Linq;

namespace SQLiteTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string connString = @"Data Source = C:\hr\code\C#\SQLite\test.db";
            List<string> nameList = new List<string> { "huangrui","sunxiuqiao","cgz","huanxi"};
            try
            {
                using(SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(@"Create table hehe(id int ,name text)",conn);
                    cmd.ExecuteNonQuery();
                    using(SQLiteTransaction transaction = conn.BeginTransaction())
                    {
                        using(SQLiteCommand myCmd = new SQLiteCommand(conn))
                        {
                            myCmd.CommandText = @"insert into hehe (id,name) values(?,?)";
                            SQLiteParameter para_id = new SQLiteParameter();
                            SQLiteParameter para_name = new SQLiteParameter();
                            myCmd.Parameters.Add(para_id);
                            myCmd.Parameters.Add(para_name);
                            int n;
                            
                            for (n = 0; n < nameList.Count;++n)
                            {
                                para_id.Value = n;
                                para_name.Value = nameList[n];
                                myCmd.ExecuteNonQuery();
                            }
                            myCmd.CommandText = "select * from hehe";
                            var reader = myCmd.ExecuteReader();
                            while(reader.Read())
                            {
                                string msg = "";
                                for( int i = 0 ;i < reader.FieldCount; ++i)
                                {
                                    msg += reader.GetValue(i) + " ";
                                }
                                Console.WriteLine(msg);
                            }
                        }
                        transaction.Commit();
                    }
                    
                }
            }
            catch(SQLiteException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }

        }
       
    }
}
