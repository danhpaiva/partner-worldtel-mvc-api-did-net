using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Partner.WorldTel.Did.Api.Data;
using Partner.WorldTel.Did.Api.DTO;
using Partner.WorldTel.Did.Api.Interface;
using Partner.WorldTel.Did.Api.Models;

namespace Partner.WorldTel.Did.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternationalDidsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InternationalDidsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InternationalDid>>> GetInternationalDid()
        {
            return await _context.InternationalDid.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InternationalDid>> GetInternationalDid(int id)
        {
            var internationalDid = await _context.InternationalDid.FindAsync(id);

            if (internationalDid == null)
            {
                return NotFound();
            }

            return internationalDid;
        }

        [HttpPost("from-number")]
        public async Task<ActionResult<InternationalDid>> CreateFromNumber(
        [FromBody] CreateDidFromNumberRequest request,
        [FromServices] IDidGeneratorService didGenerator)
        {
            try
            {
                var did = await didGenerator.CreateFromE164NumberAsync(request.E164Number, request.CreatedBy ?? "api");
                return CreatedAtAction(nameof(GetInternationalDid), new { id = did.Id }, did);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInternationalDid(int id, InternationalDid internationalDid)
        {
            if (id != internationalDid.Id)
            {
                return BadRequest();
            }

            _context.Entry(internationalDid).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InternationalDidExists(id))
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

        [HttpPost]
        public async Task<ActionResult<InternationalDid>> PostInternationalDid(InternationalDid internationalDid)
        {
            _context.InternationalDid.Add(internationalDid);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInternationalDid", new { id = internationalDid.Id }, internationalDid);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInternationalDid(int id)
        {
            var internationalDid = await _context.InternationalDid.FindAsync(id);
            if (internationalDid == null)
            {
                return NotFound();
            }

            _context.InternationalDid.Remove(internationalDid);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InternationalDidExists(int id)
        {
            return _context.InternationalDid.Any(e => e.Id == id);
        }
    }
}
