using ControleLancamento.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleLancamento.Domain.Model
{
    public class Debito : LancamentoBase
    {
        protected Debito(DateTime dataHora, decimal valor, string descricao) 
            : base(dataHora, valor, descricao)
        {
        }

        protected override TipoLancamento DefinirTipoLancamento() => TipoLancamento.D;

        public static Debito Lancar(DateTime dataHora, decimal valor, string descricao)
        {
            var debito = new Debito(dataHora, valor, descricao);
            var @event = DebitoLancadoEvent.Criar(debito);
            debito.AdicionarEvento(@event);

            return debito;
        }
    }
}
