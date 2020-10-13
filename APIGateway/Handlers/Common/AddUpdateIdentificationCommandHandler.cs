using APIGateway.Contracts.Commands.Common;
using APIGateway.Contracts.Response.Common;
using APIGateway.Data;
using APIGateway.DomainObjects.Credit;
using APIGateway.Repository.Interface.Common; 
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System; 
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Common
{ 

	public class AddUpdateIdentificationCommandHandler : IRequestHandler<AddUpdateIdentificationCommand, LookUpRegRespObj>
	{
		private readonly DataContext _dataContext;
		private readonly ICommonRepository _repo;
		public AddUpdateIdentificationCommandHandler(DataContext dataContext, ICommonRepository commonRepository)
		{
			_dataContext = dataContext;
			_repo = commonRepository;
		}
		public async Task<LookUpRegRespObj> Handle(AddUpdateIdentificationCommand request, CancellationToken cancellationToken)
		{
			try
			{
				if (request.IdentificationId < 1)
				{
					if (await _dataContext.cor_identification.AnyAsync(x => x.IdentificationName.Trim().ToLower() == request.IdentificationName.Trim().ToLower() && x.Deleted == false))
					{
						return new LookUpRegRespObj
						{
							Status = new APIResponseStatus
							{
								IsSuccessful = false,
								Message = new APIResponseMessage
								{
									FriendlyMessage = "This Name Already Exist"
								}
							}
						};
					}
				}
				var item = new cor_identification
				{
					Active = true,
					CreatedOn = request.CreatedOn,
					Deleted = false,
					CreatedBy = request.CreatedBy,
					IdentificationName = request.IdentificationName,
					
				};
				if (request.IdentificationId > 0)
				{
					item.IdentificationId = request.IdentificationId;
					item.UpdatedOn = DateTime.Today;
				}
				var addedOrUpdated = await _repo.AddUpdateIdentificationAsync(item);
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
					Status = new APIResponseStatus
					{
						IsSuccessful = true,
						Message = new APIResponseMessage
						{
							FriendlyMessage = "Successful"
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
