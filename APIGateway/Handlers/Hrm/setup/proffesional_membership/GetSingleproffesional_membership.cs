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
    public class GetSingle_hrm_setup_proffesional_membership_Query : IRequest<hrm_setup_proffesional_membership_contract_resp>
    {
        public int SetupId { get; set; }
        public class GetSingle_hrm_setup_proffesional_membership_QueryHandler : IRequestHandler<GetSingle_hrm_setup_proffesional_membership_Query, hrm_setup_proffesional_membership_contract_resp>
        { 
            private readonly DataContext _data;
            public GetSingle_hrm_setup_proffesional_membership_QueryHandler(
                DataContext data)
            {
                _data = data;
            }
            public async Task<hrm_setup_proffesional_membership_contract_resp> Handle(GetSingle_hrm_setup_proffesional_membership_Query request, CancellationToken cancellationToken)
            {
                var response = new hrm_setup_proffesional_membership_contract_resp { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                var sol = await _data.hrm_setup_proffesional_membership.Where(e => e.Id == request.SetupId).ToListAsync();
                response.Setuplist = sol.Select(a => new hrm_setup_proffesional_membership_contract
                {
                    Id = a.Id,
                    Professional_membership = a.Professional_membership,
                    Description = a.Description
                }).ToList();

                response.Status.Message.FriendlyMessage = sol.Count() > 0 ? string.Empty : "Search Complete!! No record found";
                return response;
            }
        }
    }

}
