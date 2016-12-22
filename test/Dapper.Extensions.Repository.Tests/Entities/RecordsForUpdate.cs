using System.ComponentModel.DataAnnotations.Schema;

namespace Dapper.Extensions.Repository.Tests.Entities
{
    [Table("RecordsForUpdate")]
    public class RecordsForUpdate : Entity
    {
        public string Name { get; set; }
    }
}
