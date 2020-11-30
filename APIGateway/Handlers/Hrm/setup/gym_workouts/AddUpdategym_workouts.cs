using APIGateway.Contracts.Response.HRM;
using APIGateway.Data;
using MediatR;
using System;
using System.Threading;
using APIGateway.DomainObjects.hrm;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Hrm.setup.jobdetail
{
	public class Add_update_hrm_setup_gym_workouts : IRequestHandler<hrm_setup_gym_workouts_contract, hrm_setup_add_update_response>
	{
		private readonly DataContext _context;
		public Add_update_hrm_setup_gym_workouts(DataContext context)
		{
			_context = context;
		}
		public async Task<hrm_setup_add_update_response> Handle(hrm_setup_gym_workouts_contract request, CancellationToken cancellationToken)
		{
			var response = new hrm_setup_add_update_response();
			try
			{
				var setup = _context.hrm_setup_gym_workouts.Find(request.Id);
				if (setup == null) setup = new hrm_setup_gym_workouts();
				setup.Id = request.Id;
				setup.Address = request.Address; 
				setup.Contact_phone_number = request.Contact_phone_number;
				setup.Gym = request.Gym;
				setup.Other_comments = request.Other_comments;
				setup.Ratings = request.Ratings; 
				
				 
				if (setup.Id < 1)
					_context.hrm_setup_gym_workouts.Add(setup);


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
