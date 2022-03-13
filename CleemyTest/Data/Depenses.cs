using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleemyTest.Data
{
    [Table("Depenses")]
    public class Depenses
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Double Montant { get; set; }
        public String Commentaire { get; set; }
        public int? DeviseFK { get; set; }
        public int? NatureFK { get; set; }
        public int? UtilisateurFK { get; set; }

        [ForeignKey("DeviseFK")]
        public Devises? Devises { get; set; }
        [ForeignKey("NatureFK")]
        public Natures? Natures { get; set; }
        [ForeignKey("UtilisateurFK")]
        public Utilisateurs? Utilisateurs { get; set; }
    }
}
