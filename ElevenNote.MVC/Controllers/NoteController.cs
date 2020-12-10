using ElevenNote.Data;
using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.MVC.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Note
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            var model = service.GetNotes();

            return View(model);
        }

        // GET: Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");

            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateNoteService();

            if (service.CreateNote(model))
            {
                TempData["SaveResult"] = "Your note was created.";
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", "Note could not be created.");

            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name", model.CategoryId);

            return View(model);
        }

        // GET: Details
        public ActionResult Details(int id)
        {
            var svc = CreateNoteService();
            var model = svc.GetNoteById(id);

            return View(model);
        }

        // GET: Edit
        public ActionResult Edit(int id)
        {
            var service = CreateNoteService();
            var detail = service.GetNoteById(id);

            var model =
                new NoteEdit
                {
                    NoteId = detail.NoteId,
                    Title = detail.Title,
                    Content = detail.Content,
                    CategoryId = detail.CategoryId,
                    IsStarred = detail.IsStarred
                };

            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name", model.CategoryId);

            return View(model);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NoteEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.NoteId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateNoteService();

            if (service.UpdateNote(model))
            {
                TempData["SaveResult"] = "Your note was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your note could not be updated.");

            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name", model.CategoryId);

            return View(model);
        }

        // GET: Delete
        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var svc = CreateNoteService();
            var model = svc.GetNoteById(id);

            return View(model);
        }

        // POST: Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateNoteService();

            service.DeleteNote(id);

            TempData["SaveResult"] = "Your note was deleted";

            return RedirectToAction("Index");
        }

        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            return service;
        }
    }
}