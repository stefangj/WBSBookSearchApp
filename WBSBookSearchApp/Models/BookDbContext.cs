using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WBSBookSearchApp.Models
{
    public class BookDbContext : DbContext
    {
        public BookDbContext() : base("BookDbContext") { }
        public DbSet<BookUserSearch> BookNameTable { get; set; }
        public DbSet<BookDetails> BookDetailsTable { get; set; }
        public DbSet<BookAuthorDetails> AuthorDetailsTable { get; set; }
    }
}