using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PacientesePlanos.Data;
using PacientesePlanos.Model;

namespace PacientesePlanos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ControllersPaciente : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ControllersPaciente(ApplicationDbContext context)
    {
        _context = context;
    }

    public class CreatePacienteDto
    {
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public List<int> PlanoIds { get; set; } = new List<int>();
    }

    public class UpdatePacienteDto
    {
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
    }

    public class PacienteDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public List<int> PlanoIds { get; set; } = new List<int>();
}

    // GET: api/pacientes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PacienteDto>>> GetPacientes()
    {
        var pacientes = await _context.Pacientes
            .Select(p => new PacienteDto
            {
                Id = p.Id,
                Nome = p.Nome,
                CPF = p.CPF,
                Telefone = p.Telefone,
                PlanoIds = p.Planos.Select(plano => plano.Id).ToList()
            })
            .ToListAsync();

        return Ok(pacientes);
    }

    // GET: api/pacientes/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PacienteDto>> GetPacienteById(int id)
    {
        var paciente = await _context.Pacientes
            .Include(p => p.Planos)
            .Select(p => new PacienteDto
            {
                Id = p.Id,
                Nome = p.Nome,
                CPF = p.CPF,
                Telefone = p.Telefone,
                PlanoIds = p.Planos.Select(plano => plano.Id).ToList()
            })
            .FirstOrDefaultAsync(p => p.Id == id);

        if (paciente == null)
        {
            return NotFound($"Paciente com ID {id} não encontrado.");
        }

        return Ok(paciente);
    }

    // PUT: api/pacientes/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePaciente(int id, UpdatePacienteDto dto)
    {
        var paciente = await _context.Pacientes.FindAsync(id);
        if (paciente == null)
        {
            return NotFound($"Paciente com ID {id} não encontrado.");
        }

        paciente.Nome = dto.Nome;
        paciente.CPF = dto.CPF;
        paciente.Telefone = dto.Telefone;

        _context.Entry(paciente).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/pacientes/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePaciente(int id)
    {
        var paciente = await _context.Pacientes.FindAsync(id);
        if (paciente == null)
        {
            return NotFound($"Paciente com ID {id} não encontrado.");
        }

        _context.Pacientes.Remove(paciente);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/pacientes
    [HttpPost]
    public async Task<IActionResult> CreatePaciente([FromBody] CreatePacienteDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var paciente = new Paciente
        {
            Nome = dto.Nome,
            CPF = dto.CPF,
            Telefone = dto.Telefone
        };

        // Adiciona planos ao paciente
        if (dto.PlanoIds != null && dto.PlanoIds.Any())
        {
            var planos = await _context.Planos
                .Where(p => dto.PlanoIds.Contains(p.Id))
                .ToListAsync();

            if (planos.Count != dto.PlanoIds.Count)
            {
                return BadRequest("Um ou mais IDs de plano são inválidos.");
            }

            foreach (var plano in planos)
            {
                paciente.Planos.Add(plano); // Supondo que Planos é um ICollection<Plano>
            }
        }

        _context.Pacientes.Add(paciente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPacienteById), new { id = paciente.Id }, paciente);
    }
}
