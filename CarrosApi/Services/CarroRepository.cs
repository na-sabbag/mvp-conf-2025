using CarrosApi.Models;
using MessagePack;

namespace CarrosApi.Services;

/// <summary>
/// Repositório que armazena carros em formato binário usando MessagePack
/// </summary>
public class CarroRepository : ICarroRepository
{
    // Armazena os dados em formato binário MessagePack
    private readonly Dictionary<int, byte[]> _carrosSerializados = new();
    private int _proximoId = 1;
    
    // ReaderWriterLockSlim permite múltiplas leituras simultâneas, melhorando performance
    private readonly ReaderWriterLockSlim _lock = new();

    /// <summary>
    /// Adiciona um novo carro, serializando-o para formato binário MessagePack
    /// </summary>
    public Carro AdicionarCarro(Carro carro)
    {
        _lock.EnterWriteLock();
        try
        {
            // Atribui um ID automático
            carro.Id = _proximoId++;
            
            // Serializa o objeto para formato binário MessagePack
            byte[] dadosSerializados = MessagePackSerializer.Serialize(carro);
            
            // Armazena em formato binário
            _carrosSerializados[carro.Id] = dadosSerializados;
            
            return carro;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Retorna todos os carros, desserializando do formato binário
    /// </summary>
    public IEnumerable<Carro> ObterTodosCarros()
    {
        _lock.EnterReadLock();
        try
        {
            // Usa LINQ Select para melhor performance e menos alocações
            return _carrosSerializados.Values
                .Select(dadosSerializados => MessagePackSerializer.Deserialize<Carro>(dadosSerializados))
                .ToList(); // ToList() para materializar antes de liberar o lock
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Retorna um carro específico por ID, desserializando do formato binário
    /// </summary>
    public Carro? ObterCarroPorId(int id)
    {
        _lock.EnterReadLock();
        try
        {
            if (!_carrosSerializados.TryGetValue(id, out byte[]? dadosSerializados))
            {
                return null;
            }
            
            // Desserializa o carro do formato binário MessagePack
            return MessagePackSerializer.Deserialize<Carro>(dadosSerializados);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <summary>
    /// Retorna estatísticas sobre o armazenamento binário
    /// </summary>
    public StorageStats ObterEstatisticas()
    {
        _lock.EnterReadLock();
        try
        {
            var totalCarros = _carrosSerializados.Count;
            var tamanhoTotal = _carrosSerializados.Values.Sum(bytes => bytes.Length);
            var tamanhoMedio = totalCarros > 0 ? tamanhoTotal / totalCarros : 0;

            return new StorageStats
            {
                TotalCarros = totalCarros,
                TamanhoTotalBytes = tamanhoTotal,
                TamanhoMedioBytes = tamanhoMedio
            };
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }
    
    // Implementa IDisposable para liberar o ReaderWriterLockSlim
    public void Dispose()
    {
        _lock?.Dispose();
    }
}

