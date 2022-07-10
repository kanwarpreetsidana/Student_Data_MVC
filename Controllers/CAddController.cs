using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebApplication5.Models;
using System.Linq;

namespace WebApplication5.Controllers
{
    public class CAddController : Controller
    {
      
        public ActionResult Index()
        {
           
           

            return View();
        }



        [HttpPost]
        public JsonResult StudentData()
        {

            List<MAdd> Student_List = new List<MAdd>();
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SELECT id,first_name,last_name,student_class FROM tbl_add_new_student", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var stud = new MAdd();

                    stud.id = Convert.ToInt32(rdr["id"]);
                    stud.first_name = (rdr["first_name"].ToString());
                    stud.last_name = rdr["last_name"].ToString();
                    stud.student_class = rdr["student_class"].ToString();
                    Student_List.Add(stud);
                }
            }

            return Json(Student_List);

        }


        [HttpPost]
        public ActionResult AddStudent(MAdd Stu)
        {
            try
            {

                if (Stu.first_name != null && Stu.last_name != null && Stu.student_class != null)
                {


                    if (AddStudents(Stu))
                    {
                       
                       

                    }
                    ModelState.Clear();
                }

                else 
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");

            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

      

        public bool AddStudents(MAdd obj)
        {

            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
               

                SqlCommand com = new SqlCommand("insert_tbl_add_new_student", con);
                com.CommandType = CommandType.StoredProcedure;

                if (obj.id != 0)
                {

                    com.Parameters.AddWithValue("@id", obj.id);
                }

                com.Parameters.AddWithValue("@first_name", obj.first_name);
                com.Parameters.AddWithValue("@last_name", obj.last_name);
                com.Parameters.AddWithValue("@student_class", obj.student_class);
                con.Open();
                int i = com.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {
                    if (obj.id != 0)
                    {
                        TempData["alert_message"] = "Updated Successfully";

                    }
                    else 
                    {
                        TempData["alert_message"] = "Added Successfully";
                       
                    }

                    return true;

                }
                else
                {

                    return false;
                }

            }
        }

        [HttpPost]
        
        public ActionResult DeleteCustomer(int id)
        {
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand com = new SqlCommand("delete_tbl_add_new_student", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@id", id);
              
                con.Open();
                int i = com.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {

                    return RedirectToAction("Index");
                }
                else
                {

                    return RedirectToAction("Index");
                }

            }
         
        }
        
        [HttpPost]
        public JsonResult EditData(int id)
        {

            List<MAdd> StudentList = new List<MAdd>();
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SELECT id,first_name,last_name,student_class FROM tbl_add_new_student where id = @id", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var student = new MAdd();

                    student.id = Convert.ToInt32(rdr["id"]);
                    student.first_name = (rdr["first_name"].ToString());
                    student.last_name = rdr["last_name"].ToString();
                    student.student_class = rdr["student_class"].ToString();
                    StudentList.Add(student);

                    

                }
            }

            return Json(StudentList);
        }







    }







}