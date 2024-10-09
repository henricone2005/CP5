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
       public string Nome { get; set; } = string.Empty; // Inicializa como vazio
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    }

    public class UpdatePacienteDto
    {
public string Nome { get; set; } = string.Empty; // Inicializa como vazio
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    }

    // POST: api/pacientes
    [HttpPost]
    public async Task<ActionResult<Paciente>> CreatePaciente(CreatePacienteDto dto)
    {
        var paciente = new Paciente
        {
            Nome = dto.Nome,
            CPF = dto.CPF,
            Telefone = dto.Telefone
        };

        _context.Pacientes.Add(paciente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPacienteById), new { id = paciente.Id }, paciente);
    }

    // GET: api/pacientes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Paciente>>> GetPacientes()
    {
        return await _context.Pacientes.ToListAsync();
    }

    // GET: api/pacientes/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Paciente>> GetPacienteById(int id)
    {
        var paciente = await _context.Pacientes
            .Include(p => p.Planos) // Inclui os planos associados
            .FirstOrDefaultAsync(p => p.Id == id);

        if (paciente == null)
        {
            return NotFound($"Paciente com ID {id} não encontrado.");
        }

        return paciente;
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
}
