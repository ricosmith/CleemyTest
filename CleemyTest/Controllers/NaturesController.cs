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
    /// Controller - Natures
    /// </summary>
    /// <remarks></remarks>
    /// <response code="200">Nature Créée</response>
    /// <response code="400">La nature saisie est invalide</response>
    /// <response code="500">Impossible de créer la nature pour le moment</response>
    [Route("api/params/[controller]")]
    [ApiController]
    public class NaturesController : ControllerBase
    {
        private readonly DataContext _context;

        /// <summary> Constructeur </summary>
        public NaturesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/params/Natures
        /// <summary>
        /// Toutes Natures
        /// </summary>
        /// <remarks>Utilisez sort pour trier les resultats</remarks>
        /// <response code="404">Aucune nature trouvée</response>
        [HttpGet("{sort?}")]
        public async Task<ActionResult<IEnumerable<Natures>>> GetNatures(NaturesOrder? sort = null)
        {
            var natures = await _context.Natures.ToListAsync();

            if (natures == null)
            {
                return NotFound();
            }

            if (sort != null)
            { NaturesSort(ref natures, (NaturesOrder)sort); }

            return natures;
        }

        // GET: api/params/Natures/id/5
        /// <summary>
        /// Nature par Id
        /// </summary>
        /// <remarks></remarks>
        /// <response code="404">La nature n'existe pas</response>
        [HttpGet("id/{id}")]
        public async Task<ActionResult<Natures>> GetNature(int id)
        {
            var natures = await _context.Natures.FindAsync(id);

            if (natures == null)
            {
                return NotFound();
            }

            return natures;
        }

        // PUT: api/params/Natures/id/5
        /// <summary>
        /// Mise à jour nature
        /// </summary>
        /// <remarks></remarks>
        /// <response code="400">Erreur de requête</response>
        /// <response code="404">La nature n'existe pas</response>
        /// <response code="204">Nature modifié</response>
        [HttpPut("id/{id}")]
        public async Task<IActionResult> PutNatures(int id, Natures natures)
        {
            if (id != natures.Id)
            {
                return BadRequest();
            }

            _context.Entry(natures).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NaturesExists(id))
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

        // POST: api/params/Natures
        /// <summary>
        /// Ajout nature
        /// </summary>
        /// <remarks></remarks>
        /// <response code="201">Nature Créée</response>
        [HttpPost]
        public async Task<ActionResult<Natures>> PostNatures(Natures natures)
        {
            _context.Natures.Add(natures);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNature", new { id = natures.Id }, natures);
        }

        // J'empeche la suppression des paramètres car pas de cascade

        private bool NaturesExists(int id)
        {
            return _context.Natures.Any(e => e.Id == id);
        }

        private void NaturesSort(ref List<Natures> natures, NaturesOrder sort)
        {
            if (sort == NaturesOrder.descriptionAsc)
            { natures.OrderBy(nat => nat.Desc); }
            else if (sort == NaturesOrder.descriptionDesc)
            { natures.OrderByDescending(nat => nat.Desc); }
        }

        /// <summary> Liste des tries </summary>
        public enum NaturesOrder : ushort
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
