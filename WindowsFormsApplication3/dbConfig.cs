using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public class dbConfig
    {
        int count, ID;


        String connectionString;

        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteDataReader reader;
        int i;
        string isRel = "";
        List<string> sourceList = new List<string>();
        List<string> filteredList = new List<string>();
        List<string> isReliableList = new List<string>();
        List<int> filteredID = new List<int>();

        public dbConfig()
        {
            connectionString = @"Data Source = D:\RWTH_BIT\Semester-2\Scrum Lab\lab work\WindowsFormsApplication3\WindowsFormsApplication3\VideoFeed_Watcher; ";
        }

        public void displayData()
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    cmd.CommandText = @"select * from video_table where video_id= @ID";
                    cmd.Connection = con;
                    //cmd.Parameters.Add(new SQLiteParameter("@tablename", "video_table"));
                    cmd.Parameters.Add(new SQLiteParameter("@ID", 1));
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("value in table:" + reader[1].ToString() + reader[2].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                reader.Close();
            }
        }

        public void insertData()
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    cmd.CommandText = @"insert into video_table values(@ID, @videoname, @path, @notes)";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SQLiteParameter("@ID", 2));
                    cmd.Parameters.Add(new SQLiteParameter("@videoname", "Nokia News"));
                    cmd.Parameters.Add(new SQLiteParameter("@path", "C:/Users/News"));
                    cmd.Parameters.Add(new SQLiteParameter("@notes", ""));
                    i = cmd.ExecuteNonQuery();

                    if (i != 0)
                    {
                        Console.WriteLine(" ROW inserted");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void deleteData()
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    cmd.CommandText = @"delete from video_table where video_id= @ID";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SQLiteParameter("@ID", 2));

                    i = cmd.ExecuteNonQuery();

                    if (i != 0)
                    {
                        Console.WriteLine("deleted row");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void updateData()
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    cmd.CommandText = @"update video_table set video_name = @videoname where video_id= @ID";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SQLiteParameter("@ID", 1));
                    cmd.Parameters.Add(new SQLiteParameter("@videoname", "New Siemens News"));

                    i = cmd.ExecuteNonQuery();

                    if (i != 0)
                    {
                        Console.WriteLine("updated");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //Source Table data being fetched
        public void displaySourceData()
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    cmd.CommandText = @"select distinct source_id, source_name, is_reliable from source_table";
                    cmd.Connection = con;
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        count++;
                        sourceList.Add((string)reader["source_name"]);
                        isReliableList.Add((string)reader["is_reliable"]);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                reader.Close();
            }
        }


        public int getCountSource()
        {
            return count;
        }

        public List<string> getSourceList()
        {
            return sourceList;
        }

        public List<string> getReliableSourceList()
        {
            return isReliableList;
        }

        //Source Table data updated: isReliable or not
        public void updateSourceTable(String sourceName)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();

                    cmd.CommandText = @"select is_reliable from source_table where source_name = @sourcename";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SQLiteParameter("@sourcename", sourceName));

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        isRel = reader[0].ToString();
                    }

                    reader.Close();

                    cmd.CommandText = @"update source_table set is_reliable = @flagvalue where source_name = @sourcename";

                    if (isRel == "false")
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@flagvalue", "true"));
                        isRel = "true";
                    }
                    else
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@flagvalue", "false"));
                        isRel = "false";
                    }

                    cmd.Connection = con;
                    cmd.Parameters.Add(new SQLiteParameter("@sourcename", sourceName));

                    i = cmd.ExecuteNonQuery();

                    if (i != 0)
                    {
                        Console.WriteLine("updated");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" Test point: " + ex.Message);
                }
            }

        }
        public string getReliableState()
        {
            return isRel;
        }


        public void enableFilter()
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                try
                {
                    string flag = "true";
                    con.Open();
                    cmd.CommandText = @"select video_name, video_id from video_table where source_id in (select source_id from source_table where is_reliable = @value)";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SQLiteParameter("@value", flag));

                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        filteredList.Add((string)reader[0]);
                        if (reader[1] != DBNull.Value)
                            ID = Convert.ToInt32(reader[1]);
                        Console.WriteLine(reader[1]);
                        filteredID.Add(ID);


                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                reader.Close();
            }

        }

        public List<string> getfilteredList()
        {
            return filteredList;
        }
        public List<int> getfilteredId()
        {
            return filteredID;
        }


    }
}