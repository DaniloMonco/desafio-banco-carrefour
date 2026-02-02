namespace Opah.TransactionService.Application.Dtos
{
    public record TransactionCreditDto : TransactionDto
    {
        public TransactionCreditDto(decimal Amount) : base(Amount, TransactionTypeDto.Credit)
        {
        }
    }
}
