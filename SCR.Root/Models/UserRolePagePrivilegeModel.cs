using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCR.Root.Models
{
    public class UserRolePagePrivilegeModel
    {
        public int IntPageId { get; set; }
      
        public int IntUserId { get; set; }
        public int IntParentPageId { get; set; }
        public int IntOrder { get; set; }
        public bool BoolContainsChild { get; set; }
       
        public string StrUIName { get; set; }
        public string StrPageName { get; set; }
        public string StrController { get; set; }
        public string StrAction { get; set; }
        public string StrStyleClass { get; set; }
        public bool boolUserAssign { get; set; }
        public string StrContrain { get; set; }
        public string strhdnValues { get; set; }
    }
}