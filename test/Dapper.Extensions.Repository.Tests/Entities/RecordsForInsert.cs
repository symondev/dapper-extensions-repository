using System.ComponentModel.DataAnnotations.Schema;

namespace Dapper.Extensions.Repository.Tests.Entities
{
    [Table("RecordsForInsert")]
    public class RecordsForInsert : Entity
    {
        public string Name { get; set; }
    }
}
