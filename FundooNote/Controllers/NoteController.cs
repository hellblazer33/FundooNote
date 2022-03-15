using BusinessLayer.Interface;
using BusinessLayer.Service;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FundooProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {

        private readonly INoteBL noteBL;
        //Constructor
        public NotesController(INoteBL noteBL)
        {
            this.noteBL = noteBL;
        }

        private long GetTokenId()
        {
            return Convert.ToInt64(User.FindFirst("Id").Value);
        }

        //Create a Note
        [Authorize]
        [HttpPost("Create")]
        public IActionResult CreateNote(Note note)
        {
            try
            {
                long Id = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var result = noteBL.CreateNote(note, Id);
                if (result != null)
                    return this.Ok(new { Success = true, message = "Notes created successful", data = result });
                else
                    return this.BadRequest(new { Success = false, message = "Notes not created " });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { success = false, Message = e.InnerException });
            }
        }



    }
}
