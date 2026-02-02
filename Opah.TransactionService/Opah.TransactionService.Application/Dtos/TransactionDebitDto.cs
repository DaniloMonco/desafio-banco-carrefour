namespace Opah.TransactionService.Application.Dtos
{
    public record TransactionDebitDto : TransactionDto
    {
        public TransactionDebitDto(decimal Amount) : base(Amount, TransactionTypeDto.Debit)
        {
        }
    }
}
