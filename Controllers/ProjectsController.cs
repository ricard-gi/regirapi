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
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.Id }, project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }



        [HttpGet("{id}/issues-grouped")]
        public async Task<ActionResult<ProjectWithGroupedIssuesDto>> GetProjectWithGroupedIssues(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Issues)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            // Agrupem les issues per prioritat i les assignem al DTO
            var projectDto = new ProjectWithGroupedIssuesDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                HighPriorityIssues = project.Issues
                    .Where(i => i.Priority == 1)
                    .Select(i => new IssueDto
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Description = i.Description,
                        Type = i.Type,
                        Status = i.Status
                    }).ToList(),
                MediumPriorityIssues = project.Issues
                    .Where(i => i.Priority == 2)
                    .Select(i => new IssueDto
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Description = i.Description,
                        Type = i.Type,
                        Status = i.Status
                    }).ToList(),
                LowPriorityIssues = project.Issues
                    .Where(i => i.Priority == 3)
                    .Select(i => new IssueDto
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Description = i.Description,
                        Type = i.Type,
                        Status = i.Status
                    }).ToList(),
            };

            return projectDto;
        }
        
    }
}
