using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;

namespace RepositoryLayer.Interface
{
    public interface INoteRL
    {
        public NoteEntity CreateNote(Note note, long Id);
        public NoteEntity UpdateNotes(UpdateNotes updateNotes, long notesId);
        bool DeleteNotes(long id, long notesId);
        IEnumerable<NoteEntity> RetrieveAllNotes(long Id);

        public bool IsPinned(long notesId, long Id);
        public bool IsTrash(long notesId, long Id);
        public bool IsArchive(long notesId, long Id);
        public NoteEntity ColorNotes(long userId, long notesId, string color);

        public NoteEntity UploadImage(long noteId, long userId, IFormFile image);
        public List<NoteEntity> GetTrash(long Id);
        public List<NoteEntity> GetArchived(long Id);

    }
}