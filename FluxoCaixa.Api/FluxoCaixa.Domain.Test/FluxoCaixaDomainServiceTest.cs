using FluxoCaixa.Domain.Cached;
using FluxoCaixa.Domain.DomainService;
using FluxoCaixa.Domain.Model;
using FluxoCaixa.Domain.Repository;
using NSubstitute;

namespace FluxoCaixa.Domain.Test
{
    public class FluxoCaixaDomainServiceTest
    {
        private const int _ano = 2024;
        private const int _mes = 8;
        private const int _dia10 = 10;
        private const int _dia11 = 11;

        private ILancamentoRepository _repository;
        private IFluxoCaixaCached _cache;

        public FluxoCaixaDomainServiceTest()
        {
            _repository = Substitute.For<ILancamentoRepository>();
            _cache = Substitute.For<IFluxoCaixaCached>();
        }

        private IEnumerable<Lancamento> LancamentosDiarioMock()
        {
            var dataHora = new DateTime(_ano, _mes, _dia10);

            return new List<Lancamento>
            {
                new Lancamento{DataHora=dataHora, Descricao = "Debito 1", TipoLancamento = TipoLancamento.D, Valor=10},
                new Lancamento{DataHora=dataHora, Descricao = "Debito 2", TipoLancamento = TipoLancamento.D, Valor=20},
                new Lancamento{DataHora=dataHora, Descricao = "Credito 1", TipoLancamento = TipoLancamento.C, Valor=20},
            };
        }

        private Model.FluxoCaixa FluxoCaixaDiarioMock()
        {
            var fluxoCaixa = Model.FluxoCaixa.Criar(_ano, _mes);
            fluxoCaixa.MontarFluxoCaixa(LancamentosDiarioMock());
            return fluxoCaixa;
        }

        private IEnumerable<Lancamento> LancamentosMensalMock()
        {
            var dataHora10 = new DateTime(_ano, _mes, _dia10);
            var dataHora11 = new DateTime(_ano, _mes, _dia11);
            return new List<Lancamento>
            {
                new Lancamento{DataHora=dataHora10, Descricao = "Debito 1", TipoLancamento = TipoLancamento.D, Valor=10},
                new Lancamento{DataHora=dataHora10, Descricao = "Debito 2", TipoLancamento = TipoLancamento.D, Valor=20},
                new Lancamento{DataHora=dataHora10, Descricao = "Credito 1", TipoLancamento = TipoLancamento.C, Valor=20},
                new Lancamento{DataHora=dataHora11, Descricao = "Credito 2", TipoLancamento = TipoLancamento.C, Valor=100},
                new Lancamento{DataHora=dataHora11, Descricao = "Debito 3", TipoLancamento = TipoLancamento.D, Valor=50},
            };
        }

        private Model.FluxoCaixa FluxoCaixaMensalMock()
        {
            var fluxoCaixa = Model.FluxoCaixa.Criar(_ano, _mes);
            fluxoCaixa.MontarFluxoCaixa(LancamentosMensalMock());
            return fluxoCaixa;
        }

        [Fact]
        public async Task Retorna_FluxoCaixa_Com_Parametros_Dia_Mes_Ano_SemCache()
        {
            _repository.RecuperarLancamentos(_ano, _mes, _dia10).Returns(Task.FromResult(LancamentosDiarioMock()));
            var service = new FluxoCaixaService(_repository, _cache);
            var fluxoCaixa = await service.RecuperarFluxoCaixa(_ano, _mes, _dia10);

            Assert.Equal(_ano, fluxoCaixa.Ano);
            Assert.Equal(_mes, fluxoCaixa.Mes);
            Assert.Equal(-10, fluxoCaixa.Saldo);
            Assert.Single(fluxoCaixa.Items);
            Assert.Equal(new DateOnly(_ano, _mes, _dia10), fluxoCaixa.Items[0].Data);
            Assert.Equal(30, fluxoCaixa.Items[0].Debito);
            Assert.Equal(20, fluxoCaixa.Items[0].Credito);
            Assert.Equal(-10, fluxoCaixa.Items[0].Saldo);

            await _cache.Received().SetAsync<Model.FluxoCaixa>(Arg.Any<string>(), fluxoCaixa, TimeSpan.FromMinutes(5));
        }

