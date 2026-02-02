using Opah.ReportService.Domain.Entities;
using Opah.ReportService.Domain.ValueObjects;
using Xunit;

namespace Opah.ReportService.Domain.Test
{
    public class ReportTests
    {
        [Fact]
        public void Build_Should_Group_Transactions_By_Date()
        {
            // Arrange
            var report = new Report(new ReferenceMonth(2025, 1));

            var transactions = new List<Transaction>
            {
                new Transaction(
                    Guid.NewGuid(),
                    new DateTime(2025, 1, 10, 10, 0, 0),
                    100m,
                    string.Empty,
                    TransactionType.D
                ),
                new Transaction(
                    Guid.NewGuid(),
                    new DateTime(2025, 1, 10, 18, 0, 0),
                    50m,
                    string.Empty,
                    TransactionType.C
                )
            };

            // Act
            report.Build(transactions);

            // Assert
            var item = Assert.Single(report.Items!);

            Assert.Equal(new DateOnly(2025, 1, 10), item.Date);
            Assert.Equal(100m, item.Debit);
            Assert.Equal(50m, item.Credit);
        }

        [Fact]
        public void Build_Should_Create_Items_For_Multiple_Days()
        {
            // Arrange
            var report = new Report(new ReferenceMonth(2025, 1));

            var transactions = new List<Transaction>
            {
                new Transaction(
                    Guid.NewGuid(),
                    new DateTime(2025, 1, 10),
                    100m,
                    string.Empty,
                    TransactionType.D                  
                ),
                new Transaction(
                    Guid.NewGuid(),
                    new DateTime(2025, 1, 11),
                    200m,
                    string.Empty,
                    TransactionType.C
                )
            };

            // Act
            report.Build(transactions);

            // Assert
            Assert.Equal(2, report.Items!.Count());
        }

        [Fact]
        public void Build_Should_Sum_Debit_And_Credit_Correctly()
        {
            // Arrange
            var report = new Report(new ReferenceMonth(2025, 1));

            var transactions = new List<Transaction>
            {
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 10), 100m, string.Empty, TransactionType.D),
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 10), 50m, string.Empty, TransactionType.D),
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 10), 30m, string.Empty, TransactionType.C)
            };

            // Act
            report.Build(transactions);

            // Assert
            var item = report.Items!.Single();

            Assert.Equal(150m, item.Debit);
            Assert.Equal(30m, item.Credit);
        }

        [Fact]
        public void Build_Should_Order_Items_By_Date()
        {
            // Arrange
            var report = new Report(new ReferenceMonth(2025, 1));

            var transactions = new List<Transaction>
            {
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 12), 10m, string.Empty, TransactionType.D),
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 10), 20m, string.Empty, TransactionType.C),
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 11), 30m, string.Empty, TransactionType.D)
            };

            // Act
            report.Build(transactions);

            // Assert
            var dates = report.Items!.Select(i => i.Date).ToList();

            Assert.Equal(
                new[]
                {
                    new DateOnly(2025, 1, 10),
                    new DateOnly(2025, 1, 11),
                    new DateOnly(2025, 1, 12)
                },
                dates
            );
        }

        [Fact]
        public void Build_Should_Set_Empty_Items_When_No_Transactions()
        {
            // Arrange
            var report = new Report(new ReferenceMonth(2025, 1));
            var transactions = new List<Transaction>();

            // Act
            report.Build(transactions);

            // Assert
            Assert.NotNull(report.Items);
            Assert.Empty(report.Items!);
        }
    }
}
