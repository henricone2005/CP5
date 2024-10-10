using Microsoft.AspNetCore.Mvc;
using PacientesePlanos.Data;
using PacientesePlanos.Model;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Tela.Controllers  // Verifique se esta linha está correta
{
    public class PacienteController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApplicationDbContext _context;

        public PacienteController(IHttpClientFactory clientFactory, ApplicationDbContext context)
        {
            _clientFactory = clientFactory;
            _context = context;
        }

        // Exibir todos os pacientes
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync("pacientes");

            if (response.IsSuccessStatusCode)
            {
                var conteudo = await response.Content.ReadAsStringAsync();
                var pacientes = JsonConvert.DeserializeObject<List<Paciente>>(conteudo);
                return View(pacientes);
            }

            return View(new List<Paciente>());
        }

        // Detalhes de um paciente específico
        public async Task<IActionResult> Detalhes(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync($"pacientes/{id}");

            if (response.IsSuccessStatusCode)
            {
                var conteudo = await response.Content.ReadAsStringAsync();
                var paciente = JsonConvert.DeserializeObject<Paciente>(conteudo);
                return View(paciente);
            }

            return NotFound($"Paciente com ID {id} não encontrado.");
        }

        // Formulário para criar um novo paciente
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient("PacientesePlanos");
                var conteudo = new StringContent(JsonConvert.SerializeObject(paciente));
                conteudo.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync("pacientes", conteudo);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(paciente);
        }

        // Formulário para editar um paciente existente
        public async Task<IActionResult> Editar(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync($"pacientes/{id}");

            if (response.IsSuccessStatusCode)
            {
                var conteudo = await response.Content.ReadAsStringAsync();
                var paciente = JsonConvert.DeserializeObject<Paciente>(conteudo);
                return View(paciente);
            }

            return NotFound($"Paciente com ID {id} não encontrado.");
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient("PacientesePlanos");
                var conteudo = new StringContent(JsonConvert.SerializeObject(paciente));
                conteudo.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PutAsync($"pacientes/{id}", conteudo);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(paciente);
        }

        // Exibir confirmação para exclusão de um paciente
        public async Task<IActionResult> Excluir(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync($"pacientes/{id}");

            if (response.IsSuccessStatusCode)
            {
                var conteudo = await response.Content.ReadAsStringAsync();
                var paciente = JsonConvert.DeserializeObject<Paciente>(conteudo);
                return View(paciente);
            }

            return NotFound($"Paciente com ID {id} não encontrado.");
        }

        [HttpPost, ActionName("Excluir")]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.DeleteAsync($"pacientes/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
