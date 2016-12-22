using System.ComponentModel.DataAnnotations.Schema;

namespace Dapper.Extensions.Repository.Tests.Entities
{
    [Table("RecordsForInsertAsync")]
    public class RecordsForInsertAsync : Entity
    {
        public string Name { get; set; }
    }
}
