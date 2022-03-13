#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CleemyTest.Data;
using CleemyTest.Models;

namespace CleemyTest.Controllers
{
    /// <summary>
    /// Controller - Utilisateurs
    /// </summary>
    /// <remarks></remarks>
    /// <response code="200">Utilisateur Créé</response>
    /// <response code="400">L'utilisateur saisi est invalide</response>
    /// <response code="500">Impossible de créer l'utilisateur pour le moment</response>
    [Route("api/params/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly DataContext _context;

        /// <summary> Constructeur </summary>
        public UtilisateursController(DataContext context)
        {
            _context = context;
        }

        // GET: api/params/Utilisateurs
        /// <summary>
        /// Tout Utilisateurs
        /// </summary>
        /// <remarks>Utilisez sort pour trier les resultats</remarks>
        /// <response code="404">Aucun utilisateur trouvé</response>
        [HttpGet("{sort?}")]
        public async Task<ActionResult<IEnumerable<Utilisateurs>>> GetUtilisateurs(UtilisateursOrder? sort = null)
        {
            var utilisateurs = await _context.Utilisateurs.Include(dep => dep.Devises).ToListAsync();

            if (utilisateurs == null)
            {
                return NotFound();
            }

            if (sort != null)
            { UtilisateursSort(ref utilisateurs, (UtilisateursOrder)sort); }

            return utilisateurs;
        }

        // GET: api/params/Utilisateurs/id/5
        /// <summary>
        /// Utilisateur par Id
        /// </summary>
        /// <remarks></remarks>
        /// <response code="404">L'utilisateur n'existe pas</response>
        [HttpGet("id/{id}")]
        public async Task<ActionResult<Utilisateurs>> GetUtilisateur(int id)
        {
            var utilisateurs = await _context.Utilisateurs.Include(dep => dep.Devises).FirstOrDefaultAsync(i => i.Id == id);

            if (utilisateurs == null)
            {
                return NotFound();
            }

            return utilisateurs;
        }

        // PUT: api/params/Utilisateurs/id/5
        /// <summary>
        /// Mise à jour utilisateur
        /// </summary>
        /// <remarks></remarks>
        /// <response code="400">Erreur de requête</response>
        /// <response code="404">L'utilisateur n'existe pas</response>
        /// <response code="204">Utilisateur modifié</response>
        [HttpPut("id/{id}")]
        public async Task<IActionResult> PutUtilisateurs(int id, UtilisateursModel utilisateurs)
        {
            if (id != utilisateurs.Id)
            {
                return BadRequest();
            }

            var utilisateurToModify = _context.Utilisateurs.Find(id);
            utilisateurToModify.Nom = utilisateurs.Nom;
            utilisateurToModify.Prenom = utilisateurs.Prenom;
            utilisateurToModify.DeviseFK = utilisateurs.DeviseFK;

            _context.Entry(utilisateurToModify).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilisateursExists(id))
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

        // POST: api/params/Utilisateurs
        /// <summary>
        /// Ajout utilisateur
        /// </summary>
        /// <remarks></remarks>
        /// <response code="201">Utilisateur Créée</response>
        [HttpPost]
        public async Task<ActionResult<Utilisateurs>> PostUtilisateurs(UtilisateursModel utilisateurs)
        {

            var utilisateurToCreate = new Utilisateurs
            {
                Nom = utilisateurs.Nom,
                Prenom = utilisateurs.Prenom,
                DeviseFK = utilisateurs.DeviseFK
            };

            _context.Utilisateurs.Add(utilisateurToCreate);
            await _context.SaveChangesAsync();

            return await GetUtilisateur(utilisateurToCreate.Id);
        }

        // J'empeche la suppression de l'utilisateur par précaution

        private bool UtilisateursExists(int id)
        {
            return _context.Utilisateurs.Any(e => e.Id == id);
        }

        private void UtilisateursSort(ref List<Utilisateurs> utilisateurs, UtilisateursOrder sort)
        {

            if (sort == UtilisateursOrder.NomAsc)
            { utilisateurs.OrderBy(dev => dev.Nom); }
            else if (sort == UtilisateursOrder.NomDesc)
            { utilisateurs.OrderByDescending(dev => dev.Nom); }
            else if(sort == UtilisateursOrder.PrenomAsc)
            { utilisateurs.OrderBy(dev => dev.Prenom); }
            else if (sort == UtilisateursOrder.PrenomDesc)
            { utilisateurs.OrderByDescending(dev => dev.Prenom); }
        }

        /// <summary> Liste des tries </summary>
        public enum UtilisateursOrder : ushort
        {
            /// <summary> pas de trie </summary>
            nothing = 0,
            /// <summary> nom ascendant </summary>
            NomAsc = 1,
            /// <summary> nom descendant </summary>
            NomDesc = 2,
            /// <summary> prenom ascendant </summary>
            PrenomAsc = 3,
            /// <summary> prenom descendant </summary>
            PrenomDesc = 4
        }
    }
}
