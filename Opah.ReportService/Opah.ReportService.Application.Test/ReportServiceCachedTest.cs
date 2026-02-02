using NSubstitute;
using Opah.ReportService.Application.Cached;
using Opah.ReportService.Application.Dtos;
using Opah.ReportService.Application.Services;
using Opah.ReportService.Domain.Entities;
using Opah.ReportService.Domain.Repositories;
using Opah.ReportService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportService.Application.Test
{
    public class ReportServiceCachedTests
    {
        private readonly IReportRepository _repository;
        private readonly IReportCached _cache;
        private readonly ReportServiceCached _service;

        public ReportServiceCachedTests()
        {
            _repository = Substitute.For<IReportRepository>();
            _cache = Substitute.For<IReportCached>();

            _service = new ReportServiceCached(_repository, _cache);
        }

        [Fact]
        public async Task GetReport_Daily_Should_Return_From_Cache_When_Cache_Hit()
        {
            // Arrange
            var @params = new ReportDailyParamsDto(2025, 1, 10);

            var cachedDto = new ReportDailyDto(
                2025, 1, 10,
                50m,
                200m,
                150m
            );

            var cacheKey = "GetReport-2025-1-10";

            _cache.GetAsync<ReportDailyDto>(cacheKey)
                  .Returns(cachedDto);

            // Act
            var result = await _service.GetReport(@params);

            // Assert
            Assert.Same(cachedDto, result);

            await _cache.DidNotReceive()
                .SetAsync(
                    Arg.Any<string>(),
                    Arg.Any<ReportDailyDto>(),
                    Arg.Any<TimeSpan>()
                );
        }

        [Fact]
        public async Task GetReport_Daily_Should_Fetch_From_Repository_And_Set_Cache_When_Cache_Miss()
        {
            // Arrange
            var @params = new ReportDailyParamsDto(2025, 1, 10);

            var transactions = new List<Transaction>
            {
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 10), 200m, string.Empty, TransactionType.C),
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 10), 50m, string.Empty, TransactionType.D)
            };

            var cacheKey = "GetReport-2025-1-10";

            _cache.GetAsync<ReportDailyDto>(cacheKey).Returns((ReportDailyDto?)null);

            _repository.GetTransactions(2025, 1, 10).Returns(transactions);

            // Act
            var result = await _service.GetReport(@params);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(150m, result.Total);

            await _repository.Received(1)
                .GetTransactions(2025, 1, 10);

            await _cache.Received(1)
                .SetAsync(
                    cacheKey,
                    Arg.Any<ReportDailyDto>(),
                    TimeSpan.FromMinutes(5)
                );
        }

        [Fact]
        public async Task GetReport_Monthly_Should_Return_From_Cache_When_Cache_Hit()
        {
            // Arrange
            var @params = new ReportParamsDto(2025, 1);

            var cachedDto = new ReportDto(
                2025, 1,
                Items: new[]
                {
                    new ReportItemDto(
                        new DateOnly(2025, 1, 10),
                        30m, 100m, 70m)
                }
            );

            var cacheKey = "GetReport-2025-1";

            _cache.GetAsync<ReportDto>(cacheKey)
                  .Returns(cachedDto);

            // Act
            var result = await _service.GetReport(@params);

            // Assert
            Assert.Same(cachedDto, result);

            await _cache.DidNotReceive()
                .SetAsync(
                    Arg.Any<string>(),
                    Arg.Any<ReportDto>(),
                    Arg.Any<TimeSpan>()
                );
        }

        [Fact]
        public async Task GetReport_Monthly_Should_Fetch_From_Repository_And_Set_Cache_When_Cache_Miss()
        {
            // Arrange
            var @params = new ReportParamsDto(2025, 1);

            var transactions = new List<Transaction>
            {
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 10), 100m, string.Empty, TransactionType.C),
                new Transaction(Guid.NewGuid(), new DateTime(2025, 1, 10), 30m, string.Empty, TransactionType.D)
            };

            var cacheKey = "GetReport-2025-1";

            _cache.GetAsync<ReportDto>(cacheKey)
                  .Returns((ReportDto?)null);

            _repository.GetTransactions(2025, 1)
                       .Returns(transactions);

            // Act
            var result = await _service.GetReport(@params);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items!);

            await _repository.Received(1)
                .GetTransactions(2025, 1);

            await _cache.Received(1)
                .SetAsync(
                    cacheKey,
                    Arg.Any<ReportDto>(),
                    TimeSpan.FromHours(1)
                );
        }
    }
}
