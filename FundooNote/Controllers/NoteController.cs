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
        [Authorize]
        [HttpPost("Update")]
        public IActionResult UpdateNotes(UpdateNotes updateNotes, long notesId)
        {
            try
            {
                var result = noteBL.UpdateNotes(updateNotes, notesId);
                if (result != null)
                    return this.Ok(new { Success = true, message = "Notes update successful", data = result });
                else
                    return this.BadRequest(new { Success = false, message = "Notes update failed " });
            }
            catch (Exception e)
            {
                throw;
            }
        }
        [Authorize]
        [HttpDelete("Delete")]
        public IActionResult DeleteNotes(long id, long noteId)
        {
            try
            {
                if (noteBL.DeleteNotes(id, noteId))
                    return this.Ok(new { Success = true, message = "Deleted successful", data = noteBL.DeleteNotes(id, noteId) });
                else
                    return this.BadRequest(new { Success = false, message = "Notes not deleted " });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { success = false, Message = e.Message });
            }
        }

        [Authorize]
        [HttpGet("GetNotes")]
        public IActionResult RetrieveAllNotes(long id)
        {
            try
            {
                var result = noteBL.RetrieveAllNotes(id);
                if (result != null)
                    return this.Ok(new { Success = true, message = "Retrieve successful", data = noteBL.RetrieveAllNotes(id) });
                else
                    return this.BadRequest(new { Success = false, message = "Failed! " });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { success = false, Message = e.Message });
            }
        }
        //IsArchieve
        [Authorize]
        [HttpPut("IsArchive")]
        public IActionResult isArchive(long notesId)
        {
            try
            {
                // Take id of  Logged User
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var result = this.noteBL.IsArchive(notesId, userId);
                if (result == true)
                {
                    return this.Ok(new { Success = true, message = "  Archive Successfull ", data = result });
                    
                }
                else
                {
                    
                    return this.BadRequest(new { Success = false, message = " Archive Unsuccessful" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //IsTrashed 
        [Authorize]
        [HttpPut("IsTrash")]
        public IActionResult IsTrash(long notesId)
        {
            try
            {
                // Take id of  Logged User
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var result = this.noteBL.IsTrash(notesId, userId);
                if (result != null)
                {
                    
                    return this.Ok(new { Success = true, message = "  Trash Successfull ", data = result });
                    
                }
                else
                {
                    
                    return this.BadRequest(new { Success = false, message = " Trash Unsuccessful" });
                    
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //IsPinned
        [Authorize]
        [HttpPut("IsPin")]
        public IActionResult IsPinned(long notesId)
        {
            long Id = GetTokenId();
            bool result = noteBL.IsPinned(notesId, Id);

            try
            {
                if (result != null)
                {
                    return Ok(new { Success = true, message = "Successful" });
                }
                else
                {
                    return BadRequest(new { Success = false, message = "Unsuccessful" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPut("Color")]
        public IActionResult ColorNotes(long notesId, String color)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var result = (noteBL.ColorNotes(userId, notesId, color));
                if (result != null)
                    return this.Ok(new { Success = true, message = "Notes color changed successfully", data = result });
                else
                    return this.BadRequest(new { Success = false, message = "Failed to change the color of the note" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPost("ImageUpload")]
        public IActionResult UploadImage(long noteId, IFormFile image)
        {
            try
            {
                // Take id of  Logged In User
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var result = this.noteBL.UploadImage(noteId, userId, image);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Image Uploaded Successfully", data = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Image Upload Failed ! Try Again " });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }



}

