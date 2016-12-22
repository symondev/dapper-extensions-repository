using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Extensions.Repository.Attributes;

namespace Dapper.Extensions.Repository.Tests.Entities
{
    [Table("Dishes")]
    public class Dish
    {
        [Key, Identity]
        public int DishId { get; set; }

        public string Name { get; set; }

        public IList<DishImage> DishImages { get; set; }

        public IList<DishOption> DishOptions { get; set; }
    }
}
