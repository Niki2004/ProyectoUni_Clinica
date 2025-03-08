using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoClinica.Models
{
    public class ShowNewPasswordViewModel
    {
        public string Email { get; set; }
        public string TemporaryPassword { get; set; }
    }
}