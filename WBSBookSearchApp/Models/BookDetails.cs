using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WBSBookSearchApp.Models
{
    public class BookDetails
    {
        [Key]
        public int BookId { get; set; }
        public string BookLink { get; set; }
        public string Name { get; set; }
        public string Abstract { get; set; }
        public int NumberOfPages { get; set; }
        public string Comment { get; set; }

        public int AuthorId { get; set; }
        public virtual BookAuthorDetails AuthorDetails { get; set; }
    }
}