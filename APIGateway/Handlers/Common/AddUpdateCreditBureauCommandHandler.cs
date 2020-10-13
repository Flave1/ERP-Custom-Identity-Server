using APIGateway.Contracts.Commands.Common;
using APIGateway.Contracts.Response.Common;
using APIGateway.Data;
using APIGateway.Repository.Interface.Common; 
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
	public class AddUpdateCreditBureauCommandHandler : IRequestHandler<AddUpdateCreditBureauCommand, LookUpRegRespObj>
	{
		private readonly DataContext _dataContext;
		private readonly ICommonRepository _repo;
		public AddUpdateCreditBureauCommandHandler(DataContext dataContext, ICommonRepository commonRepository)
		{
			_dataContext = dataContext;
			_repo = commonRepository;
		}
		public async Task<LookUpRegRespObj> Handle(AddUpdateCreditBureauCommand request, CancellationToken cancellationToken)
		{
			try
			{
				if (request.CreditBureauId < 1)
				{
					if (await _dataContext.credit_creditbureau.AnyAsync(x => x.CreditBureauName.Trim().ToLower() == request.CreditBureauName.Trim().ToLower()))
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
				var item = new credit_creditbureau
				{
					Active = true, 
					CreatedOn = request.CreatedOn,
					Deleted = false,
					CreatedBy = request.CreatedBy,
					CorporateChargeAmount = request.CorporateChargeAmount,
					IsMandatory = request.IsMandatory,
					GLAccountId = request.GLAccountId,
					IndividualChargeAmount = request.IndividualChargeAmount,
					CreditBureauName  =request.CreditBureauName,
				};

				if(request.CreditBureauId > 0)
				{
					item.CreditBureauId = request.CreditBureauId;
					item.UpdatedOn = DateTime.Today;
				}
				var addedOrUpdated = await _repo.AddUpdateCreditBureauAsync(item);
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
