using APIGateway.Contracts.Commands.Common;
using APIGateway.Contracts.Response.Common;
using APIGateway.Data;
using APIGateway.Repository.Interface.Common;
using GODP.APIsContinuation.DomainObjects.Currency;
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
	public class AddUpdateCurrencyRateCommandHandler : IRequestHandler<AddUpdateCurrencyRateCommand, LookUpRegRespObj>
	{
		private readonly DataContext _dataContext;
		private readonly ICommonRepository _repo;
		public AddUpdateCurrencyRateCommandHandler(DataContext dataContext, ICommonRepository commonRepository)
		{
			_dataContext = dataContext;
			_repo = commonRepository;
		}
		public async Task<LookUpRegRespObj> Handle(AddUpdateCurrencyRateCommand request, CancellationToken cancellationToken)
		{
			try
			{
				 if(_dataContext.cor_currencyrate.FirstOrDefault(c => c.CurrencyId == request.CurrencyId && c.Date == request.Date && c.Deleted == false) != null)
				{
					return new LookUpRegRespObj
					{
						Status = new APIResponseStatus
						{
							IsSuccessful = false,
							Message = new APIResponseMessage
							{
								FriendlyMessage = "This Currency rate already exist"
							}
						}
					};
				}
				var item = new cor_currencyrate
				{
					Active = true, 
					CreatedOn = request.CreatedOn,
					Deleted = false,
					CreatedBy = request.CreatedBy,
					SellingRate = request.SellingRate,
					CurrencyId = request.CurrencyId,
					BuyingRate = request.BuyingRate,
					CurrencyCode = request.CurrencyCode,
					Date = request.Date,
				};
				if(request.CurrencyRateId > 0)
				{
					item.CurrencyRateId = request.CurrencyRateId; 
				}
				await _repo.AddUpdateCurrencyRateAsync(item); 
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
