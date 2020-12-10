using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models.Category
{
    public class CategoryDetail
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        [Display(Name = "Number of Notes")]
        public int NotesCount { get; set; }

        [Display(Name = "List of Notes")]
        public List<NoteListItem> Notes { get; set; } = new List<NoteListItem>();

    }
}
