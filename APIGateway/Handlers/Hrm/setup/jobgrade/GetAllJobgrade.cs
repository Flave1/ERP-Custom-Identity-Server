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
    public class GetAll_hrm_setup_jobgrade_Query : IRequest<hrm_setup_jobgrade_contract_resp>
    {
        public class GetAll_hrm_setup_jobgrade_QueryHandler : IRequestHandler<GetAll_hrm_setup_jobgrade_Query, hrm_setup_jobgrade_contract_resp>
        { 
            private readonly DataContext _data;
            public GetAll_hrm_setup_jobgrade_QueryHandler(DataContext data)
            {
                _data = data;
            }
            public async Task<hrm_setup_jobgrade_contract_resp> Handle(GetAll_hrm_setup_jobgrade_Query request, CancellationToken cancellationToken)
            {
                var response = new hrm_setup_jobgrade_contract_resp { Setuplist = new List<hrm_setup_jobgrade_contract>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                var sol = await _data.hrm_setup_jobgrade.ToListAsync();
                response.Setuplist = sol.Select(a => new hrm_setup_jobgrade_contract
                {
                    Id = a.Id,
                    Rank = a.Rank,
                    Probation_period_in_months = a.Probation_period_in_months,
                    Description = a.Description,
                    Job_grade = a.Job_grade,
                    Job_grade_reporting_to = a.Job_grade_reporting_to,
                    Job_grade_reporting_to_name = _data.hrm_setup_jobgrade.FirstOrDefault(e => e.Id == a.Job_grade_reporting_to)?.Job_grade
                }).ToList();

                response.Status.Message.FriendlyMessage = sol.Count() > 0 ? string.Empty : "Search Complete!! No record found";
                return response;
            }
        }
    }

}
