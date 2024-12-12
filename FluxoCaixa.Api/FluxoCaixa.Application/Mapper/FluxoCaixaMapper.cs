using FluxoCaixa.Application.Dto;
using FluxoCaixa.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Application.Mapper
{
    public static class FluxoCaixaMapper
    {
        public static FluxoCaixaDto ToDto(this Domain.Model.FluxoCaixaItem model)
        {
            return new FluxoCaixaDto
            {
                Credito = model.Credito,
                Debito = model.Debito,
                Data = model.Data,
                Saldo = model.Saldo,
            };
        }

        public static IEnumerable<FluxoCaixaDto> ToDto(this Domain.Model.FluxoCaixa model)
        {
            return model.Items?.Select(i => new FluxoCaixaDto
            {
                Credito = i.Credito,
                Debito = i.Debito,
                Data = i.Data,
                Saldo = i.Saldo,
            });
        }
    }
}
