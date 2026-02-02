using System;
using System.Collections.Generic;
using System.Text;

namespace Opah.ReportService.Application.Dtos
{
    public record ReportDto(UInt16 Year, UInt16 Month, IEnumerable<ReportItemDto>? Items);
}
