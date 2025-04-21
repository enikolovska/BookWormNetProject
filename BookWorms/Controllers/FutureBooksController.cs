using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookWorms.Models;
using Microsoft.AspNet.Identity;

namespace BookWorms.Controllers
{
    [Authorize(Roles = "User")]
    public class FutureBooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FutureBooks
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var futureBooks = db.FutureBooks.Include(f => f.Book).Where(db => db.UserId == userId);
            return View(futureBooks.ToList());
        }

        // GET: FutureBooks/Details/5
        // GET: Books/Details/5
        public ActionResult Details(int id)
        {
            var futureBook = db.FutureBooks.Include(f => f.Book).FirstOrDefault(f => f.Id == id);
            if (futureBook == null)
            {
                return HttpNotFound();
            }

            return View(futureBook);
        }



        // GET: FutureBooks/Create
        // GET: FutureBooks/Create
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

            var futureBook = new FutureBook
            {
                Book = book 
            };

            return View(futureBook);
        }

        // POST: FutureBooks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookId")] FutureBook futureBook)
        {
            if (ModelState.IsValid)
            {
                var book = db.Books.Find(futureBook.BookId);
                if (book == null)
                {
                    return HttpNotFound();
                }

                futureBook.Book = book; // Set the book to the FutureBook
                db.FutureBooks.Add(futureBook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookId = new SelectList(db.Books, "Id", "ImageUrl", futureBook.BookId);
            return View(futureBook);
        }


        // GET: FutureBooks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            FutureBook futureBook = db.FutureBooks.Include(f => f.Book).FirstOrDefault(f => f.Id == id);
            if (futureBook == null)
            {
                return HttpNotFound();
            }

            ViewBag.BookId = new SelectList(db.Books, "Id", "ImageUrl", futureBook.BookId);
            return View(futureBook);
        }

        // POST: FutureBooks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, BookId")] FutureBook futureBook)
        {
            if (ModelState.IsValid)
            {
                var book = db.Books.Find(futureBook.BookId);
                if (book == null)
                {
                    ModelState.AddModelError("", "Избраната книга не постои во базата.");
                }
                else
                {
                    futureBook.Book = book;
                    db.Entry(futureBook).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.BookId = new SelectList(db.Books, "Id", "ImageUrl", futureBook.BookId);
            return View(futureBook);
        }


        // GET: FutureBooks/Delete/5
        
        public ActionResult Delete(int id)
        {
            FutureBook futureBook = db.FutureBooks.Include(f => f.Book).FirstOrDefault(f => f.Id == id);
            db.FutureBooks.Remove(futureBook);
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

        public ActionResult ReadBooks(int id)
        {
            var futureBook = db.FutureBooks.Find(id); 
            if (futureBook == null)
            {
                return HttpNotFound();
            }

            
            var readBook = new ReadBook
            {
                BookId = futureBook.BookId,
                Rating = 0, 
                Comment = "", 
                ReadDate = DateTime.Now, 
                Type = "" 
            };

            
            return RedirectToAction("CreateFromFutureBooks", "ReadBooks", new { readBook = readBook });
        }
    }
}
