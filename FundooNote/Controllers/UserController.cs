using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }

}
