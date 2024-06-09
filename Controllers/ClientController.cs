using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaffardBankApi.Data;
using HaffardBankApi.Models;

namespace HaffardBankApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly HaffardDbContext _context;

        public ClientController(HaffardDbContext context)
        {
            _context = context;
        }

        // GET: api/Client
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientModel>>> GetClient()
        {
            return await _context.Client.ToListAsync();
        }

        // GET: api/Client/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientModel>> GetClientModel(long id)
        {
            var clientModel = await _context.Client.FindAsync(id);

            if (clientModel == null)
            {
                return NotFound();
            }

            return clientModel;
        }

        // PUT: api/Client/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPatch("{id}")]
        // public async Task<IActionResult> PutClientModel(long id, ClientModel clientModel)
        // {
        //     if (id != clientModel.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(clientModel).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ClientModelExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // // POST: api/Client
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<ClientModel>> PostClientModel(ClientModel clientModel)
        // {
        //     _context.Client.Add(clientModel);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetClientModel", new { id = clientModel.Id }, clientModel);
        // }

        // // DELETE: api/Client/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteClientModel(long id)
        // {
        //     var clientModel = await _context.Client.FindAsync(id);
        //     if (clientModel == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Client.Remove(clientModel);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        // private bool ClientModelExists(long id)
        // {
        //     return _context.Client.Any(e => e.Id == id);
        // }
    }
}
