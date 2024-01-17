using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UporabniskiVmesnik.ViewModels
{
    public class Uporabnik
    {
        public int id { get; set; }
        public string Email { get; set; }
        public string Geslo { get; set; }   
        public string Username { get; set; }

    }
}
