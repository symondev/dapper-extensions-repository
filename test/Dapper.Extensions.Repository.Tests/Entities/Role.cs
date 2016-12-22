using System.ComponentModel.DataAnnotations.Schema;

namespace Dapper.Extensions.Repository.Tests.Entities
{
    [Table("Roles")]
    public class Role : Entity
    {
        public int UserId { get; set; }

        public string Name { get; set; }
    }
}
 