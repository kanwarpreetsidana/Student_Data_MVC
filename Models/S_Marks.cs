using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
    public class S_Marks
    {

        public int id { get; set; }
        public int student_id { get; set;}

        public string first_name { get; set; }

        public string last_name { get; set; }

        [Display(Name = "Select Name:")]

        public string student_name { get; set; }


        [Display(Name = "Select Class:")]
        public string student_class { get; set; }

        [Display(Name = "Subject:")]
        public string subject { get; set; }

        [Display(Name = "Marks:")]
        public decimal marks { get; set; }


    

    }
}