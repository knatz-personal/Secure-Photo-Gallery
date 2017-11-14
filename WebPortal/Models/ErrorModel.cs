using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPortal.Models
{
    public class ErrorLogModel
    {
        [Required]
        public DateTime? DateAndTime { get; set; }

        [Required]
        public string ExceptionAndTrace { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string Username { get; set; }
    }

    public class ErrorModel
    {
        public string Code { get; internal set; }
    }
}