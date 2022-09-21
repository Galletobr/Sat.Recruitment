using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    public class Result
    {
        public Result()
        {
            IsSuccess = false;
            Errors = "Ocurrio un error";
        }
        public bool IsSuccess { get; set; }
        public string Errors { get; set; }
    }

    [ApiController]
    [Route("v1/[controller]")]
    public partial class UsersController : ControllerBase
    {
        Result _result = new Result();
        private readonly IUtileria _utileria;
        private readonly List<User> _users = new List<User>();
        public UsersController(IUtileria utileria)
        {
            _utileria = utileria;
        }

        [HttpPost]
        [Route("/create-user")]
        public async Task<Result> CreateUser(User _user)
        {
            await Task.Delay(1);
            var errors = "";

          _utileria.ValidateErrors(_user, ref errors);
            _result.Errors = errors;
            
            if (!string.IsNullOrWhiteSpace(errors))
            {
                return _result;
            }
            User newUser = _user;

            if (newUser.UserType == "Normal")
            {
                if (_user.Money > 100)
                {
                    var percentage = Convert.ToDecimal(0.12);
                    //If new user is normal and has more than USD100
                    var gif = _user.Money * percentage;
                    newUser.Money = newUser.Money + gif;
                }
                if (_user.Money < 100)
                {
                    if (_user.Money > 10)
                    {
                        var percentage = Convert.ToDecimal(0.8);
                        var gif = _user.Money * percentage;
                        newUser.Money += gif;
                    }
                }
            }
            if (newUser.UserType == "SuperUser")
            {
                if (_user.Money > 100)
                {
                    var percentage = Convert.ToDecimal(0.20);
                    var gif = _user.Money * percentage;
                    newUser.Money += gif;
                }
            }
            if (newUser.UserType == "Premium")
            {
                if (_user.Money > 100)
                {
                    var gif = _user.Money * 2;
                    newUser.Money += gif;
                }
            }


            var reader = ReadUsersFromFile();

            //Normalize email
            var aux = newUser.Email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

            newUser.Email = string.Join("@", new string[] { aux[0], aux[1] });

            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLineAsync().Result;
                string[] cadena = line.Split(',');
                _users.Add(new User
                {

                    Name = cadena[0],
                    Email = cadena[1],
                    Phone = cadena[2],
                    Address = cadena[3],
                    UserType = cadena[4],
                    Money = decimal.Parse(cadena[5]),
                });
            }
            reader.Close();
            try
            {
                var isDuplicated = _users.FirstOrDefault(U => U.Email.Equals(newUser.Email) || U.Phone.Equals(newUser.Phone)
                || U.Name.Equals(newUser.Name) || U.Address.Equals(newUser.Address))!=null;
                if (isDuplicated)
                {
                    throw new Exception("User is duplicated");
                }

                if (!isDuplicated)
                {
                    Debug.WriteLine("User Created");
                    _result.IsSuccess = true;
                    _result.Errors = "User Created";
                    return _result;
                }
                else
                {
                    Debug.WriteLine("The user is duplicated");
                    _result.Errors = "The user is duplicated";
                    return _result;
                }
            }
            catch
            {
                Debug.WriteLine("The user is duplicated");
                _result.Errors = "The user is duplicated";
                return _result;
            }

        }

        //Validate errors
        //private void ValidateErrors(User user, ref string errors)
        //{

        //    if (user.Name == null)
        //    {
        //        //Validate if Name is null
        //        errors = "The name is required";
        //    }
        //    if (user.Email == null)
        //    {
        //        //Validate if Email is null
        //        errors += " The email is required";
        //    }
        //    if (user.Address == null)
        //    {
        //        //Validate if Address is null
        //        errors += " The address is required";
        //    }
        //    if (user.Phone == null)
        //    {
        //        //Validate if Phone is null
        //        errors += " The phone is required";
        //    }
        //}
    }
    //public class User
    //{
    //    public string Name { get; set; }
    //    public string Email { get; set; }
    //    public string Address { get; set; }
    //    public string Phone { get; set; }
    //    public string UserType { get; set; }
    //    public decimal Money { get; set; }
    //}
}
