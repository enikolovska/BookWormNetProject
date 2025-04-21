using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookWorms.Models;
using Microsoft.AspNet.Identity;

namespace BookWorms.Controllers
{
    [Authorize(Roles = "User")]
    public class ReadBooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ReadBooks
        public ActionResult Index()
        {
             

            string userId = User.Identity.GetUserId();
            var readBooks = db.ReadBooks.Include(f => f.Book).Where(db => db.UserId == userId).ToList();
            return View(readBooks.ToList());


        }

        // GET: ReadBooks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReadBook readBook = db.ReadBooks.Find(id);
            if (readBook == null)
            {
                return HttpNotFound();
            }
            return View(readBook);
        }

        // GET: ReadBooks/Create
        public ActionResult Create(int? bookId)
        {
            if (bookId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var book = db.Books.Find(bookId);
            if (book == null)
            {
                return HttpNotFound();
            }

            var readBook = new ReadBook
            {
                Book = book
            };

            return View(readBook);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Rating, Comment, ReadDate, Type, BookId")] ReadBook readBook)
        {
            if (ModelState.IsValid)
            {
                readBook.UserId = User.Identity.GetUserId();
                var userId = User.Identity.GetUserId();


                var existingReadBook = db.ReadBooks.FirstOrDefault(rb => rb.BookId == readBook.BookId && rb.UserId == userId);
                if (existingReadBook != null)
                {
                    ModelState.AddModelError("", "Оваа книга веќе е додадена во Прочитани книги.");
                    readBook.Book = db.Books.Find(readBook.BookId);
                    ViewBag.IsDuplicate = true;
                    return View(readBook);
                }

                var book = db.Books.Find(readBook.BookId);
                if (book == null)
                {
                    return HttpNotFound();
                }

                var futureBook = db.FutureBooks.FirstOrDefault(fb => fb.BookId == readBook.BookId && fb.UserId == userId);
                if (futureBook != null)
                {
                    db.FutureBooks.Remove(futureBook);
                    db.SaveChanges();
                }

                readBook.Book = book;
                db.ReadBooks.Add(readBook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            readBook.Book = db.Books.Find(readBook.BookId); 
            return View(readBook);
        }




        // GET: ReadBooks/Edit/5
        public ActionResult Edit(int? id)
        {
            string userId = User.Identity.GetUserId();
            var readBook = db.ReadBooks.Include(rb => rb.Book)
                                       .FirstOrDefault(rb => rb.Id == id && rb.UserId == userId);

            if (readBook == null)
            {
                return HttpNotFound();
            }
            return View(readBook);
        }

        // POST: ReadBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BookId,Rating,Comment,ReadDate,Type")] ReadBook readBook)
        {
            
            {
                if (ModelState.IsValid)
                {
                    string userId = User.Identity.GetUserId(); 

                    
                    var existingReadBook = db.ReadBooks.FirstOrDefault(rb => rb.Id == readBook.Id && rb.UserId == userId);

                    if (existingReadBook == null)
                    {
                        return HttpNotFound(); 
                    }

                    
                    existingReadBook.Rating = readBook.Rating;
                    existingReadBook.Comment = readBook.Comment;
                    existingReadBook.ReadDate = readBook.ReadDate;
                    existingReadBook.Type = readBook.Type;

                    db.SaveChanges();
                    return RedirectToAction("Index"); 
                }

                
                var existingBook = db.Books.Find(readBook.BookId);
                if (existingBook != null)
                {
                    ViewBag.BookTitle = existingBook.Title;
                    ViewBag.BookAuthor = existingBook.Author;
                    ViewBag.BookGenre = existingBook.Genre;
                    ViewBag.BookImageUrl = existingBook.ImageUrl;
                }

                return View(readBook);
            }
        }




        // GET: ReadBooks/Delete/5

        public ActionResult Delete(int id)
        {
            ReadBook book = db.ReadBooks.Find(id); 
            db.ReadBooks.Remove(book); 
            db.SaveChanges(); 
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [Route("ReadBooks/CreateFromFutureBooks")]
        public ActionResult CreateFromFutureBooks(ReadBook readBook)
        {
            if (ModelState.IsValid)
            {
                db.ReadBooks.Add(readBook);
                db.SaveChanges();

                var futureBook = db.FutureBooks.Find(readBook.BookId);
                if (futureBook != null)
                {
                    db.FutureBooks.Remove(futureBook);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "ReadBooks");
            }

            
            return View("Create", readBook);
        }
    }
}
