namespace CleemyTest.Models
{
    public class UtilisateursModel
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int DeviseFK { get; set; }

        public UtilisateursModel(int id, string nom, string prenom,int deviseFK)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            DeviseFK = deviseFK;
        }
    }
}
