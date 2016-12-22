using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Extensions.Repository.Attributes.LogicalDelete;

namespace Dapper.Extensions.Repository.Tests.Entities
{
    [Table("Users")]
    public class User : Entity
    {
        public string Name { get; set; }

        [Status, Deleted]
        public bool Deleted { get; set; }

        public IList<Car> Cars { get; set; }

        public IList<Role> Roles { get; set; }

        public Image Image { get; set; }
    }
}
