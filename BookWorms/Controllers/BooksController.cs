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
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public ActionResult Index()
        {
            return View(db.Books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

      

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Author,Genre,ReleaseDate,Publisher,ImageUrl,Content")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                
                System.Diagnostics.Debug.WriteLine("Image URL: " + book.ImageUrl);
                return RedirectToAction("Index");
            }
            return View(book);
        }


       

        [Authorize(Roles = "Admin,Editor")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }


        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Author,Genre,ReleaseDate,Publisher,ImageUrl,Content")] Book book)
        {
            if (ModelState.IsValid)
            {
               
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index"); 
            }
            return View(book); 
        }


     



        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Book book = db.Books.Find(id); 
            db.Books.Remove(book); 
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




        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToFutureBooks(int id)
        {
            var book = db.Books.Find(id);
     
            if (book != null)
            {
                
                var userId = User.Identity.GetUserId();

                bool isBookAlreadyInFutureBooks = db.FutureBooks.Any(f => f.BookId == book.Id 
                && f.UserId == userId);

                bool isBookAlreadyInReadBooks = db.ReadBooks.Any(f => f.BookId == book.Id
                && f.UserId == userId);

                if (isBookAlreadyInFutureBooks)
                {
                    
                    TempData["ErrorMessage"] = "Оваа книга е веќе додадена во списокот 'Книги кои ќе ги читам'.";
                    return RedirectToAction("Index", "FutureBooks");  
                }

                

                if (isBookAlreadyInReadBooks)
                {
                    
                    TempData["ErrorMessage"] = "Оваа книга е веќе додадена во списокот 'Книги кои ѓи имам прочитано'.";
                    return RedirectToAction("Index", "FutureBooks");  
                }

                
                var futureBook = new FutureBook
                {
                    BookId = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Genre = book.Genre,
                    ImageUrl = book.ImageUrl,
                    
                };

                futureBook.UserId = User.Identity.GetUserId();
                db.FutureBooks.Add(futureBook);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "FutureBooks");
        }
    }
    }
