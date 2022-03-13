using CleemyTest.Controllers;
using CleemyTest.Data;

namespace CleemyTest.Methodes
{
    public class DepensesModel
    {
        public int Id { get; set; }
        public int UtilisateurFK { get; set; }
        public DateTime Date { get; set; }
        public int NatureFK { get; set; }
        public int DeviseFK { get; set; }
        public Double Montant { get; set; }
        public String Commentaire { get; set; }

        public DepensesModel(int id, int utilisateurFK, DateTime date, int natureFK, int deviseFK, Double montant, String commentaire)
        {
            Id = id;
            UtilisateurFK = utilisateurFK;
            Date = date;
            NatureFK = natureFK;
            DeviseFK = deviseFK;
            Montant = montant;
            Commentaire = commentaire;
        }

        public ControllerProblemClass? VerifDepense(DataContext context)
        {
            //Verifs Clés
            var VerifUtil = context.Utilisateurs.Find(this.UtilisateurFK);
            var VerifDevise = context.Natures.Find(this.DeviseFK);
            var VerifNature = context.Devises.Find(this.NatureFK);
            if (VerifUtil == null)
            { return new ControllerProblemClass("Utilisateur", "L'utilisateur n'existe pas", "Erreur", 418); }
            else if (VerifDevise == null)
            { return new ControllerProblemClass("Devises", "La devise n'existe pas", "Erreur", 418); }
            else if (VerifNature == null)
            { return new ControllerProblemClass("Natures", "La nature n'existe pas", "Erreur", 418); }

            // Verifs de Cohérences
            if (this.Date > DateTime.Today)
            { return new ControllerProblemClass("Date", "Date dans le futur", "Erreur", 418); }
            else if (this.Date < DateTime.Today.AddMonths(-3))
            { return new ControllerProblemClass("Date", "Date de moins de 3 mois", "Erreur", 418); }
            else if (this.Commentaire == null || this.Commentaire == "")
            { return new ControllerProblemClass("Commentaire", "Le commentaire est obligatoire", "Erreur", 418); }
            else if (this.DeviseFK != VerifUtil.DeviseFK)
            { return new ControllerProblemClass("Devises", "La devise de dépense doit être la même que celle de l'utilisateur", "Erreur", 418); }

            // Verifs de Doublons
            if (context.Depenses.Any(dep => dep.Utilisateurs.Id == VerifUtil.Id && dep.Date == this.Date && dep.Montant == this.Montant))
            { return new ControllerProblemClass("Depenses", "Une dépense similaire est déjà présente", "Erreur", 418); }

            return null;
        }
    }
}
