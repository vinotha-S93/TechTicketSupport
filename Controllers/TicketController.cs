using System;
using System.Linq;
using System.Web.Mvc;
using TECHSUPPORTTICKET_MANAGEMENTSYSTEM.Models;
using TECHSUPPORTTICKET_MANAGEMENTSYSTEM.Repository;

namespace TECHSUPPORTTICKET_MANAGEMENTSYSTEM.Controllers
{
    public class TicketController : Controller
    {
        private readonly TicketRepository repo = new TicketRepository();

        public ActionResult Index()
        {
            try
            {
                var list = repo.GetAllTickets();
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error loading tickets: " + ex.Message;
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Ticket ticket)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        System.Diagnostics.Debug.WriteLine("Model error: " + modelError.ErrorMessage);
                    }

                    ViewBag.Message = "Validation failed!";
                    return View(ticket);
                }

                ticket.Status = "Open";
                ticket.AssignedTo = null;

                bool inserted = repo.InsertTicket(ticket);

                if (inserted)
                    return RedirectToAction("Index");

                ViewBag.Message = "Oops! Ticket not inserted.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Exception: " + ex.Message;
            }

            return View(ticket);
        }
    }
}
