using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WBSBookSearchApp.Classes;
using WBSBookSearchApp.Models;
using PagedList;
using System.Data.Entity;

namespace WBSBookSearchApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookDbContext _db = new BookDbContext();

        private readonly UserSearch _userSearch = new UserSearch();

        private readonly AuthorDetailsSearch _authorSearch = new AuthorDetailsSearch();

        public async Task<ActionResult> Index(string id = null, int page = 1, int pageSize = 10)
        {
            if (!String.IsNullOrEmpty(id))
            {
                if (!String.IsNullOrEmpty(id.Trim()))
                {
                    _userSearch.Search(id);
                }
            }

            var books = from b in _db.BookNameTable select b;

            if (!String.IsNullOrEmpty(id))
            {
                if (!String.IsNullOrEmpty(id.Trim()))
                {
                    books = books.Where(s => s.SearchedFor.Equals(id));
                }
            }

            IEnumerable<BookUserSearch> bookList = await books.OrderByDescending(s => s.DataAndTime).ToListAsync();

            PagedList<BookUserSearch> model = new PagedList<BookUserSearch>(bookList, page, pageSize);

            return View(model);
        }

        public ActionResult AuthorDetails(string authorLink)
        {
            if (!String.IsNullOrEmpty(authorLink))
            {
                if (!String.IsNullOrEmpty(authorLink.Trim()))
                {
                    _authorSearch.Search(authorLink);
                }
                else
                {
                    Response.Redirect("Index");
                }
            }
            else
            {
                Response.Redirect("Index");
            }

            var details = from b in _db.AuthorDetailsTable select b;
            details = details.Where(s => s.AuthorLink.Equals(authorLink));

            return View(details);
        }

        public ActionResult BookDetails(string bookLink, string authorLink)
        {
            if (!String.IsNullOrEmpty(authorLink))
            {
                if (!String.IsNullOrEmpty(authorLink.Trim()))
                {
                    _authorSearch.Search(authorLink);
                }
                else
                {
                    Response.Redirect("Index");
                }
            }
            else
            {
                Response.Redirect("Index");
            }

            var details = from b in _db.BookDetailsTable select b;
            details = details.Where(s => s.BookLink.Equals(bookLink));

            return View(details);
        }

        ~HomeController()
        {
            _db.Dispose();
        }

        public new void Dispose()
        {
            _db.Dispose();
        }
    }
}