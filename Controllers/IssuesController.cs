using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using regirapi.Models;

namespace regirapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IssuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Issues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Issue>>> GetIssues()
        {
            return await _context.Issues
               //      .Include(i => i.Project)  // Carrega la relació amb Project
               //     .Include(i => i.User)     // Carrega la relació amb User
                   .ToListAsync();
        }

        // GET: api/Issues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Issue>> GetIssue(int id)
        {
            //var issue = await _context.Issues.FindAsync(id);
            var issue = await _context.Issues
                    .Include(i => i.Project)  // Carrega la relació amb Project
                    .Include(i => i.User)     // Carrega la relació amb User
                    .FirstOrDefaultAsync(i => i.Id == id);

            if (issue == null)
            {
                return NotFound();
            }

            return issue;
        }

        // PUT: api/Issues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIssue(int id, Issue issue)
        {
            if (id != issue.Id)
            {
                return BadRequest();
            }

            _context.Entry(issue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssueExists(id))
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



        // POST: api/Issues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*
        [HttpPost]
        public async Task<ActionResult<Issue>> PostIssue(Issue issue)
        {
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIssue", new { id = issue.Id }, issue);
        }
        */



        // Endpoint per crear una nova incidència amb opcionalment un projecte i usuari associats
        [HttpPost]
        public async Task<IActionResult> CreateIssue([FromBody] CreateIssueDto issueDto)
        {
            var issue = new Issue
            {
                Name = issueDto.Name,
                Description = issueDto.Description,
                Type = issueDto.Type,
                Priority = issueDto.Priority,
                Status = issueDto.Status,
                CreatedAt = DateTime.UtcNow
            };

            // Si s'ha proporcionat ProjectId, assigna el projecte
            if (issueDto.ProjectId.HasValue)
            {
                var project = await _context.Projects.FindAsync(issueDto.ProjectId.Value);
                if (project == null)
                {
                    return BadRequest("Projecte no trobat.");
                }
                issue.ProjectId = project.Id;
            }

            // Si s'ha proporcionat UserId, assigna l'usuari
            if (issueDto.UserId.HasValue)
            {
                var user = await _context.Users.FindAsync(issueDto.UserId.Value);
                if (user == null)
                {
                    return BadRequest("Usuari no trobat.");
                }
                issue.UserId = user.Id;
            }

            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIssue", new { id = issue.Id }, issue);
        }




        // DELETE: api/Issues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIssue(int id)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }

            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IssueExists(int id)
        {
            return _context.Issues.Any(e => e.Id == id);
        }



        // Endpoint per canviar l'usuari assignat a una incidència
        [HttpPut("{id}/change-user")]
        public async Task<IActionResult> ChangeIssueUser(int id, [FromBody] ChangeIssueUserDto dto)
        {
            // Busca la Issue per ID
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null)
            {
                return NotFound("La incidència no existeix.");
            }

            // Comprova que l'usuari nou existeixi
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
            {
                return NotFound("L'usuari especificat no existeix.");
            }

            // Actualitza l'assignació de la Issue
            issue.UserId = dto.UserId;
            await _context.SaveChangesAsync();

            return Ok($"Usuari assignat a la incidència actualitzat a {user.Name}.");
        }
        
    }
}
