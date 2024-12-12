using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Application.Messages
{
    public abstract class LancamentoMessage
    {
        public Guid EventId { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid LancamentoId { get; set; }
        public DateTime DataHora { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
    }
}
