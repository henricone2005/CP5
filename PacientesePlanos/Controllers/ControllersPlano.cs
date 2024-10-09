using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PacientesePlanos.Data;
using PacientesePlanos.Model;

namespace PacientesePlanos.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ControllersPlano : ControllerBase
{
private readonly ApplicationDbContext _context;

    public ControllersPlano(ApplicationDbContext context)
    {
        _context = context;
    }

    public class CreatePlanoDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Cobertura { get; set; } = string.Empty;
    }

    public class UpdatePlanoDto
    {
        public string Nome { get; set; } = string.Empty; 
        public string Codigo { get; set; } = string.Empty;
        public string Cobertura { get; set; } = string.Empty;
    }

    // POST: api/planos
    [HttpPost]
    public async Task<ActionResult<Plano>> CreatePlano(CreatePlanoDto dto)
    {
        var plano = new Plano
        {
            Nome = dto.Nome,
            Codigo = dto.Codigo,
            Cobertura = dto.Cobertura
        };

        _context.Planos.Add(plano);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPlanoById), new { id = plano.Id }, plano);
    }

    // GET: api/planos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Plano>>> GetPlanos()
    {
        return await _context.Planos.ToListAsync();
    }

    // GET: api/planos/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Plano>> GetPlanoById(int id)
    {
        var plano = await _context.Planos
            .Include(p => p.Pacientes) // Inclui os pacientes associados
            .FirstOrDefaultAsync(p => p.Id == id);

        if (plano == null)
        {
            return NotFound($"Plano com ID {id} não encontrado.");
        }

        return plano;
    }

    // PUT: api/planos/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlano(int id, UpdatePlanoDto dto)
    {
        var plano = await _context.Planos.FindAsync(id);
        if (plano == null)
        {
            return NotFound($"Plano com ID {id} não encontrado.");
        }

        plano.Nome = dto.Nome;
        plano.Codigo = dto.Codigo;
        plano.Cobertura = dto.Cobertura;

        _context.Entry(plano).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/planos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlano(int id)
    {
        var plano = await _context.Planos.FindAsync(id);
        if (plano == null)
        {
            return NotFound($"Plano com ID {id} não encontrado.");
        }

        _context.Planos.Remove(plano);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
