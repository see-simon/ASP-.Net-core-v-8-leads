using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartFibreAPI.Data;
using SmartFibreAPI.Models;
using SmartFibreAPI.Services;

namespace SmartFibreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public LeadsController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: api/leads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lead>>> GetLeads()
        {
            return await _context.Leads.OrderByDescending(l => l.CreatedAt).ToListAsync();
        }

        // POST: api/leads
        [HttpPost]
        public async Task<ActionResult<Lead>> CreateLead(Lead lead)
        {
            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();

            try
            {
                await _emailService.SendLeadEmailAsync(
                    lead.Name,
                    lead.Surname,
                    lead.Email,
                    lead.PhoneNumber,
                    lead.Description
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetLeads), new { id = lead.Id }, lead);
        }
    }
}