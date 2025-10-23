using CarrosApi.Models;

namespace CarrosApi.Services;

/// <summary>
/// Interface para o repositório de carros com serialização MessagePack
/// </summary>
public interface ICarroRepository : IDisposable
{
    /// <summary>
    /// Adiciona um novo carro e o serializa em formato binário
    /// </summary>
    Carro AdicionarCarro(Carro carro);
    
    /// <summary>
    /// Retorna todos os carros, desserializando do formato binário
    /// </summary>
    IEnumerable<Carro> ObterTodosCarros();
    
    /// <summary>
    /// Retorna um carro específico por ID, desserializando do formato binário
    /// </summary>
    Carro? ObterCarroPorId(int id);
    
    /// <summary>
    /// Retorna estatísticas sobre o armazenamento binário
    /// </summary>
    StorageStats ObterEstatisticas();
}

/// <summary>
/// Estatísticas sobre o armazenamento binário
/// </summary>
public class StorageStats
{
    public int TotalCarros { get; set; }
    public long TamanhoTotalBytes { get; set; }
    public long TamanhoMedioBytes { get; set; }
}

