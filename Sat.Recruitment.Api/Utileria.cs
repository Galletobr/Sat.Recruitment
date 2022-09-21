using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Sat.Recruitment.Api
{
    public class Utileria : IUtileria
    {
        public void ValidateErrors(User user, ref string errors)
        {

            if (user.Name == null)
            {
                //Validate if Name is null
                errors = "The name is required";
            }
            if (user.Email == null)
            {
                //Validate if Email is null
                errors += " The email is required";
            }
            if (user.Address == null)
            {
                //Validate if Address is null
                errors += " The address is required";
            }
            if (user.Phone == null)
            {
                //Validate if Phone is null
                errors += " The phone is required";
            }
        }
    }
}
