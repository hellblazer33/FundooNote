using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;
        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }
        [HttpPost("Register")]
        public IActionResult Registration(UserRegistration user)
        {
            try
            {
                var result = userBL.Registration(user);
                if (result != null)
                    return this.Ok(new { success = true, message = "Registration Successful", data = result });
                else
                    return this.BadRequest(new { success = false, message = "Registration UnSuccessful" });
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost("login")]

        public IActionResult login(UserLogin userLogin)
        {
            try
            {
                var result = userBL.login(userLogin);

                if (result != null)
                    return this.Ok(new { success = true, message = "Login Successful", data = result });
                else
                    return this.BadRequest(new { success = false, message = "Login UnSuccessful" });

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost("forgotPassword")]
        public IActionResult ForgetPassword(string email)
        {
            try
            {
                var user = this.userBL.ForgetPassword(email);
                if (user != null)
                {
                    return this.Ok(new { Success = true, message = "mail sent is successful" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Enter Valid Email" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(string password, string confirmpassword)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var user = userBL.ResetPassword(email, password, confirmpassword);
                if (!user)
                {
                    return this.BadRequest(new { success = false, message = "enter valid password" });

                }



                else
                {
                    return this.Ok(new { success = true, message = "reset password is successful" });
                }




            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }

}
