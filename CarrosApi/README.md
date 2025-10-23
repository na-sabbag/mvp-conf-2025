# API de Carros

API simples em .NET 8.0 para gerenciar um cadastro de carros em memória.

## 🚀 Como Executar

1. Navegue até a pasta do projeto:
```bash
cd CarrosApi
```

2. Execute a aplicação:
```bash
dotnet run
```

3. Acesse o Swagger UI em: `https://localhost:5001/swagger`

## 📋 Endpoints Disponíveis

### GET /api/carros
Retorna todos os carros cadastrados.

**Resposta de exemplo:**
```json
[
  {
    "id": 1,
    "marca": "Toyota",
    "modelo": "Corolla",
    "ano": 2023,
    "cor": "Prata",
    "preco": 125000.00
  }
]
```

### GET /api/carros/{id}
Retorna um carro específico por ID.

**Exemplo:** `GET /api/carros/1`

**Resposta de exemplo:**
```json
{
  "id": 1,
  "marca": "Toyota",
  "modelo": "Corolla",
  "ano": 2023,
  "cor": "Prata",
  "preco": 125000.00
}
```

### POST /api/carros
Adiciona um novo carro ao cadastro.

**Body de exemplo:**
```json
{
  "marca": "Toyota",
  "modelo": "Corolla",
  "ano": 2023,
  "cor": "Prata",
  "preco": 125000.00
}
```

**Resposta de exemplo:**
```json
{
  "id": 1,
  "marca": "Toyota",
  "modelo": "Corolla",
  "ano": 2023,
  "cor": "Prata",
  "preco": 125000.00
}
```

## 📝 Modelo de Dados

```csharp
public class Carro
{
    public int Id { get; set; }           // Gerado automaticamente
    public string Marca { get; set; }     // Obrigatório
    public string Modelo { get; set; }    // Obrigatório
    public int Ano { get; set; }
    public string Cor { get; set; }
    public decimal Preco { get; set; }
}
```

## ⚠️ Importante

- Os dados são armazenados **apenas em memória**
- Ao reiniciar a aplicação, todos os registros serão perdidos
- O ID é gerado automaticamente de forma sequencial




