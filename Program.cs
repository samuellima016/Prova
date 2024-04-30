using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();


var app = builder.Build();

// PRONTO
app.MapGet("api/funcionario/listar", ([FromServices] AppDbContext context) =>
{
    if (context.Funcionarios.Any())
    {
        return Results.Ok(context.Funcionarios.ToList());
    }
    return Results.NotFound("Não foi encontrado nenhum funcionário");

});

// PRONTO
app.MapGet("api/folha/listar", ([FromServices] AppDbContext context) =>
{
    if (context.Folhas.Any())
    {
        return Results.Ok(context.Folhas.ToList());
    }
    return Results.NotFound("Não foi encontrado nenhuma folha de pagamento");

});

// PRONTO
app.MapGet("api/folha/buscar/{cpf}/{mes}/{ano}", ([FromRoute] string cpf, int mes, int ano, [FromServices] AppDbContext context) =>
{
    Funcionario? funcionarioBuscado = context.Funcionarios.FirstOrDefault(x => x.Cpf == cpf);
    if (funcionarioBuscado is null) return Results.NotFound("Não existe nenhum funcionário existente com esse CPF!! Cadastre um CPF e depois vincule a folha com o CPF correto do funcionário");

    Folha? folhaEncontrada = context.Folhas.Where(f => f.Mes == mes).Where(f => f.Ano == ano).FirstOrDefault(x => x.FuncionarioId == funcionarioBuscado.Id);

    if (folhaEncontrada != null) Results.NotFound("Não foi encontrado nenhuma folha com o CPF deste funcionário com esta data, favor validar!");
    else
    {
        Results.Ok(folhaEncontrada);
    }
    return Results.NotFound();

});


app.MapPost("api/folha/cadastrar", ([FromServices] AppDbContext context,
                                    [FromBody] Folha folhaNova) =>
{
    if (folhaNova != null)
    {
        context.Folhas.Add(folhaNova);
        context.SaveChangesAsync();
        return Results.Created("Criado com sucesso!", folhaNova);
    }
    return Results.BadRequest("Falha ao registrar a folha!");
});



app.MapPost("api/funcionario/cadastrar", ([FromServices] AppDbContext context,
                                    [FromBody] Funcionario funcionarioNovo) =>
{
    if (funcionarioNovo != null)
    {
        context.Funcionarios.Add(funcionarioNovo);
        context.SaveChangesAsync();
        return Results.Created("Criado com sucesso!", funcionarioNovo);
    }
    return Results.BadRequest("Falha ao registrar o funcionário!");
});

app.Run();