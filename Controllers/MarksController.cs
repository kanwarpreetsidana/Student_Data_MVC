using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication5.Controllers
{


    public class MarksController : Controller
    {

        string connection = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        List<SelectListItem> class_list = new List<SelectListItem>();
        List<SelectListItem> team = new List<SelectListItem>();
        

        public ActionResult Index()
        {
         
            ViewBag.var1 = GetOptions();
            ViewBag.var2 = team;
           
            return View();
        }

        private SelectList GetOptions()
        {

            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    SqlDataReader myReader = null;
                    SqlCommand myCommand = new SqlCommand("select distinct student_class,student_class  from tbl_add_new_student tb order by tb.student_class   asc", conn);

                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {

                        class_list.Add(new SelectListItem { Text = myReader["student_class"].ToString(), Value = myReader["student_class"].ToString() });
                    }


                }
                catch (Exception e)
                {
                    string msg = e.Message;
                }
                finally
                {

                    conn.Close();
                }

                return new SelectList(class_list, "Value", "Text", "id");

            }
        }

        public JsonResult Team(string name)
        {

            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand("select first_name+ ' '+ last_name as name_student,id  from tbl_add_new_student where student_class ='" + name + "' ", conn);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {

                    team.Add(new SelectListItem { Text = myReader["name_student"].ToString(), Value = myReader["id"].ToString() });
                }
            }
            return Json(team, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public JsonResult StudentData()
        {

            List<S_Marks> stdList = new List<S_Marks>();
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("select sm.id,studentid,subject,marks,first_name,last_name,student_class from tbl_add_student_marks sm inner join tbl_add_new_student ns on  sm.studentid = ns.id order by studentid asc", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var st = new S_Marks();
                    st.id = Convert.ToInt32(rdr["id"]);
                    st.student_id = Convert.ToInt32(rdr["studentid"]);
                    st.first_name = rdr["first_name"].ToString();
                    st.last_name = rdr["last_name"].ToString();
                    st.student_class = rdr["student_class"].ToString();
                    st.subject = rdr["subject"].ToString();
                    st.marks = Convert.ToDecimal (rdr["marks"]);


                    stdList.Add(st);
                }
            }

            return Json(stdList);

        }

        [HttpPost]
        public ActionResult AddMrks(S_Marks Emp)
        {
            try
            {
                    if (AddMrkss(Emp))
                    {
                        ViewBag.Message = "Details added successfully";
                    }

                    ModelState.Clear();

                    return RedirectToAction("Index");
               
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public bool AddMrkss(S_Marks obj)
        {

            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand com = new SqlCommand("insert_tbl_add_student_marks", con);
                com.CommandType = CommandType.StoredProcedure;

                if (obj.id != 0)
                {
                    com.Parameters.AddWithValue("@id", obj.id);
                    com.Parameters.AddWithValue("@studentid", obj.student_id);
                }
                else 
                {
                    com.Parameters.AddWithValue("@studentid", Request.Form["var2"]);
                }

                com.Parameters.AddWithValue("@subject", Request.Form["subject_drop"]);
                com.Parameters.AddWithValue("@marks", obj.marks);
                con.Open();
                int i = com.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {
                    return true;
                }
                else
                {

                    return false;
                }
            }
        }

        [HttpPost]

        public ActionResult DeleteMarks(int id)
        {
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand com = new SqlCommand("delete_tbl_add_student_marks", con);
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
        public JsonResult EditData(int id , string subject)
        {

            List<S_Marks> StudntList = new List<S_Marks>();
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("get_details_for_update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@studentid", id);
                cmd.Parameters.AddWithValue("@subject", subject);
                
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var stnd = new S_Marks();

                    stnd.id = Convert.ToInt32(rdr["id"]);
                    stnd.student_name = (rdr["student_name"].ToString());
                    stnd.student_class = rdr["student_class"].ToString();
                    stnd.subject = rdr["subject"].ToString();
                    stnd.marks =  Convert.ToDecimal(rdr["marks"]);
                    stnd.student_id = Convert.ToInt32(rdr["studentid"]);
                    StudntList.Add(stnd);

                    Team(stnd.student_class);

                }

            }

            return Json(StudntList);
        }




        





    }
}