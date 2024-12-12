using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Domain.Model
{
    public class Lancamento
    {
        public Guid Id { get; set; }
        public DateTime DataHora { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public TipoLancamento TipoLancamento { get; set; }

    }
}
