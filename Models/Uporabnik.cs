using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blitz_chat.Models
{
    public class Uporabnik
    {
        public string id { get; set; }
        public string Email { get; set; }
        public string Geslo { get; set; }   
        public string Profilna { get; set; }
        public string Status { get; set; }

    }
}
