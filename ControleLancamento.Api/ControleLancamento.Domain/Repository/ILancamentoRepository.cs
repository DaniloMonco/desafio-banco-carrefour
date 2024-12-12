using ControleLancamento.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleLancamento.Domain.Repository
{
    public interface ILancamentoRepository
    {
        Task Salvar(Credito credito);
        Task Salvar(Debito debito);
    }
}
