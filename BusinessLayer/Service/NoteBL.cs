using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class NoteBL: INoteBL
    {
        private readonly INoteRL noteRL;
        //Constructor
        public NoteBL(INoteRL noteRL)
        {
            this.noteRL = noteRL;
        }

        public NoteEntity CreateNote(Note note, long Id)
        {
            try
            {
                return noteRL.CreateNote(note, Id);
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
                return noteRL.UpdateNotes(updateNotes, notesId);

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

                return noteRL.DeleteNotes(id, notesId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<NoteEntity> RetrieveAllNotes(long notesId)
        {
            try
            {

                return noteRL.RetrieveAllNotes(notesId);
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
                return noteRL.IsPinned(notesId, Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsTrash(long notesId, long Id)
        {
            try
            {
                return noteRL.IsTrash(notesId, Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsArchive(long notesId, long Id)
        {
            try
            {
                return noteRL.IsArchive(notesId, Id);
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
                return this.noteRL.ColorNotes(userId, notesId, color);
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
                return this.noteRL.UploadImage(noteId, userId, image);
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
                return noteRL.GetTrash(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NoteEntity> GetArchived(long Id)
        {
            try
            {
                return noteRL.GetArchived(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}