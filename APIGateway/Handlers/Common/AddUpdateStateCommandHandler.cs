using APIGateway.Contracts.Commands.Common;
using APIGateway.Contracts.Response.Common;
using APIGateway.Data;
using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.DomainObjects.Company;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Common
{
	public class AddUpdateStateCommand : IRequest<LookUpRegRespObj>
	{
		public int StateId { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public int CountryId { get; set; }
		public class AddUpdateStateCommandHandler : IRequestHandler<AddUpdateStateCommand, LookUpRegRespObj>
		{
			private readonly DataContext _dataContext;
			private readonly ICommonRepository _repo;
			public AddUpdateStateCommandHandler(DataContext dataContext, ICommonRepository commonRepository)
			{
				_dataContext = dataContext;
				_repo = commonRepository;
			}
			public async Task<LookUpRegRespObj> Handle(AddUpdateStateCommand request, CancellationToken cancellationToken)
			{
				try
				{ 
					var item = new cor_state
					{
						Active = true,
						StateCode = request.Code,
						StateId = request.StateId,
						CountryId = request.CountryId,
						StateName = request.Name,
						CreatedOn = DateTime.Now,
						Deleted = false,
						CreatedBy = ""
					};
					var addedOrUpdated = await _repo.AddUpdateStateAsync(item);
					if (!addedOrUpdated)
						return new LookUpRegRespObj
						{
							Status = new APIResponseStatus
							{
								IsSuccessful = false,
								Message = new APIResponseMessage
								{
									FriendlyMessage = "Unable to add item"
								}
							}
						};
					return new LookUpRegRespObj
					{
						LookUpId = item.StateId,
						Status = new APIResponseStatus
						{
							IsSuccessful = true,
							Message = new APIResponseMessage
							{
								FriendlyMessage = "Item added successfully"
							}
						}
					};
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

	
}
