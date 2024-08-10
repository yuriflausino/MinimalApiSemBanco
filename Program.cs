var builder = WebApplication.CreateBuilder(args);

// Adicionando serviços necessários
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurando Swagger no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Lista em memória para armazenar os produtos
var produtos = new List<Produto>
{
    new Produto { Id = 1, Nome = "Produto A", Descricao = "Descricao do Produto A", Preco = 10.5m, QuantidadeEmEstoque = 100 },
    new Produto { Id = 2, Nome = "Produto B", Descricao = "Descricao do Produto B", Preco = 20.0m, QuantidadeEmEstoque = 50 },
    new Produto { Id = 3, Nome = "Produto C", Descricao = "Descricao do Produto C", Preco = 15.75m, QuantidadeEmEstoque = 30 },
};

// Endpoint para consultar todos os produtos
app.MapGet("/produtos", () =>
{
    return produtos;
});

// Endpoint para consultar um produto por Id
app.MapGet("/produtos/{id:int}", (int id) =>
{
    var produto = produtos.FirstOrDefault(p => p.Id == id);
    return produto is not null ? Results.Ok(produto) : Results.NotFound();
});

// Endpoint para cadastrar um novo produto
app.MapPost("/produtos", (Produto novoProduto) =>
{
    novoProduto.Id = produtos.Max(p => p.Id) + 1;
    produtos.Add(novoProduto);
    return Results.Created($"/produtos/{novoProduto.Id}", novoProduto);
});

// Endpoint para atualizar um produto existente
app.MapPut("/produtos/{id:int}", (int id, Produto produtoAtualizado) =>
{
    var produto = produtos.FirstOrDefault(p => p.Id == id);
    if (produto is null)
        return Results.NotFound();

    produto.Nome = produtoAtualizado.Nome;
    produto.Descricao = produtoAtualizado.Descricao;
    produto.Preco = produtoAtualizado.Preco;
    produto.QuantidadeEmEstoque = produtoAtualizado.QuantidadeEmEstoque;

    return Results.Ok(produto);
});

// Endpoint para excluir um produto
app.MapDelete("/produtos/{id:int}", (int id) =>
{
    var produto = produtos.FirstOrDefault(p => p.Id == id);
    if (produto is null)
        return Results.NotFound();

    produtos.Remove(produto);
    return Results.NoContent();
});

app.Run();
