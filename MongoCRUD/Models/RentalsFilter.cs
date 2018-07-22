using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoCRUD.Models
{
    public class RentalsFilter
    {
        public decimal? PriceLimit { get; set; }
        public int? MinimumRooms { get; set; }

        public FilterDefinition<Rental> ToFilterDefinition(RentalsFilter filter)
        {
            var filterDefinition = Builders<Rental>.Filter.Empty;

            if (filter.MinimumRooms.HasValue)
            {
                filterDefinition &= Builders<Rental>.Filter
                    .Gte(r => r.NumberOfRooms, filter.MinimumRooms);
                //.Where(r => r.NumberOfRooms >= filter.MinimumRooms);
            }

            if (filter.PriceLimit.HasValue)
            {
                filterDefinition &= Builders<Rental>.Filter
                .Where(r => r.Price <= filter.PriceLimit);
            }

            return filterDefinition;
        }
    }
}
