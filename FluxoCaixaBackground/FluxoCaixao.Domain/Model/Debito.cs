using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Domain.Model
{
    public class Debito : Lancamento
    {
        public Debito(Guid id, DateTime dataHora, decimal valor, string descricao)
            : base(id, dataHora, valor, descricao)
        {
        }

        protected override TipoLancamento DefinirTipoLancamento() => TipoLancamento.D;
    }
}
