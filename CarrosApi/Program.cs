using CarrosApi.Services;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Otimizações de serialização JSON
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
        options.JsonSerializerOptions.WriteIndented = false; // Reduz tamanho da resposta
    });

builder.Services.AddEndpointsApiExplorer();

// Configuração de Response Compression para melhor performance de rede
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest; // Fastest para melhor performance
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

// Configuração de Response Caching
builder.Services.AddResponseCaching();

// Registra o repositório como Singleton para manter dados em memória
// O repositório usa MessagePack para serialização/desserialização binária
builder.Services.AddSingleton<ICarroRepository, CarroRepository>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Carros API com MessagePack", 
        Version = "v1",
        Description = "API para gerenciar cadastro de carros com serialização/desserialização MessagePack. " +
                      "Os dados são armazenados em formato binário para otimização de performance."
    });
});

var app = builder.Build();

// Configure o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Carros API v1");
    });
}

// Response Compression deve vir antes de Response Caching
app.UseResponseCompression();

// Response Caching para melhorar performance
app.UseResponseCaching();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();



