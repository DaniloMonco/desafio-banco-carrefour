using System.Security.Claims;
using Opah.TransactionService.Application.Dtos;

namespace Opah.TransactionService.Api
{
    public static class EndpointsExtensions
    {
        extension(WebApplication app)
        {
            public void AddTransactionEndpoints()
            {
                app.MapPost("/transactions/credit", async (ClaimsPrincipal user, TransactionCreditDto dto, Application.Services.TransactionService service) =>
                {
                    dto.UserName = user.FindFirst("preferred_username")!.Value;
                    var transaction = await service.Create(dto);

                    return Results.Created($"/transactions/credit/{transaction.Id}", transaction);
                })
                .RequireAuthorization("RequireUserRole")
                .WithName("credit-transactions");


                app.MapPost("/transactions/debit", async (ClaimsPrincipal user, TransactionDebitDto dto, Application.Services.TransactionService service) =>
                {
                    dto.UserName = user.FindFirst("preferred_username")!.Value;
                    var transaction = await service.Create(dto);

                    return Results.Created($"/transactions/debit/{transaction.Id}", transaction);
                })
                .RequireAuthorization("RequireUserRole")
                .WithName("debit-transactions");

            }
        }
    }
}
