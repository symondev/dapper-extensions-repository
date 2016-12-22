using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Extensions.Repository.Attributes;

namespace Dapper.Extensions.Repository.Tests.Entities
{
    [Table("DishImages")]
    public class DishImage
    {
        [Key, Identity]
        public int DishImageId { get; set; }

        public int DishId { get; set; }

        public string Image { get; set; }
    }
}
