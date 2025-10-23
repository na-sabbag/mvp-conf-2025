using Microsoft.AspNetCore.Mvc;
using CarrosApi.Models;

namespace CarrosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarrosController : ControllerBase
{
    // Lista estática para armazenar os carros em memória
    private static List<Carro> carros = new List<Carro>();
    private static int proximoId = 1;

    /// <summary>
    /// Retorna todos os carros cadastrados
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<Carro>> GetCarros()
    {
        return Ok(carros);
    }

    /// <summary>
    /// Retorna um carro específico por ID
    /// </summary>
    [HttpGet("{id}")]
    public ActionResult<Carro> GetCarro(int id)
    {
        var carro = carros.FirstOrDefault(c => c.Id == id);
        
        if (carro == null)
        {
            return NotFound(new { mensagem = $"Carro com ID {id} não encontrado." });
        }
        
        return Ok(carro);
    }

    /// <summary>
    /// Adiciona um novo carro
    /// </summary>
    [HttpPost]
    public ActionResult<Carro> PostCarro([FromBody] Carro novoCarro)
    {
        if (novoCarro == null)
        {
            return BadRequest(new { mensagem = "Dados do carro inválidos." });
        }

        // Validações básicas
        if (string.IsNullOrWhiteSpace(novoCarro.Marca) || 
            string.IsNullOrWhiteSpace(novoCarro.Modelo))
        {
            return BadRequest(new { mensagem = "Marca e Modelo são obrigatórios." });
        }

        if (novoCarro.Ano < 1886 || novoCarro.Ano > DateTime.Now.Year + 1)
        {
            return BadRequest(new { mensagem = "Ano inválido." });
        }

        // Atribui um ID automático
        novoCarro.Id = proximoId++;
        
        // Adiciona o carro à lista
        carros.Add(novoCarro);
        
        // Retorna o carro criado com status 201 Created
        return CreatedAtAction(nameof(GetCarro), new { id = novoCarro.Id }, novoCarro);
    }
}