        [Fact]
        public async Task Retorna_FluxoCaixa_Com_Parametros_Mes_Ano_SemCache()
        {
            _repository.RecuperarLancamentos(_ano, _mes).Returns(Task.FromResult(LancamentosMensalMock()));
            var service = new FluxoCaixaService(_repository, _cache);
            var fluxoCaixa = await service.RecuperarFluxoCaixa(_ano, _mes);

            Assert.Equal(_ano, fluxoCaixa.Ano);
            Assert.Equal(_mes, fluxoCaixa.Mes);
            Assert.Equal(40, fluxoCaixa.Saldo);
            Assert.Equal(2, fluxoCaixa.Items.Count);
            Assert.Equal(new DateOnly(_ano, _mes, _dia10), fluxoCaixa.Items[0].Data);
            Assert.Equal(30, fluxoCaixa.Items[0].Debito);
            Assert.Equal(20, fluxoCaixa.Items[0].Credito);
            Assert.Equal(-10, fluxoCaixa.Items[0].Saldo);
            Assert.Equal(new DateOnly(_ano, _mes, _dia11), fluxoCaixa.Items[1].Data);
            Assert.Equal(50, fluxoCaixa.Items[1].Debito);
            Assert.Equal(100, fluxoCaixa.Items[1].Credito);
            Assert.Equal(50, fluxoCaixa.Items[1].Saldo);

            await _cache.Received().SetAsync<Model.FluxoCaixa>(Arg.Any<string>(), fluxoCaixa, TimeSpan.FromHours(12));
        }

        [Fact]
        public async Task Retorna_FluxoCaixa_Com_Parametros_Dia_Mes_Ano_ComCache()
        {
            _cache.GetAsync<Model.FluxoCaixa>(Arg.Any<string>()).Returns(Task.FromResult(FluxoCaixaDiarioMock()));
            var service = new FluxoCaixaService(_repository, _cache);
            var fluxoCaixa = await service.RecuperarFluxoCaixa(_ano, _mes, _dia10);

            Assert.Equal(_ano, fluxoCaixa.Ano);
            Assert.Equal(_mes, fluxoCaixa.Mes);
            Assert.Equal(-10, fluxoCaixa.Saldo);
            Assert.Single(fluxoCaixa.Items);
            Assert.Equal(new DateOnly(_ano, _mes, _dia10), fluxoCaixa.Items[0].Data);
            Assert.Equal(30, fluxoCaixa.Items[0].Debito);
            Assert.Equal(20, fluxoCaixa.Items[0].Credito);
            Assert.Equal(-10, fluxoCaixa.Items[0].Saldo);

            await _cache.Received().GetAsync<Model.FluxoCaixa>(Arg.Any<string>());
            await _repository.DidNotReceive().RecuperarLancamentos(_ano, _mes, _dia10);
            await _cache.DidNotReceive().SetAsync<Model.FluxoCaixa>(Arg.Any<string>(), fluxoCaixa, TimeSpan.FromMinutes(5));
        }

        [Fact]
        public async Task Retorna_FluxoCaixa_Com_Parametros_Mes_Ano_ComCache()
        {
            _cache.GetAsync<Model.FluxoCaixa>(Arg.Any<string>()).Returns(Task.FromResult(FluxoCaixaMensalMock()));
            var service = new FluxoCaixaService(_repository, _cache);
            var fluxoCaixa = await service.RecuperarFluxoCaixa(_ano, _mes);

            Assert.Equal(_ano, fluxoCaixa.Ano);
            Assert.Equal(_mes, fluxoCaixa.Mes);
            Assert.Equal(40, fluxoCaixa.Saldo);
            Assert.Equal(2, fluxoCaixa.Items.Count);
            Assert.Equal(new DateOnly(_ano, _mes, _dia10), fluxoCaixa.Items[0].Data);
            Assert.Equal(30, fluxoCaixa.Items[0].Debito);
            Assert.Equal(20, fluxoCaixa.Items[0].Credito);
            Assert.Equal(-10, fluxoCaixa.Items[0].Saldo);
            Assert.Equal(new DateOnly(_ano, _mes, _dia11), fluxoCaixa.Items[1].Data);
            Assert.Equal(50, fluxoCaixa.Items[1].Debito);
            Assert.Equal(100, fluxoCaixa.Items[1].Credito);
            Assert.Equal(50, fluxoCaixa.Items[1].Saldo);

            await _cache.Received().GetAsync<Model.FluxoCaixa>(Arg.Any<string>());
            await _repository.DidNotReceive().RecuperarLancamentos(_ano, _mes, _dia10);
            await _cache.DidNotReceive().SetAsync<Model.FluxoCaixa>(Arg.Any<string>(), fluxoCaixa, TimeSpan.FromHours(12));
        }
    }
}