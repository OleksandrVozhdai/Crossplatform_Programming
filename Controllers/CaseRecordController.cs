using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using disease_outbreaks_detector.Models;

namespace disease_outbreaks_detector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseRecordController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CaseRecordController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CaseRecord
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaseRecord>>> GetCaseRecords()
        {
            return await _context.CaseRecords.ToListAsync();
        }

        // GET: api/CaseRecord/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CaseRecord>> GetCaseRecord(int id)
        {
            var caseRecord = await _context.CaseRecords.FindAsync(id);

            if (caseRecord == null)
            {
                return NotFound();
            }

            return caseRecord;
        }

        // PUT: api/CaseRecord/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCaseRecord(int id, CaseRecord caseRecord)
        {
            if (id != caseRecord.Id)
            {
                return BadRequest();
            }

            _context.Entry(caseRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CaseRecordExists(id))
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

        // POST: api/CaseRecord
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CaseRecord>> PostCaseRecord(CaseRecord caseRecord)
        {
            _context.CaseRecords.Add(caseRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCaseRecord", new { id = caseRecord.Id }, caseRecord);
        }

        // DELETE: api/CaseRecord/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCaseRecord(int id)
        {
            var caseRecord = await _context.CaseRecords.FindAsync(id);
            if (caseRecord == null)
            {
                return NotFound();
            }

            _context.CaseRecords.Remove(caseRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CaseRecordExists(int id)
        {
            return _context.CaseRecords.Any(e => e.Id == id);
        }
    }
}
