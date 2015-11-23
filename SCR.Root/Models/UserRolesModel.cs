using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TkmDataAccess;

namespace SCR.Root.Models
{
    public class UserRolesModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}