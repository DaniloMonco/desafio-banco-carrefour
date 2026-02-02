using Opah.ReportService.Domain.Entities;
using Opah.ReportService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportService.Domain.Test
{
    public class ReportItemTests
    {
        [Fact]
        public void Constructor_Should_Set_Initial_Values()
        {
            // Arrange
            var date = new DateOnly(2025, 1, 10);

            // Act
            var item = new ReportItem(date, 100m, 250m);

            // Assert
            Assert.Equal(date, item.Date);
            Assert.Equal(100m, item.Debit);
            Assert.Equal(250m, item.Credit);
            Assert.Equal(150m, item.Total);
        }

        [Fact]
        public void Constructor_With_Date_Only_Should_Set_Zero_Values()
        {
            // Arrange
            var date = new DateOnly(2025, 1, 10);

            // Act
            var item = new ReportItem(date);

            // Assert
            Assert.Equal(date, item.Date);
            Assert.Equal(0m, item.Debit);
            Assert.Equal(0m, item.Credit);
            Assert.Equal(0m, item.Total);
        }

        [Fact]
        public void AdicionarValor_Should_Add_Credit_When_Type_Is_Credit()
        {
            // Arrange
            var item = new ReportItem(new DateOnly(2025, 1, 10));

            // Act
            item.AdicionarValor(TransactionType.C, 200m);

            // Assert
            Assert.Equal(200m, item.Credit);
            Assert.Equal(0m, item.Debit);
            Assert.Equal(200m, item.Total);
        }

        [Fact]
        public void AdicionarValor_Should_Add_Debit_When_Type_Is_Debit()
        {
            // Arrange
            var item = new ReportItem(new DateOnly(2025, 1, 10));

            // Act
            item.AdicionarValor(TransactionType.D, 80m);

            // Assert
            Assert.Equal(80m, item.Debit);
            Assert.Equal(0m, item.Credit);
            Assert.Equal(-80m, item.Total);
        }

        [Fact]
        public void AdicionarValor_Should_Accumulate_Values()
        {
            // Arrange
            var item = new ReportItem(new DateOnly(2025, 1, 10));

            // Act
            item.AdicionarValor(TransactionType.C, 100m);
            item.AdicionarValor(TransactionType.C, 50m);
            item.AdicionarValor(TransactionType.D, 30m);

            // Assert
            Assert.Equal(150m, item.Credit);
            Assert.Equal(30m, item.Debit);
            Assert.Equal(120m, item.Total);
        }
    }
}
