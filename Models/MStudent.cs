using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication5.Models;

namespace WebApplication5.Models
{


    public class MStudent
    {

       

        public string AddEmployeeRecord(MAdd employeeEntities)
        {
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("insert_tbl_add_new_student", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@first_name", employeeEntities.first_name);
                    cmd.Parameters.AddWithValue("@last_name", employeeEntities.last_name);
                    cmd.Parameters.AddWithValue("@student_class", employeeEntities.student_class);
                  
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return ("Data save Successfully");
                }
                catch (Exception ex)
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    return (ex.Message.ToString());
                }


            }


        }
    }
}