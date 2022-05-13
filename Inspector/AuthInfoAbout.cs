using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspector
{
    class AuthInfoAbout
    {
        private static int _auth = new int();

        public static int Auth { get => _auth; set => _auth = value; }
    }
}
