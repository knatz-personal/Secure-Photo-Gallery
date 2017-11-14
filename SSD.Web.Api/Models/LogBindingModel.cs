using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSD.Web.Api.Models
{
    public class LogBindingModel
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
}