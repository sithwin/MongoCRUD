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
    public class CustomersController : Controller
    {
        public MongoContext context = new MongoContext();

        public CustomersController()
        {

        }
        
        // GET: Customers
        public ActionResult Index()
        {
            var result = context.mongoDatabase.GetCollection<Customer>("Customers").Find(FilterDefinition<Customer>.Empty).ToList();
            return View(result);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int id)
        {
            
            var result = context.mongoDatabase.GetCollection<Customer>("Customers")
                .Find<Customer>(c => c.CustomerId == id).FirstOrDefault();
            return View(result);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            try
            {
                // TODO: Add insert logic here
                context.mongoDatabase.GetCollection<Customer>("Customers").InsertOne(customer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int id)
        {
            var result = context.mongoDatabase.GetCollection<Customer>("Customers")
                .Find<Customer>(c => c.CustomerId == id).FirstOrDefault();
            return View(result);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Customer customer)
        {
            try
            {
                var filter = Builders<Customer>.Filter.Eq("CustomerId", customer.CustomerId);

                var updateStatement = Builders<Customer>.Update.Set("CustomerId", customer.CustomerId);
                updateStatement = updateStatement.Set("CustomerName", customer.CustomerName);
                updateStatement = updateStatement.Set("Address", customer.Address);

                var result = context.mongoDatabase.GetCollection<Customer>("Customers").UpdateOne(filter, updateStatement);
                if (result.IsAcknowledged == false)
                {
                    return BadRequest("unable to update customer " + customer.CustomerName);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Get the database connection  
            //fetch the details from CustomerDB and pass into view  
            Customer customer = context.mongoDatabase.GetCollection<Customer>("Customers").Find<Customer>(k => k.CustomerId == id).FirstOrDefault();
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Customer customer)
        {
            try
            {
                var result = context.mongoDatabase.GetCollection<Customer>("Customers").DeleteOne<Customer>
                    (c => c.CustomerId == id);

                if (result.IsAcknowledged == false)
                {
                    return BadRequest("Unable to delete customer " + customer.CustomerId);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}