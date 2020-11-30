using APIGateway.Contracts.Response.HRM;
using APIGateway.Data;
using MediatR;
using System;
using System.Threading;
using APIGateway.DomainObjects.hrm;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Hrm.setup.jobdetail
{
	public class Add_update_hrm_setup_jobetails : IRequestHandler<hrm_setup_jobdetails_contract, hrm_setup_add_update_response>
	{
		private readonly DataContext _context;
		public Add_update_hrm_setup_jobetails(DataContext context)
		{
			_context = context;
		}
		public async Task<hrm_setup_add_update_response> Handle(hrm_setup_jobdetails_contract request, CancellationToken cancellationToken)
		{
			var response = new hrm_setup_add_update_response();
			try
			{
				var setup = _context.hrm_setup_jobdetails.Find(request.Id);
				if (setup == null) setup = new hrm_setup_jobdetails();
				setup.Id = request.Id;
				setup.Job_description = request.Job_description;
				setup.Job_title = request.Job_title;
				using(var trans = _context.Database.BeginTransaction())
				{
					try
					{
						if (setup.Id < 1)
							_context.hrm_setup_jobdetails.Add(setup);

						if (request.Sub_Skills.Count > 0)
						{
							foreach (var item in request.Sub_Skills)
							{
								var sub_item = new hrm_setup_sub_skill();
								sub_item.Id = item.Id;
								sub_item.Job_details_Id = setup.Id;
								sub_item.Skill = item.Skill;
								sub_item.Weight = item.Weight;
								if (sub_item.Id < 1)
									_context.hrm_setup_sub_skill.Add(sub_item);
							}
						}
						_context.SaveChanges();
						trans.Commit();
						response.Status.Message.FriendlyMessage = "Successful";
						response.Status.IsSuccessful = true;
						return response;
					}
					catch (Exception e)
					{
						trans.Rollback();
						response.Status.Message.FriendlyMessage = "Error Occurred !! Please contact help desk";
						response.Status.Message.TechnicalMessage = e.ToString();
						return response;
					}
					finally { trans.Dispose(); } 
				}
				
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
