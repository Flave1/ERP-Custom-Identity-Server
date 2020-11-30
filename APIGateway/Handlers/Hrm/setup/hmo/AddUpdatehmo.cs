using APIGateway.Contracts.Response.HRM;
using APIGateway.Data;
using MediatR;
using System;
using System.Threading;
using APIGateway.DomainObjects.hrm;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Hrm.setup.jobdetail
{
	public class Add_update_hrm_setup_hmo : IRequestHandler<hrm_setup_hmo_contract, hrm_setup_add_update_response>
	{
		private readonly DataContext _context;
		public Add_update_hrm_setup_hmo(DataContext context)
		{
			_context = context;
		}
		public async Task<hrm_setup_add_update_response> Handle(hrm_setup_hmo_contract request, CancellationToken cancellationToken)
		{
			var response = new hrm_setup_add_update_response();
			try
			{
				var setup = _context.hrm_setup_hmo.Find(request.Id);
				if (setup == null) setup = new hrm_setup_hmo();
				setup.Id = request.Id;
				setup.Address = request.Address;
				setup.Contact_email = request.Contact_email;
				setup.Contact_phone_number = request.Contact_phone_number;
				setup.Hmo_code = request.Hmo_code;
				setup.Hmo_name = request.Hmo_name;
				setup.Order_comments = request.Order_comments;
				setup.Rating = request.Rating;
				setup.Reg_date = request.Reg_date;
				
				 
				if (setup.Id < 1)
					_context.hrm_setup_hmo.Add(setup);


				_context.SaveChanges();
				response.Status.IsSuccessful = true;
				response.Status.Message.FriendlyMessage = "Successful"; 
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
