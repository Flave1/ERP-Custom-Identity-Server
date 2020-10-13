using APIGateway.Contracts.Commands.Common;
using APIGateway.Contracts.Response.Common;
using APIGateway.Data;
using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.DomainObjects.Company;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Common
{

	public class AddUpdateCityCommandHandler : IRequestHandler<AddUpdateCityCommand, LookUpRegRespObj>
	{
		private readonly DataContext _dataContext;
		private readonly ICommonRepository _repo;
		public AddUpdateCityCommandHandler(DataContext dataContext, ICommonRepository commonRepository)
		{
			_dataContext = dataContext;
			_repo = commonRepository;
		}
		public async Task<LookUpRegRespObj> Handle(AddUpdateCityCommand request, CancellationToken cancellationToken)
		{
			try
			{
				if (request.CityId < 1)
				{
					if (await _dataContext.cor_jobtitles.AnyAsync(x => x.Name.Trim().ToLower() == request.CityName.Trim().ToLower() && x.Deleted == false))
					{
						return new LookUpRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "This Name Already Exist" } } };
					}
				}
				var item = new cor_city
				{
					Active = true,
					CityId = request.CityId,
					CityCode = request.CityCode,
					CityName = request.CityName,
					StateId = request.StateId,
					CreatedOn = request.CreatedOn,
					Deleted = false,
					CreatedBy = request.CreatedBy
				};

				var addedOrUpdated = await _repo.AddUpdateCityAsync(item); 
			
				return new LookUpRegRespObj { Status = new APIResponseStatus{ IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } } };
			
			}
			catch (Exception ex)
			{
				#region Log error to file 
				return new LookUpRegRespObj
				{

					Status = new APIResponseStatus
					{
						IsSuccessful = false,
						Message = new APIResponseMessage
						{
							FriendlyMessage = "Error occured!! Unable to process request",
							TechnicalMessage = $"{ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
						}
					}
				};
				#endregion
			}
		}
	}
	  
}
