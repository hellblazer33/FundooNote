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

      
    }
}