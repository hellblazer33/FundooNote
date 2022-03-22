using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly FundooContext fundooContext;
        private readonly IConfiguration _Toolsettings;

        public UserRL(FundooContext fundooContext, IConfiguration _Toolsettings)
        {
            this.fundooContext = fundooContext;
            this._Toolsettings = _Toolsettings;

        }

        public UserEntity Registration(UserRegistration User)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = User.FirstName;
                userEntity.LastName = User.LastName;
                userEntity.Email = User.Email;
                userEntity.Password = this.EncryptPassword(User.Password);
                fundooContext.Add(userEntity);
                int result = fundooContext.SaveChanges();
                if (result > 0)
                    return userEntity;
                else
                    return null;

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method for EncryptPassword
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string EncryptPassword(string password)
        {
            try
            {
                byte[] encode = new byte[password.Length];
                encode = Encoding.UTF8.GetBytes(password);
                string encryptPass = Convert.ToBase64String(encode);
                return encryptPass;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method for DecryptPassword
        /// </summary>
        /// <param name="encryptpwd"></param>
        /// <returns></returns>
        public string DecryptPassword(string encryptpwd)
        {
            try
            {
                UTF8Encoding encoder = new UTF8Encoding();
                Decoder utf8Decode = encoder.GetDecoder();
                byte[] toDecodeByte = Convert.FromBase64String(encryptpwd);
                int charCount = utf8Decode.GetCharCount(toDecodeByte, 0, toDecodeByte.Length);
                char[] decodedChar = new char[charCount];
                utf8Decode.GetChars(toDecodeByte, 0, toDecodeByte.Length, decodedChar, 0);
                string PassDecrypt = new string(decodedChar);
                return PassDecrypt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string login(UserLogin userLogin)
        {
            try
            {
                // if Email and password is empty return null. 
                if (string.IsNullOrEmpty(userLogin.Email) || string.IsNullOrEmpty(userLogin.Password))
                {
                    return null;
                }
                var result = fundooContext.User.Where(x => x.Email == userLogin.Email).FirstOrDefault();
                string dcryptPass = this.DecryptPassword(result.Password);
                if (result != null && dcryptPass == userLogin.Password)
                {
                    string token = GenerateSecurityToken(result.Email, result.Id);
                    return token;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GenerateSecurityToken(string Email, long Id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Toolsettings["Jwt:secretkey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(ClaimTypes.Email,Email),
                new Claim("Id",Id.ToString())
            };
            var token = new JwtSecurityToken(_Toolsettings["Jwt:Issuer"],
              _Toolsettings["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(60),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public string ForgetPassword(string email)
        {
            try
            {
                var user = fundooContext.User.Where(x => x.Email == email).FirstOrDefault();
                if (user != null)
                {
                    var token = GenerateSecurityToken(user.Email, user.Id);
                    new Msmq().Sender(token);
                    return token;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ResetPassword(string email, string password, string confirmpassword)
        {
            try
            {
                if (password.Equals(confirmpassword))
                {
                    var user = fundooContext.User.Where(x => x.Email == email).FirstOrDefault();
                    user.Password = confirmpassword;
                    fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        //code
    }
}