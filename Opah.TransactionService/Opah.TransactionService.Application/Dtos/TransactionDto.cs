using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Opah.TransactionService.Domain.ValueObject;

namespace Opah.TransactionService.Application.Dtos
{
    public abstract record TransactionDto(decimal Amount, 
                                          [property: JsonIgnore(Condition = JsonIgnoreCondition.Always)] TransactionTypeDto Type
                                          )
    {
        [property: JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public string? UserName { get; set; }
    }
}
