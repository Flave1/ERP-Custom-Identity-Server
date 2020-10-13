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
	public class AddUpdateBranchCommandHandler : IRequestHandler<AddUpdateBranchCommand, LookUpRegRespObj>
	{
		
		private readonly DataContext _dataContext;
		private readonly ICommonRepository _repo;
		public AddUpdateBranchCommandHandler(DataContext dataContext, ICommonRepository commonRepository)
		{
			_dataContext = dataContext;
			_repo = commonRepository;
		}
		public async Task<LookUpRegRespObj> Handle(AddUpdateBranchCommand request, CancellationToken cancellationToken)
		{
			try
			{
				if (request.BranchId < 1)
				{
					if (await _dataContext.cor_branch.AnyAsync(x => x.BranchName.Trim().ToLower() == 
					request.BranchName.Trim().ToLower() && x.Deleted == false))
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
				var item = new cor_branch
				{
					Active = true,
					BranchId = request.BranchId,
					Address = request.Address,
					BranchCode = request.BranchCode,
					BranchName = request.BranchName,
					CompanyId = request.CompanyId, 
					CreatedOn = request.CreatedOn,
					Deleted = false,
					CreatedBy = request.CreatedBy
				};
				var addedOrUpdated = await _repo.AddUpdateBranchAsync(item);
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
