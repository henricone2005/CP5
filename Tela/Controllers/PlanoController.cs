

using Microsoft.AspNetCore.Mvc;
using PacientesePlanos.Data;
using PacientesePlanos.Model;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Tela.Controllers
{
    public class PlanoController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApplicationDbContext _context;

        public PlanoController(IHttpClientFactory clientFactory, ApplicationDbContext context)
        {
            _clientFactory = clientFactory;
            _context = context;
        }

        // Exibir todos os planos
        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync("planos");

            if (response.IsSuccessStatusCode)
            {
                var conteudo = await response.Content.ReadAsStringAsync();
                var planos = JsonConvert.DeserializeObject<List<Plano>>(conteudo);
                return View(planos);
            }

            return View(new List<Plano>());
        }

        // Detalhes de um plano específico
        public async Task<IActionResult> Detalhes(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync($"planos/{id}");

            if (response.IsSuccessStatusCode)
            {
                var conteudo = await response.Content.ReadAsStringAsync();
                var plano = JsonConvert.DeserializeObject<Plano>(conteudo);
                return View(plano);
            }

            return NotFound($"Plano com ID {id} não encontrado.");
        }

        // Formulário para criar um novo plano
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Plano plano)
        {
            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient("PacientesePlanos");
                var conteudo = new StringContent(JsonConvert.SerializeObject(plano));
                conteudo.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync("planos", conteudo);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(plano);
        }

        // Formulário para editar um plano existente
        public async Task<IActionResult> Editar(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync($"planos/{id}");

            if (response.IsSuccessStatusCode)
            {
                var conteudo = await response.Content.ReadAsStringAsync();
                var plano = JsonConvert.DeserializeObject<Plano>(conteudo);
                return View(plano);
            }

            return NotFound($"Plano com ID {id} não encontrado.");
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Plano plano)
        {
            if (id != plano.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient("PacientesePlanos");
                var conteudo = new StringContent(JsonConvert.SerializeObject(plano));
                conteudo.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PutAsync($"planos/{id}", conteudo);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(plano);
        }

        // Exibir confirmação para exclusão de um plano
        public async Task<IActionResult> Excluir(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.GetAsync($"planos/{id}");

            if (response.IsSuccessStatusCode)
            {
                var conteudo = await response.Content.ReadAsStringAsync();
                var plano = JsonConvert.DeserializeObject<Plano>(conteudo);
                return View(plano);
            }

            return NotFound($"Plano com ID {id} não encontrado.");
        }

        [HttpPost, ActionName("Excluir")]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            var client = _clientFactory.CreateClient("PacientesePlanos");
            var response = await client.DeleteAsync($"planos/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
