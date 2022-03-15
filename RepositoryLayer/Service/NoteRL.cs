using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;

namespace RepositoryLayer.Service
{
    public class NoteRL : INoteRL
    {
        //instance of  FundooContext Class
        private readonly FundooContext fundooContext;
        private IConfiguration _config;

        //Constructor
        public NoteRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this._config = configuration;

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

        public Note CreateNote(NoteEntity note, long userId)
        {
            throw new NotImplementedException();
        }
    }
}
