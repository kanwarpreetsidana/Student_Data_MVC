using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class MAdd
    {
        public int id { get; set; }

        [Display(Name = "First Name:")]
        [Required(ErrorMessage = "Name Required")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string first_name { get; set; }


        [Required]
        [Display(Name = "Last Name:")]
        public string last_name { get; set; }

        [Required]

        [Display(Name = "Class:")]
        public string student_class { get; set; }

    }


       
    }
