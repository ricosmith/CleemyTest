using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleemyTest.Data
{
    [Table("Utilisateurs")]
    public class Utilisateurs
    {
        [Key]
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get ; set; }
        public int? DeviseFK { get; set; }

        [ForeignKey("DeviseFK")]
        public Devises? Devises { get; set; }
    }
}
