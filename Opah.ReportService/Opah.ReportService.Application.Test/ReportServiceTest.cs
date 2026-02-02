using NSubstitute;
using Opah.ReportService.Application.Dtos;
using Opah.ReportService.Domain.Entities;
using Opah.ReportService.Domain.Repositories;
using Opah.ReportService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportService.Application.Test
{
    public class ReportServiceTests
    {
        private readonly IReportRepository _repositoryMock;
        private readonly Services.ReportService _service;

        public ReportServiceTests()
        {
            _repositoryMock = Substitute.For<IReportRepository>();
            _service = new Services.ReportService(_repositoryMock);
        }

        [Fact]
        public async Task GetReport_Daily_Should_Return_ReportDailyDto()
        {
            // Arrange
            var @params = new ReportDailyParamsDto(2025, 1, 10);

            var transactions = new List<Transaction>
            {
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 10), 200m, string.Empty, TransactionType.C),
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 10), 50m, string.Empty, TransactionType.D)
            };

            _repositoryMock.GetTransactions(2025, 1, 10).Returns(transactions);

            // Act
            var result = await _service.GetReport(@params);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2025, result.Year);
            Assert.Equal(1, result.Month);
            Assert.Equal((ushort)10, result.Day);
            Assert.Equal(50m, result.Debit);
            Assert.Equal(200m, result.Credit);
            Assert.Equal(150m, result.Total);

            await _repositoryMock.Received(1)
                .GetTransactions(2025, 1, 10);
        }

        [Fact]
        public async Task GetReport_Monthly_Should_Return_ReportDto()
        {
            // Arrange
            var @params = new ReportParamsDto(2025, 1);

            var transactions = new List<Transaction>
            {
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 10), 100m, string.Empty, TransactionType.C),
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 10), 30m, string.Empty, TransactionType.D),
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 11), 50m, string.Empty, TransactionType.C )
            };

            _repositoryMock.GetTransactions(2025, 1).Returns(transactions);

            // Act
            var result = await _service.GetReport(@params);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2025, result.Year);
            Assert.Equal(1, result.Month);

            var items = result.Items!.ToList();
            Assert.Equal(2, items.Count);

            var day10 = items.First(i => i.Date.Day == 10);
            Assert.Equal(30m, day10.Debit);
            Assert.Equal(100m, day10.Credit);
            Assert.Equal(70m, day10.Total);

            var day11 = items.First(i => i.Date.Day == 11);
            Assert.Equal(0m, day11.Debit);
            Assert.Equal(50m, day11.Credit);
            Assert.Equal(50m, day11.Total);

            await _repositoryMock.Received(1)
                .GetTransactions(2025, 1);
        }
    }
}
