using System.ComponentModel.DataAnnotations.Schema;

namespace Dapper.Extensions.Repository.Tests.Entities
{
    [Table("RecordsForDelete")]
    public class RecordsForDelete : Entity
    {
        public string Name { get; set; }
    }
}
