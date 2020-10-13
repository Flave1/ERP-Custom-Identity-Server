using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Commands.Admin
{
    public class GenerateExportStaffCommad : IRequest<byte[]> { }
}
