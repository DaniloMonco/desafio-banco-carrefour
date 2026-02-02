using Opah.ReportService.Domain.Entities;
using Opah.ReportService.Domain.ValueObjects;
using Opah.ReportService.Application.Mappers;
using Xunit;

namespace Opah.ReportService.Application.Test
{
    public class ReportMapperTests
    {
        [Fact]
        public void ToReportDto_Should_Map_Report_Correctly()
        {
            // Arrange
            var report = new Report(new ReferenceMonth(2025, 1));
            report.Build(new List<Transaction>
            {
                new Transaction(Guid.NewGuid(), new DateTime(2025,1,10), 100m, string.Empty, TransactionType.D),
                new Transaction(Guid.NewGuid(), new DateTime(2025,1,10), 250m, string.Empty, TransactionType.C),
            });

            // Act
            var dto = report.ToReportDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(2025, dto!.Year);
            Assert.Equal(1, dto.Month);

            var item = Assert.Single(dto.Items!);
            Assert.Equal(new DateOnly(2025, 1, 10), item.Date);
            Assert.Equal(100m, item.Debit);
            Assert.Equal(250m, item.Credit);
            Assert.Equal(150m, item.Total);
        }

        [Fact]
        public void ToReportDto_Should_Allow_Items_Null()
        {
            // Arrange
            var report = new Report(new ReferenceMonth(2025, 1))
            { 
            };

            // Act
            var dto = report.ToReportDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Null(dto!.Items);
        }

        [Fact]
        public void ToReportDailyDto_Should_Return_Null_When_Items_Is_Null()
        {
            // Arrange
            var report = new Report(new ReferenceMonth(2025, 1))
            {
            };

            // Act
            var dto = report.ToReportDailyDto();

            // Assert
            Assert.Null(dto);
        }

        [Fact]
        public void ToReportDailyDto_Should_Map_First_Item()
        {
            // Arrange
            var report = new Report(new ReferenceMonth(2025, 1));
            
            report.Build(new List<Transaction>
            {
                new Transaction(Guid.NewGuid(), new DateTime(2025,1,10), 100m, string.Empty, TransactionType.D),
                new Transaction(Guid.NewGuid(), new DateTime(2025,1,10), 200m, string.Empty, TransactionType.C),
                new Transaction(Guid.NewGuid(), new DateTime(2025,1,10), 50m, string.Empty, TransactionType.D),
                new Transaction(Guid.NewGuid(), new DateTime(2025,1,10), 80m, string.Empty, TransactionType.C),
            });

            // Act
            var dto = report.ToReportDailyDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(2025, dto!.Year);
            Assert.Equal(1, dto.Month);
            Assert.Equal((ushort)10, dto.Day);
            Assert.Equal(150m, dto.Debit);
            Assert.Equal(280m, dto.Credit);
            Assert.Equal(130m, dto.Total);
        }

        [Fact]
        public void ToReportDailyDto_Should_Calculate_Total_Correctly()
        {
            // Arrange
            var report = new Report(new ReferenceMonth(2025, 2));
            report.Build(new List<Transaction>
            {
                new Transaction(Guid.NewGuid(), new DateTime(2025,2,5), 300m, string.Empty, TransactionType.D),
                new Transaction(Guid.NewGuid(), new DateTime(2025,2,5), 120m, string.Empty, TransactionType.C),
            });

            // Act
            var dto = report.ToReportDailyDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(-180m, dto!.Total);
        }
    }
}
