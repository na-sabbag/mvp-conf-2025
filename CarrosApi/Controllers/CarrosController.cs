using Microsoft.AspNetCore.Mvc;
using CarrosApi.Models;
using CarrosApi.Services;

namespace CarrosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarrosController : ControllerBase
{
    private readonly ICarroRepository _repository;

    public CarrosController(ICarroRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Retorna todos os carros cadastrados
    /// Os dados são desserializados do formato binário MessagePack
    /// </summary>
    [HttpGet]
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
    public async Task<ActionResult<IEnumerable<Carro>>> GetCarros()
    {
        // Executa a operação em uma thread pool para não bloquear a thread de requisição
        var carros = await Task.Run(() => _repository.ObterTodosCarros());
        return Ok(carros);
    }

    /// <summary>
    /// Retorna um carro específico por ID
    /// Os dados são desserializados do formato binário MessagePack
    /// </summary>
    [HttpGet("{id}")]
    [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "id" })]
    public async Task<ActionResult<Carro>> GetCarro(int id)
    {
        var carro = await Task.Run(() => _repository.ObterCarroPorId(id));
        
        if (carro == null)
        {
            return NotFound(new { mensagem = $"Carro com ID {id} não encontrado." });
        }
        
        return Ok(carro);
    }

    /// <summary>
    /// Adiciona um novo carro
    /// Os dados recebidos são serializados para formato binário MessagePack antes do armazenamento
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Carro>> PostCarro([FromBody] Carro novoCarro)
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

        // Adiciona o carro ao repositório (será serializado em formato binário MessagePack)
        var carroAdicionado = await Task.Run(() => _repository.AdicionarCarro(novoCarro));
        
        // Retorna o carro criado com status 201 Created
        return CreatedAtAction(nameof(GetCarro), new { id = carroAdicionado.Id }, carroAdicionado);
    }

    /// <summary>
    /// Retorna estatísticas sobre o armazenamento binário MessagePack
    /// </summary>
    [HttpGet("stats")]
    [ResponseCache(Duration = 5, Location = ResponseCacheLocation.Any)]
    public async Task<ActionResult<StorageStats>> GetStats()
    {
        var stats = await Task.Run(() => _repository.ObterEstatisticas());
        return Ok(stats);
    }
}



