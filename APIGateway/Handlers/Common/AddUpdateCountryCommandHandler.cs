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
    public class AddUpdateCountryCommandHandler : IRequestHandler<AddUpdateCountryCommand, LookUpRegRespObj>
    {
		private readonly DataContext _dataContext;
		private readonly ICommonRepository _repo;
		public AddUpdateCountryCommandHandler(DataContext dataContext, ICommonRepository commonRepository)
		{
			_dataContext = dataContext;
			_repo = commonRepository;
		}
        public async Task<LookUpRegRespObj> Handle(AddUpdateCountryCommand request, CancellationToken cancellationToken)
        {
			try
			{
				if(request.CountryId < 1)
				{
					if(await _dataContext.cor_country.AnyAsync(x => x.CountryName.Trim().ToLower() == request.CountryName.Trim().ToLower() && x.Deleted == false))
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
				var item = new cor_country
				{
					Active = true,
					CountryCode = request.CountryCode,
					CountryId = request.CountryId,
					CountryName = request.CountryName,
					CreatedOn = request.CreatedOn,
					Deleted = false,
					CreatedBy = request.CreatedBy
				};
				
				await _repo.AddUpdateCountryAsync(item); 
				return new LookUpRegRespObj
				{
					Status = new APIResponseStatus
					{
						IsSuccessful = true,
						Message = new APIResponseMessage
						{
							FriendlyMessage = "Item Added successfully"
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
