using FluxoCaixa.Application.Services;
using FluxoCaixa.Domain.Model;
using NSubstitute;

namespace FluxoCaixa.Application.Test
{
    public class FluxoCaiApplicationServiceTest
    {
        private const int _ano = 2024;
        private const int _mes = 8;
        private const int _dia10 = 10;
        private const int _dia11 = 11;

        private Domain.DomainService.IFluxoCaixaService _domainService;

        public FluxoCaiApplicationServiceTest()
        {
            _domainService = Substitute.For<Domain.DomainService.IFluxoCaixaService>();
        }

        private Domain.Model.FluxoCaixa FluxoCaixaDiarioMock()
        {
            var dataHora = new DateTime(_ano, _mes, _dia10);
            var lancamentos = new List<Lancamento>
            {
                new Lancamento{DataHora=dataHora, Descricao = "Debito 1", TipoLancamento = TipoLancamento.D, Valor=10},
                new Lancamento{DataHora=dataHora, Descricao = "Debito 2", TipoLancamento = TipoLancamento.D, Valor=20},
                new Lancamento{DataHora=dataHora, Descricao = "Credito 1", TipoLancamento = TipoLancamento.C, Valor=20},
            };
            var fluxoCaixa = Domain.Model.FluxoCaixa.Criar(_ano, _mes);
            fluxoCaixa.MontarFluxoCaixa(lancamentos);
            return fluxoCaixa;
        }

        private Domain.Model.FluxoCaixa FluxoCaixaMensalMock()
        {
            var dataHora10 = new DateTime(_ano, _mes, _dia10);
            var dataHora11 = new DateTime(_ano, _mes, _dia11);
            var lancamentos = new List<Lancamento>
            {
                new Lancamento{DataHora=dataHora10, Descricao = "Debito 1", TipoLancamento = TipoLancamento.D, Valor=10},
                new Lancamento{DataHora=dataHora10, Descricao = "Debito 2", TipoLancamento = TipoLancamento.D, Valor=20},
                new Lancamento{DataHora=dataHora10, Descricao = "Credito 1", TipoLancamento = TipoLancamento.C, Valor=20},
                new Lancamento{DataHora=dataHora11, Descricao = "Credito 2", TipoLancamento = TipoLancamento.C, Valor=100},
                new Lancamento{DataHora=dataHora11, Descricao = "Debito 3", TipoLancamento = TipoLancamento.D, Valor=50},
            };
            var fluxoCaixa = Domain.Model.FluxoCaixa.Criar(_ano, _mes);
            fluxoCaixa.MontarFluxoCaixa(lancamentos);
            return fluxoCaixa;
        }

        [Fact]
        public async Task Retorna_FluxoCaixa_Com_Parametros_Dia_Mes_Ano()
        {
            _domainService.RecuperarFluxoCaixa(_ano, _mes, _dia10).Returns(Task.FromResult(FluxoCaixaDiarioMock()));
            var service = new FluxoCaixaService(_domainService);
            var result = await service.RecuperarFluxoCaixa(_ano, _mes, _dia10);

            Assert.Single(result);

            var dto = result.First();
            Assert.Equal(new DateOnly(_ano, _mes, _dia10), dto.Data);
            Assert.Equal(30, dto.Debito);
            Assert.Equal(20, dto.Credito);
            Assert.Equal(-10, dto.Saldo);
        }

        [Fact]
        public async Task Retorna_FluxoCaixa_Com_Parametros_Mes_Ano()
        {
            _domainService.RecuperarFluxoCaixa(_ano, _mes).Returns(Task.FromResult(FluxoCaixaMensalMock()));
            var service = new FluxoCaixaService(_domainService);
            var result = (await service.RecuperarFluxoCaixa(_ano, _mes)).ToArray();

            Assert.Equal(2, result.Length);

            Assert.Equal(new DateOnly(_ano, _mes, _dia10), result[0].Data);
            Assert.Equal(30, result[0].Debito);
            Assert.Equal(20, result[0].Credito);
            Assert.Equal(-10, result[0].Saldo);

            Assert.Equal(new DateOnly(_ano, _mes, _dia11), result[1].Data);
            Assert.Equal(50, result[1].Debito);
            Assert.Equal(100, result[1].Credito);
            Assert.Equal(50, result[1].Saldo);
        }
    }
}