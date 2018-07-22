using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoCRUD.Models
{
    public class RentalsList
    {
        public IEnumerable<RentalViewModel> Rentals { get; set; }
        public RentalsFilter Filters { get; set; }
    }

    // For Projection
    public class RentalViewModel
    {
        public ObjectId Id { get; set; }
        public string Description { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal Price { get; set; }
        public List<string> Address = new List<string>();
    }
}
