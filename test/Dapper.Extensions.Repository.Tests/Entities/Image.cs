using System.ComponentModel.DataAnnotations.Schema;

namespace Dapper.Extensions.Repository.Tests.Entities
{
    [Table("Images")]
    public class Image : Entity
    {
        public int UserId { get; set; }

        public string Name { get; set; }
    }
}
 