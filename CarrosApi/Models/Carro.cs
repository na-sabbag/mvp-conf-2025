using MessagePack;

namespace CarrosApi.Models;

[MessagePackObject]
public class Carro
{
    [Key(0)]
    public int Id { get; set; }
    
    [Key(1)]
    public string Marca { get; set; } = string.Empty;
    
    [Key(2)]
    public string Modelo { get; set; } = string.Empty;
    
    [Key(3)]
    public int Ano { get; set; }
    
    [Key(4)]
    public string Cor { get; set; } = string.Empty;
    
    [Key(5)]
    public decimal Preco { get; set; }
}



