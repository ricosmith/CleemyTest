#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CleemyTest.Data;
using System.Text;
using CleemyTest.Methodes;

namespace CleemyTest.Controllers
{
    /// <summary>
    /// Controller - Depenses
    /// </summary>
    /// <remarks></remarks>
    /// <response code="200">Depense Créée</response>
    /// <response code="400">La depense saisie est invalide</response>
    /// <response code="500">Impossible de créer la depense pour le moment</response>
    [Route("api/[controller]")]
    [ApiController]
    public class DepensesController : ControllerBase
    {
        private readonly DataContext _context;

        /// <summary> Constructeur </summary>
        public DepensesController(DataContext context)
        {
            _context = context;
        }

        // Desactivation de la fonction getAll pour eviter les surcharges de la base sql

        // GET: api/Depenses/id/{DepensesId}
        /// <summary>
        /// Depense par Id
        /// </summary>
        /// <remarks></remarks>
        /// <response code="404">La Depense n'existe pas</response>
        [HttpGet("id/{id}")]
        public async Task<ActionResult<Depenses>> GetDepense(int id)
        {
            var depenses = await _context.Depenses.Include(dep => dep.Devises).Include(dep => dep.Natures).Include(dep => dep.Utilisateurs).FirstOrDefaultAsync(i => i.Id == id);

            if (depenses == null)
            {
                return NotFound();
            }

            return depenses;
        }

        // GET: api/Depenses/Utilisateur/{UtilisateursId}/{sort}
        /// <summary>
        /// Depenses par utilisateur
        /// </summary>
        /// <remarks>Utilisez sort pour trier les resultats</remarks>
        /// <response code="404">Aucune depense trouvée</response>
        [HttpGet("Utilisateur/{id}/{sort?}")]
        public async Task<ActionResult<IEnumerable<Depenses>>> GetDepensesUtilisateur(int id, DepensesOrder? sort = null)
        {
            var depenses = await _context.Depenses.Include(dep => dep.Devises).Include(dep => dep.Natures).Include(dep => dep.Utilisateurs).Where(dep => dep.UtilisateurFK == id).ToListAsync();

            if (depenses == null)
            {
                return NotFound();
            }

            if (sort != null)
            { DepensesSort(ref depenses, (DepensesOrder)sort); }
            
            return depenses;
        }

        // GET: api/Depenses/AffichDesc/{Id}
        /// <summary>
        /// Affichage texte d'une dépense
        /// </summary>
        /// <remarks></remarks>
        /// <response code="404">La depense n'existe pas</response>
        [HttpGet("AffichDesc/{id}")]
        public async Task<ActionResult<string>> GetAffichDesc(int id)
        {
            var depenses = await _context.Depenses.Include(dep => dep.Devises).Include(dep => dep.Natures).Include(dep => dep.Utilisateurs).FirstOrDefaultAsync(i => i.Id == id);

            if (depenses == null)
            {
                return NotFound();
            }

            var Result = new StringBuilder();
            Result.Append("Id:" + depenses.Id);
            Result.Append(", Utilisateur:" + depenses.Utilisateurs.Nom + " " + depenses.Utilisateurs.Prenom);
            Result.Append(", Date:" + depenses.Date);
            Result.Append(", Nature:" + depenses.Natures.Desc);
            Result.Append(", Devise:" + depenses.Devises.Desc);
            Result.Append(", Montant:" + depenses.Montant.ToString());
            Result.Append(", Commentaire:" + depenses.Commentaire);

            return Result.ToString();
        }

        // PUT: api/Depenses/id/5
        /// <summary>
        /// Mise à jour depense
        /// </summary>
        /// <remarks></remarks>
        /// <response code="400">Erreur de requête</response>
        /// <response code="404">La depense n'existe pas</response>
        /// <response code="418">Erreur de saisie dans la depense (voir retour)</response>
        /// <response code="204">Depense modifiée</response>
        [HttpPut("id/{id}")]
        public async Task<IActionResult> PutDepenses(int id, DepensesModel depenses)
        {
            var Verif = depenses.VerifDepense(_context);
            if (Verif != null)
            {
                return Problem(Verif.Detail,null, Verif.StatusCode, Verif.Title, Verif.Type);
            }

            var depenseToModify = _context.Depenses.Find(id);
            depenseToModify.Date = depenses.Date;
            depenseToModify.Montant = depenses.Montant;
            depenseToModify.Commentaire = depenses.Commentaire;
            depenseToModify.DeviseFK = depenses.DeviseFK;
            depenseToModify.NatureFK = depenses.NatureFK;
            depenseToModify.UtilisateurFK = depenses.UtilisateurFK;

            _context.Entry(depenseToModify).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepensesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Depenses
        /// <summary>
        /// Ajout depense
        /// </summary>
        /// <remarks></remarks>
        /// <response code="418">Erreur de saisie dans la depense (voir retour)</response>
        /// <response code="201">Depense Créée</response>
        [HttpPost]
        public async Task<ActionResult<Depenses>> PostDepenses(DepensesModel depenses)
        {
            var Verif = depenses.VerifDepense(_context);
            if (Verif != null)
            {
                return Problem(Verif.Detail, null, Verif.StatusCode, Verif.Title, Verif.Type);
            }

            var depenseToCreate = new Depenses
            {
                Date = depenses.Date,
                Montant = depenses.Montant,
                Commentaire = depenses.Commentaire,
                DeviseFK = depenses.DeviseFK,
                NatureFK = depenses.NatureFK,
                UtilisateurFK = depenses.UtilisateurFK
            };

            _context.Depenses.Add(depenseToCreate);
            await _context.SaveChangesAsync();

            return await GetDepense(depenseToCreate.Id);
        }

        // DELETE: api/Depenses/5
        /// <summary>
        /// Suppression depense
        /// </summary>
        /// <remarks></remarks>
        /// <response code="404">La depense n'existe pas</response>
        /// <response code="204">Depense suprimée</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepenses(int id)
        {
            var depenses = await _context.Depenses.FindAsync(id);
            if (depenses == null)
            {
                return NotFound();
            }

            _context.Depenses.Remove(depenses);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepensesExists(int id)
        {
            return _context.Depenses.Any(e => e.Id == id);
        }

        private void DepensesSort(ref List<Depenses> depenses, DepensesOrder sort)
        {
            if (sort == DepensesOrder.montantAsc)
            { depenses.OrderBy(dep => dep.Montant); }
            else if (sort == DepensesOrder.montantDesc)
            { depenses.OrderByDescending(dep => dep.Montant); }
            else if (sort == DepensesOrder.dateAsc)
            { depenses.OrderBy(dep => dep.Date); }
            else if (sort == DepensesOrder.dateDesc)
            { depenses.OrderByDescending(dep => dep.Date); }
            else if (sort == DepensesOrder.NatureAsc)
            { depenses.OrderBy(dep => dep.Natures.Desc); }
            else if (sort == DepensesOrder.NatureDesc)
            { depenses.OrderByDescending(dep => dep.Natures.Desc); }
        }

        /// <summary> Liste des tries </summary>
        public enum DepensesOrder : ushort
        {
            /// <summary> pas de trie </summary>
            nothing = 0,
            /// <summary> montant ascendant </summary>
            montantAsc = 1,
            /// <summary> montant descendant </summary>
            montantDesc = 2,
            /// <summary> date ascendant </summary>
            dateAsc = 3,
            /// <summary> date descendant </summary>
            dateDesc = 4,
            /// <summary> nature ascendant </summary>
            NatureAsc = 5,
            /// <summary> nature descendant </summary>
            NatureDesc = 6
        }
    }
}
