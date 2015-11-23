using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCR.Root.App_Code
{
    public class Enumerator
    {
        public enum MessageType
        {
            information = 1,
            warning = 2,
            question = 3,
            error = 4,
            confirmation = 5,
            defaultmsg = 6,
        }
    }
}