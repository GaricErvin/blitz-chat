using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UporabniskiVmesnik.Models;

namespace blitz_chat.Models
{
    public class FirebaseContext
    {
        public FirebaseClient firebase;
        public static string firebaseUrl = "https://blitzchat-4a405-default-rtdb.europe-west1.firebasedatabase.app/";


        public void InitializeFirebase()
        {
            firebase = new FirebaseClient(firebaseUrl);
        }
    }
}
