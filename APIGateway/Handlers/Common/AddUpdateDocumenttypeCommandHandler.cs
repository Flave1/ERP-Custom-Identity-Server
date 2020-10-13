using APIGateway.Contracts.Commands.Common;
using APIGateway.Contracts.Response.Common;
using APIGateway.Data;
using APIGateway.DomainObjects.Credit;
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
    public class AddUpdateDocumenttypeCommandHandler : IRequestHandler<AddUpdateDocumenttypeCommand, LookUpRegRespObj>
    {
        private readonly ICommonRepository _repo;
        private readonly DataContext _dataContext;
        public AddUpdateDocumenttypeCommandHandler(ICommonRepository commonRepository, DataContext dataContext)
        {
            _dataContext = dataContext;
            _repo = commonRepository;
        }
        public async Task<LookUpRegRespObj> Handle(AddUpdateDocumenttypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.DocumentTypeId < 1)
                {
                    if (await _dataContext.credit_documenttype.AnyAsync(x => x.Name.Trim().ToLower() == request.Name.Trim().ToLower() && x.Deleted == false))
                    {
                        return new LookUpRegRespObj { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "This Name Already Exist" } } };
                    }
                }
                var item = new credit_documenttype
                {
                    Active = true,
                    Name = request.Name, 
                    CreatedOn = request.CreatedOn,
                    Deleted = false,
                    CreatedBy = request.CreatedBy
                };
                if(request.DocumentTypeId > 0)
                {
                    item.DocumentTypeId = request.DocumentTypeId;
                    item.UpdatedOn = DateTime.Today;
                }
                var addedOrUpdated = await _repo.AddUpdateDocumentTypeAsync(item); 
                return new LookUpRegRespObj { LookUpId = item.DocumentTypeId, Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } } };

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
