using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleemyTest.Data
{
    [Table("Devises")]
    public class Devises
    {
        [Key]
        public int Id { get; set; }
        public string Desc { get; set; }
    }
}
