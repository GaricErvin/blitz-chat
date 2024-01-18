using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blitz_chat.Models
{
    public class Prijateljstva
    {
        public string Id { get; set; }
        public string uid1 { get; set; }
        public string uid2 { get; set; }
        public bool IsFriendConfirmed { get; set; }
    }
}
