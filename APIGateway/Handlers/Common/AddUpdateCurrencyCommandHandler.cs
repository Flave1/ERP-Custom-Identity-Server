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

	public class AddUpdateCurrencyCommandHandler : IRequestHandler<AddUpdateCurrencyCommand, LookUpRegRespObj>
	{
		private readonly DataContext _dataContext;
		private readonly ICommonRepository _repo;
		public AddUpdateCurrencyCommandHandler(DataContext dataContext, ICommonRepository commonRepository)
		{
			_dataContext = dataContext;
			_repo = commonRepository;
		}
		public async Task<LookUpRegRespObj> Handle(AddUpdateCurrencyCommand request, CancellationToken cancellationToken)
		{
			try
			{
				if (request.CurrencyId < 1)
				{
					if (await _dataContext.cor_currency.AnyAsync(x => x.CurrencyName.Trim().ToLower() == request.CurrencyName.Trim().ToLower() && x.Deleted == false))
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
				var item = new cor_currency
				{ 
					CurrencyCode = request.CurrencyCode,
					CurrencyName = request.CurrencyName,
					INUSE = request.INUSE,
					BaseCurrency = request.BaseCurrency,
					CurrencyId = request.CurrencyId,
					Active = true ,
					Deleted = true,
				}; 

				await _repo.AddUpdateCurrencyAsync(item); 
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
