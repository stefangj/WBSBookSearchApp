using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WBSBookSearchApp.Models;
using VDS.RDF.Query;


namespace WBSBookSearchApp.Classes
{
    public class UserSearch
    {
        private readonly BookDbContext _db = new BookDbContext();
        private readonly int _minutesOld = -2;

        public void Search(string searchString)
        {
            if (_db.BookNameTable.Any(o => o.SearchedFor.Equals(searchString)))
            {
                var queryDate = from bnt in _db.BookNameTable
                                where bnt.SearchedFor == searchString
                                select bnt;

                var dateTimeNow = DateTime.Now;
                var dateTimeOldest = dateTimeNow.AddMinutes(_minutesOld);
                var dateTimeSearch = queryDate.FirstOrDefault().DataAndTime;

                if (dateTimeSearch <= dateTimeNow && dateTimeSearch >= dateTimeOldest)
                {

                }
                else if (dateTimeSearch < dateTimeOldest)
                {
                    var books = from b in _db.BookNameTable select b;
                    books = books.Where(s => s.SearchedFor.Equals(searchString));
                    foreach (BookUserSearch i in books)
                    {
                        _db.BookNameTable.Remove(_db.BookNameTable.Find(i.Id));
                    }
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("error removing items from database");
                        Console.WriteLine(e);
                    }

                    //Барање на нова инфромација
                    SparqlResultSet resultSet = Utilities.QueryDbpedia(Utilities.QueryUserSearchBookName(searchString));
                    LoopValuesToDatabase(searchString, resultSet);
                }
                else if (dateTimeSearch > dateTimeNow)
                {
                    Console.WriteLine("ERROR: Database information is newer than possible");
                }
            }
            else
            {
                //Барање на нова инфромација
                SparqlResultSet resultSet = Utilities.QueryDbpedia(Utilities.QueryUserSearchBookName(searchString));
                LoopValuesToDatabase(searchString, resultSet);
            }
        }

        private void LoopValuesToDatabase(String searchString, SparqlResultSet resultSet)
        {
            foreach (SparqlResult result in resultSet)
            {
                String bookLink = result["s"].ToString();
                String name = result["bookName"].ToString();
                String authorLink = result["authorLink"].ToString();
                String author = result["author"].ToString();

                //Бришење на @en
                name = Utilities.RemoveLast3Cahracters(name);
                author = Utilities.RemoveLast3Cahracters(author);

                AddToDatabase(searchString, name, authorLink, author, bookLink);
            }
        }

        private void AddToDatabase(String search, String name, String authorlink, String author, String bookLink)
        {
            BookUserSearch book = new BookUserSearch
            {
                SearchedFor = search,
                Name = name,
                AuthorLink = authorlink,
                Author = author,
                BookLink = bookLink
            };

            _db.BookNameTable.Add(book);

            try
            {
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("error adding to database");
                Console.WriteLine(e);
            }
        }
    }
}