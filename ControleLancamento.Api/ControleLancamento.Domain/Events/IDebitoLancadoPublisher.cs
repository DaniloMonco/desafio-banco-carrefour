using ControleLancamento.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleLancamento.Domain.Events
{
    public interface IDebitoLancadoPublisher
    {
        Task Publicar(Debito model);
    }
}
