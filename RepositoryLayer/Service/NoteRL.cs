using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryLayer.Service
{
    public class NoteRL : INoteRL
    {
        //instance of  FundooContext Class
        public readonly FundooContext fundooContext;
        private readonly IConfiguration configuration;
        public NoteRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
        }

        public NoteEntity CreateNote(Note note, long Id)
        {
            try
            {
                NoteEntity newNotes = new NoteEntity();
                newNotes.Title = note.Title;
                newNotes.Description = note.Description;
                newNotes.Reminder = note.Reminder;
                newNotes.Color = note.Color;
                newNotes.Image = note.Image;
                newNotes.IsArchive = note.IsArchive;
                newNotes.IsTrash = note.IsTrash;
                newNotes.IsPinned = note.IsPinned;
                newNotes.CreatedAt = note.CreatedAt;
                newNotes.ModifierAt = note.ModifierAt;
                newNotes.Id = Id;
                this.fundooContext.Notes.Add(newNotes);
                int result = this.fundooContext.SaveChanges();
                if (result > 0)
                    return newNotes;
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NoteEntity UpdateNotes(UpdateNotes updateNotes, long notesId)
        {
            try
            {
                var note = fundooContext.Notes.Where(update => update.NotesId == notesId).FirstOrDefault();
                if (note != null)
                {
                    note.Title = updateNotes.Title;
                    note.Description = updateNotes.Description;
                    note.Color = updateNotes.Color;
                    note.Image = updateNotes.Image;
                    note.ModifierAt = updateNotes.ModifierAt;
                    note.Id = notesId;
                    fundooContext.Notes.Update(note);
                    int result = fundooContext.SaveChanges();
                    return note;
                }

                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteNotes(long id, long notesId)
        {
            try
            {
                var result = fundooContext.Notes.Where(e => e.Id == id && e.NotesId == notesId).FirstOrDefault();

                if (result != null)
                {
                    fundooContext.Notes.Remove(result);
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
        // methods for retrieve notes details by note id 
        public IEnumerable<NoteEntity> RetrieveAllNotes(long Id)
        {
            try
            {
                var result = fundooContext.Notes.Where(e => e.Id == Id).ToList();
                if (result != null)
                {

                    return result;
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
        public bool IsPinned(long notesId, long Id)
        {
            try
            {
                var result = fundooContext.Notes.FirstOrDefault(e => e.NotesId == notesId && e.Id == Id);

                if (result != null)
                {
                    if (result.IsPinned == true)
                    {
                        result.IsPinned = false;
                    }
                    else if (result.IsPinned == false)
                    {
                        result.IsPinned = true;
                    }
                    result.ModifierAt = DateTime.Now;
                }
                int changes = fundooContext.SaveChanges();

                if (changes > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Method to IsTrash Details.
        public bool IsTrash(long notesId, long Id)
        {
            try
            {
                var result = fundooContext.Notes.FirstOrDefault(e => e.NotesId == notesId && e.Id == Id);

                if (result != null)
                {
                    result.IsTrash = true;
                    result.IsArchive = false;

                    result.ModifierAt = DateTime.Now;
                }
                int changes = fundooContext.SaveChanges();

                if (changes > 0) { return true; }

                else { return false; }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Method to IsArchive Details.
        public bool IsArchive(long notesId, long Id)
        {
            try
            {
                var result = fundooContext.Notes.FirstOrDefault(e => e.NotesId == notesId && e.Id == Id);

                if (result != null)
                {
                    result.IsArchive = true;
                    result.IsTrash = false;

                    result.ModifierAt = DateTime.Now;
                }
                int changes = fundooContext.SaveChanges();

                if (changes > 0) return true;

                else return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<NoteEntity> GetTrash(long Id)
        {
            try
            {
                var result = fundooContext.Notes.Where(e => e.Id == Id && e.IsTrash == true).ToList();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Method to GetArchived Details.
        public List<NoteEntity> GetArchived(long Id)
        {
            try
            {
                var result = fundooContext.Notes.Where(e => e.Id == Id && e.IsArchive == true).ToList();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NoteEntity ColorNotes(long userId, long notesId, string color)
        {
            try
            {
                NoteEntity note = this.fundooContext.Notes.FirstOrDefault(x => x.Id == userId && x.NotesId == notesId);
                if (note != null)
                {
                    note.Color = color;
                    fundooContext.Notes.Update(note);
                    this.fundooContext.SaveChanges();
                    return note;
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
        public NoteEntity UploadImage(long noteId, long userId, IFormFile image)
        {
            try
            {
                // Fetch All the details with the given noteId and userId
                var note = this.fundooContext.Notes.FirstOrDefault(n => n.NotesId == noteId && n.Id == userId);
                if (note != null)
                {
                    Account acc = new Account(configuration["Cloudinary:CloudName"], configuration["Cloudinary:ApiKey"], configuration["Cloudinary:ApiSecret"]);
                    Cloudinary cloud = new Cloudinary(acc);
                    var imagePath = image.OpenReadStream();
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(image.FileName, imagePath),
                    };
                    var uploadResult = cloud.Upload(uploadParams);
                    note.Image = image.FileName;
                    this.fundooContext.Notes.Update(note);
                    int upload = this.fundooContext.SaveChanges();
                    if (upload > 0)
                    {
                        return note;
                    }
                    else
                    {
                        return null;
                    }
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

    }

    
}
