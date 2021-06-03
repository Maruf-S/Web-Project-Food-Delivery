using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Delivery.Helpers
{
    public static class Role
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
        public const string Employee = "Employee";
        public static string[] GetAllRolesForSeed()
        {

            return new string[]
            {
             Admin,Customer,Employee
            };
        }
    }
}
