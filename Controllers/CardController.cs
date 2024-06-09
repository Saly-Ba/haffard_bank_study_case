using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaffardBankApi.Data;
using HaffardBankApi.Models;
using HaffardBankApi.Services;

namespace HaffardBankApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly HaffardDbContext _context;
        private readonly IPinService _pinService;

        public CardController(HaffardDbContext context,  IPinService pinService)
        {
            _context = context;
            _pinService = pinService;
        }

        // GET: api/Card
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardModel>>> GetCard()
        {
            return await _context.Card.ToListAsync();
        }

        // GET: api/Card/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CardModel>> GetCardModel(long id)
        {
            var cardModel = await _context.Card.FindAsync(id);

            if (cardModel == null)
            {
                return NotFound();
            }

            return cardModel;
        }

        [HttpGet("pin/{clientId}")]
        public async Task<IActionResult> GetPin(long clientId)
        {
            var cardModel = await _context.Card.Include(cr => cr.ClientModel).FirstOrDefaultAsync(cr => cr.ClientId == clientId);

            if (cardModel == null)
            {
                return NotFound();
            }

            var pin = _pinService.DecryptPin(cardModel.Pin!);

            return Ok(new {pin = pin});
        }

        // PUT: api/Card/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("activate/{id}")]
        public async Task<IActionResult> ActivateCard(long id, string pin)
        {
            var cardModel = await _context.Card.FindAsync(id);
            var encryptedPin = _pinService.EncryptPin(pin);

            if (cardModel == null)
            {
                return NotFound();
            }
            
            if(cardModel.Pin != encryptedPin)
            {
                return BadRequest();
            }

            cardModel.IsActive = true;

            _context.Entry(cardModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new {message = "card status updated"});
        }

        [HttpPatch("deactivate/{id}")]
        public async Task<IActionResult> DeactivateCard(long id, string pin)
        {
            var cardModel = await _context.Card.FindAsync(id);
            var encryptedPin = _pinService.EncryptPin(pin);

            if (cardModel == null)
            {
                return NotFound();
            }

            if(cardModel.Pin != encryptedPin)
            {
                return BadRequest();
            }

            cardModel.IsActive = false;

            _context.Entry(cardModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new {message = "card status updated"});
        }

        // POST: api/Card
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<CardModel>> PostCardModel(CardModel cardModel)
        // {
        //     _context.Card.Add(cardModel);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetCardModel", new { id = cardModel.Id }, cardModel);
        // }

        // // DELETE: api/Card/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteCardModel(long id)
        // {
        //     var cardModel = await _context.Card.FindAsync(id);
        //     if (cardModel == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Card.Remove(cardModel);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool CardModelExists(long id)
        {
            return _context.Card.Any(e => e.Id == id);
        }
    }
}
