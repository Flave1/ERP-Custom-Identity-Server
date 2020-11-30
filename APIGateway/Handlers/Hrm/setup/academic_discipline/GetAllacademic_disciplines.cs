using APIGateway.Contracts.Response.HRM;
using APIGateway.Data;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Hrm.setup.jobdetail
{
    public class GetAll_hrm_setup_academic_discipline_Query : IRequest<hrm_setup_academic_discipline_contract_resp>
    {
        public class GetAll_hrm_setup_academic_discipline_QueryHandler : IRequestHandler<GetAll_hrm_setup_academic_discipline_Query, hrm_setup_academic_discipline_contract_resp>
        { 
            private readonly DataContext _data;
            public GetAll_hrm_setup_academic_discipline_QueryHandler(DataContext data)
            {
                _data = data;
            }
            public async Task<hrm_setup_academic_discipline_contract_resp> Handle(GetAll_hrm_setup_academic_discipline_Query request, CancellationToken cancellationToken)
            {
                var response = new hrm_setup_academic_discipline_contract_resp { Setuplist = new List<hrm_setup_academic_discipline_contract>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                var sol = await _data.hrm_setup_academic_discipline.ToListAsync();
                response.Setuplist = sol.Select(a => new hrm_setup_academic_discipline_contract
                {
                    Id = a.Id,
                    Rank = a.Rank,
                    Discipline = a.Discipline,
                    Description = a.Description, 
                }).ToList();

                response.Status.Message.FriendlyMessage = sol.Count() > 0 ? string.Empty : "Search Complete!! No record found";
                return response;
            }
        }
    }

}
