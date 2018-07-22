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
                //.Sort(Builders<Rental>.Sort.Ascending(r => r.Price))
                // Only Need below fields
                .Project(r => new RentalViewModel
                {
                    Id = r.Id,
                    Address = r.Address,
                    NumberOfRooms = r.NumberOfRooms,
                    Description = r.Description,
                    Price = r.Price
                })
                .SortByDescending(r => r.Price)
                .ThenBy(r => r.NumberOfRooms)
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
        public async Task<ActionResult> Create(PostRental postRental)
        {
            try
            {
                var rental = new Rental(postRental);
                await context.Rentals.InsertOneAsync(rental);
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
        public async Task<ActionResult> AdjustPrice(string id, AdjustPrice adjustPrice)
        {
            var rental = GetRental(id);
            rental.AdjustPrice(adjustPrice);
            var result = await context.Rentals.ReplaceOneAsync(r => r.Id == new ObjectId(id), rental);

            if (result.IsAcknowledged == false)
            {
                return BadRequest("unable to Adjust price " + rental.Price);
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(string id)
        {
            Rental result = GetRental(id);
            return View(result);
        }

        // UPDATE
        //[HttpPost]
        //public  ActionResult Edit(string id, PostRental postRental)
        //{
        //    var rental = GetRental(id);
        //    var modifiedRental = Builders<Rental>.Update
        //        .Set(r => r.Description, postRental.Description)
        //        .Set(r => r.Price, postRental.Price)
        //        .Set(r => r.NumberOfRooms, postRental.NumberOfRooms);
        //    context.Rentals.UpdateOne(r => r.Id == new ObjectId(id), modifiedRental);
        //    return RedirectToAction(nameof(Index));
        //}

        // Upsert
        [HttpPost]
        public ActionResult Edit(string id, PostRental postRental)
        {
            var rental = GetRental(id);
            UpdateOptions options = new UpdateOptions
            {
                IsUpsert = true
            };

            context.Rentals.ReplaceOne(r => r.Id == new ObjectId(id), rental, options);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(string id)
        {
            try
            {
                context.Rentals.DeleteOne(r => r.Id == new ObjectId(id));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private Rental GetRental(string id)
        {
            return context.Rentals
                .Find(r => r.Id == new ObjectId(id))
                .FirstOrDefault();
        }

    }
}