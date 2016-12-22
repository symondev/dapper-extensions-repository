using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Extensions.Repository.Attributes;

namespace Dapper.Extensions.Repository.Tests.Entities
{
    [Table("DishOptions")]
    public class DishOption
    {
        [Key, Identity]
        public int DishOptionId { get; set; }

        public int DishId { get; set; }

        public string Option { get; set; }
    }
}
