using APIGateway.Contracts.Response.HRM;
using APIGateway.Data;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Hrm.setup.jobdetail
{
    public class GetSingle_hrm_setup_high_school_subjects_Query : IRequest<hrm_setup_high_school_subjects_contract_resp>
    {
        public int SetupId { get; set; }
        public class GetSingle_hrm_setup_high_school_subjects_QueryHandler : IRequestHandler<GetSingle_hrm_setup_high_school_subjects_Query, hrm_setup_high_school_subjects_contract_resp>
        { 
            private readonly DataContext _data;
            public GetSingle_hrm_setup_high_school_subjects_QueryHandler(
                DataContext data)
            {
                _data = data;
            }
            public async Task<hrm_setup_high_school_subjects_contract_resp> Handle(GetSingle_hrm_setup_high_school_subjects_Query request, CancellationToken cancellationToken)
            {
                var response = new hrm_setup_high_school_subjects_contract_resp { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                var sol = await _data.hrm_setup_high_school_subjects.Where(e => e.Id == request.SetupId).ToListAsync();
                response.Setuplist = sol.Select(a => new hrm_setup_high_school_subjects_contract
                {
                    Id = a.Id, 
                    Description = a.Description,
                    Subject = a.Subject
                }).ToList();

                response.Status.Message.FriendlyMessage = sol.Count() > 0 ? string.Empty : "Search Complete!! No record found";
                return response;
            }
        }
    }

}
