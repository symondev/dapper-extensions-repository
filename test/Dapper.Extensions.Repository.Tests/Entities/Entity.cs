using System.ComponentModel.DataAnnotations;
using Dapper.Extensions.Repository.Attributes;

namespace Dapper.Extensions.Repository.Tests.Entities
{
    public class Entity
    {
        [Key, Identity]
        public int Id { get; set; }
    }
}
