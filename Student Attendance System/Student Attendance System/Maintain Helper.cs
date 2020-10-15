﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Attendance_System
{
    public class Maintain_Helper
    {
        public static string connstr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\AttendanceDB.mdf;Integrated Security=True";
        SqlConnection conn = null;

        public string ConnectionString   
        {
            get { return connstr; }
            set { connstr = value; } 
        }

        private void Connect()
        {
            if(conn == null)
                conn = new SqlConnection(ConnectionString);
        }

        public bool CheckEmail(string email)
        {
            try
            {
                var eaddr = new System.Net.Mail.MailAddress(email);

                return eaddr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckID(string id)
        {
            int i;

            return int.TryParse(id,out i);
        }

        public bool Insert(char type, string id, string fname, string lname, string email, string password)
        {
            int result = 0;

            Connect();

            string table = "", fields = "";

            if (type == 'S')
            {
                table = "STUDENTS ";
                fields = "(Student_ID, Name, Last_Name, Email, Password)";
            }
            else if (type == 'L')
            {
                table = "LECTURERS ";
                fields = "(Lecturer_ID, Name, Last_Name, Email, Password)";
            }
            else
                table = null;

            if (table != null)
            {
                string sql = "INSERT INTO " + table + fields + " VALUES (@id, @fname, @lname, @email, @password)" ;

                SqlCommand comm = new SqlCommand(sql, conn);
                comm.Parameters.AddWithValue("@id", id);
                comm.Parameters.AddWithValue("@fname", fname);
                comm.Parameters.AddWithValue("@lname", lname);
                comm.Parameters.AddWithValue("@email", email);
                comm.Parameters.AddWithValue("@password", password);

                conn.Open();
                result = comm.ExecuteNonQuery();
                conn.Close();
            }

            if (result < 0)
                return false;
            else
                return true;

        }

        public void Insert(string modID, string modDes)
        {

            string sql = "INSERT INTO MODULES (Module_ID, Description) VALUES (@modID, @modDes)";

            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@modID", modID);
            comm.Parameters.AddWithValue("@modDes", modDes);

            Connect();
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
        }

        public void Update(char type, string id, string fname, string lname, string email, string password)
        {
            string table = "", field = "";

            if (type == 'S')
            {
                table = "STUDENTS ";
                field = "Student_ID";
            }
            else if (type == 'L')
            {
                table = "LECTURERS ";
                field = "Lecturer_ID";
            }
            else
                table = null;

            if (table != null)
            {
                string pw = "";

                if (password != "")
                    pw = ", Password = '@password'";

                string sql = "UPDATE " + table + " SET Name = '@fname', Last_Name = '@lname', Email = '@email'" + pw + " WHERE " + field + " = '@id'";

                Connect();

                SqlCommand comm = new SqlCommand(sql, conn);
                comm.Parameters.AddWithValue("@id", id);
                comm.Parameters.AddWithValue("@fname", fname);
                comm.Parameters.AddWithValue("@lname", lname);
                comm.Parameters.AddWithValue("@email", email);

                if(password != "")
                    comm.Parameters.AddWithValue("@password", password);

                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void Update(string modID, string modDes)
        {
            string sql = "UPDATE MODULES SET Description = '@modDes' WHERE Module_ID = '@id'";

            Connect();

            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@modDes", modDes);

            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
        }

        public void Delete(char type, string id)
        {
            string table = "", field = "";

            if (type == 'S')
            {
                table = "STUDENTS ";
                field = "Student_ID";
            }
            else if (type == 'L')
            {
                table = "LECTURER ";
                field = "Lecturer_ID";
            }
            else
                table = null;

            if (table != null)
            {
                string sql = "DELETE FROM " + table +  "WHERE " + field + " = '@id'";

                Connect();

                SqlCommand comm = new SqlCommand(sql, conn);
                comm.Parameters.AddWithValue("@id", id);
                
                conn.Open();
                comm.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void Delete(string id)
        {
            string sql = "DELETE FROM MODULES WHERE Module_ID = '@id'";

            Connect();

            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@id", id);

            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
        }
    }
}
