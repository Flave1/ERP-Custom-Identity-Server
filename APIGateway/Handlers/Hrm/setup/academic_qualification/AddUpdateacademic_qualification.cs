using APIGateway.Contracts.Response.HRM;
using APIGateway.Data;
using MediatR;
using System;
using System.Threading;
using APIGateway.DomainObjects.hrm;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Hrm.setup.jobdetail
{
	public class Add_update_hrm_setup_academic_qualification : IRequestHandler<hrm_setup_academic_qualification_contract, hrm_setup_add_update_response>
	{
		private readonly DataContext _context;
		public Add_update_hrm_setup_academic_qualification(DataContext context)
		{
			_context = context;
		}
		public async Task<hrm_setup_add_update_response> Handle(hrm_setup_academic_qualification_contract request, CancellationToken cancellationToken)
		{
			var response = new hrm_setup_add_update_response();
			try
			{
				var setup = _context.hrm_setup_academic_qualification.Find(request.Id);
				if (setup == null) setup = new hrm_setup_academic_qualification();
				setup.Id = request.Id;
				setup.Description = request.Description;
				setup.Qualification = request.Qualification;
				setup.Rank = request.Rank; 
				

				if (setup.Id < 1)
					_context.hrm_setup_academic_qualification.Add(setup);


				_context.SaveChanges();
				response.Status.Message.FriendlyMessage = "Successful";
				response.Status.IsSuccessful = true;
				return response; 
			}
			catch (Exception e)
			{
				response.Status.Message.FriendlyMessage = "Error Occurred !! Please contact help desk";
				response.Status.Message.TechnicalMessage = e.ToString();
				return response;
			}
		}
	}
}
