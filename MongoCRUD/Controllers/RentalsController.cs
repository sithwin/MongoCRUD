using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoCRUD.Data;
using MongoCRUD.Models;
using MongoDB.Driver;

namespace MongoCRUD.Controllers
{
    public class RentalsController : Controller
    {
        public readonly MongoContext context = new MongoContext();

        // GET: Rentals
        public ActionResult Index()
        {
            var result = context.Rentals.Find(FilterDefinition<Rental>.Empty).ToList();
            return View(result);
        }

        // GET: Rentals/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Rentals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rentals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PostRental postRental)
        {
            try
            {
                var rental = new Rental(postRental);
                context.Rentals.InsertOne(rental);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AdjustPrice(string id)
        {
            Rental result = GetRental(id);
            return View(result);
        }

        
        [HttpPost]
        public ActionResult AdjustPrice(string id, AdjustPrice adjustPrice)
        {
            
            var rental = GetRental(id);
            rental.AdjustPrice(adjustPrice);
            var result = context.Rentals.ReplaceOne(r => r.Id == new MongoDB.Bson.ObjectId(id), rental);
            if (result.IsAcknowledged == false)
            {
                return BadRequest("unable to Adjust price " + rental.Price);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Rentals/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Rentals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Rentals/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Rentals/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private Rental GetRental(string id)
        {
            return context.Rentals.Find<Rental>(r => r.Id == new MongoDB.Bson.ObjectId(id)).FirstOrDefault();
        }

    }
}