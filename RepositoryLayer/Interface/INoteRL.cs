using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;

namespace RepositoryLayer.Interface
{
    public interface INoteRL
    {
        public NoteEntity CreateNote(Note note, long userId);
      
    }
}