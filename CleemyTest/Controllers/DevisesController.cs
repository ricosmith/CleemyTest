#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CleemyTest.Data;

namespace CleemyTest.Controllers
{
    /// <summary>
    /// Controller - Devises
    /// </summary>
    /// <remarks></remarks>
    /// <response code="200">Devise Créée</response>
    /// <response code="400">La devise saisie est invalide</response>
    /// <response code="500">Impossible de créer la devise pour le moment</response>
    [Route("api/params/[controller]")]
    [ApiController]
    public class DevisesController : ControllerBase
    {
        private readonly DataContext _context;

        /// <summary> Constructeur </summary>
        public DevisesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/params/Devises
        /// <summary>
        /// Toutes Devises
        /// </summary>
        /// <remarks>Utilisez sort pour trier les resultats</remarks>
        /// <response code="404">Aucune devise trouvée</response>
        [HttpGet("{sort?}")]
        public async Task<ActionResult<IEnumerable<Devises>>> GetDevises(DevisesOrder? sort = null)
        {
            var devises = await _context.Devises.ToListAsync();

            if (devises == null)
            {
                return NotFound();
            }

            if (sort != null)
            { DevisesSort(ref devises, (DevisesOrder)sort); }

            return devises;
        }

        // GET: api/params/Devises/id/5
        /// <summary>
        /// Devise par Id
        /// </summary>
        /// <remarks></remarks>
        /// <response code="404">La devise n'existe pas</response>
        [HttpGet("id/{id}")]
        public async Task<ActionResult<Devises>> GetDevise(int id)
        {
            var devises = await _context.Devises.FindAsync(id);

            if (devises == null)
            {
                return NotFound();
            }

            return devises;
        }

        // PUT: api/params/Devises/5
        /// <summary>
        /// Mise à jour devise
        /// </summary>
        /// <remarks></remarks>
        /// <response code="400">Erreur de requête</response>
        /// <response code="404">La devise n'existe pas</response>
        /// <response code="204">Devise modifié</response>
        [HttpPut("id/{id}")]
        public async Task<IActionResult> PutDevises(int id, Devises devises)
        {
            if (id != devises.Id)
            {
                return BadRequest();
            }

            _context.Entry(devises).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DevisesExists(id))
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

        // POST: api/params/Devises
        /// <summary>
        /// Ajout devise
        /// </summary>
        /// <remarks></remarks>
        /// <response code="201">Devise Créée</response>
        [HttpPost]
        public async Task<ActionResult<Devises>> PostDevises(Devises devises)
        {
            _context.Devises.Add(devises);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDevise", new { id = devises.Id }, devises);
        }

        // J'empeche la suppression des paramètres car pas de cascade

        private bool DevisesExists(int id)
        {
            return _context.Devises.Any(e => e.Id == id);
        }

        private void DevisesSort(ref List<Devises> devises, DevisesOrder sort)
        {
            if (sort == DevisesOrder.descriptionAsc)
            { devises.OrderBy(dev => dev.Desc); }
            else if (sort == DevisesOrder.descriptionDesc)
            { devises.OrderByDescending(dev => dev.Desc); }
        }

        /// <summary> Liste des tries </summary>
        public enum DevisesOrder : ushort
        {
            /// <summary> pas de trie </summary>
            nothing = 0,
            /// <summary> description ascendant </summary>
            descriptionAsc = 1,
            /// <summary> description descendant </summary>
            descriptionDesc = 2
        }
    }
}
