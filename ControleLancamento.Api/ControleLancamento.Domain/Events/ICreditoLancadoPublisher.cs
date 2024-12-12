using ControleLancamento.Domain.Model;

namespace ControleLancamento.Domain.Events
{
    public interface ICreditoLancadoPublisher
    {
        Task Publicar(Credito model);
    }
}
