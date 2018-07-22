using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoCRUD.Data;
using MongoCRUD.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoCRUD.Controllers
{
    public class RentalsController : Controller
    {
        public readonly MongoContext context = new MongoContext();

        // GET: Rentals
        public async Task<ActionResult> Index(RentalsFilter filter)
        {
            var filterDefinition = filter.ToFilterDefinition(filter);

            var rentals = await context.Rentals
                .Find(filterDefinition)
                .ToListAsync();

            var model = new RentalsList
            {
                Rentals = rentals,
                Filters = filter
            };
            return View(model);
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

        public ActionResult Delete(string id)
        {
            try
            {
                context.Rentals.DeleteOne<Rental>(r => r.Id == new MongoDB.Bson.ObjectId(id));
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