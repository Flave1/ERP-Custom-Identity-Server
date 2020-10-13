using APIGateway.Contracts.Queries.Admin;
using APIGateway.Data;
using AutoMapper; 
using GODP.APIsContinuation.DomainObjects.UserAccount;

using GOSLibraries.GOS_Error_logger.Service;
using GODP.APIsContinuation.Repository.Interface.Admin;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.Response.Admin;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GODP.APIsContinuation.Handlers.Admin
{
    public class GetAllStaffQueryHandler : IRequestHandler<GetAllStaffQuery, StaffRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IAdminRepository _repo;
        private readonly UserManager<cor_useraccount> _userManager;
        private readonly IMapper _mapper;
        public GetAllStaffQueryHandler(
            IAdminRepository adminRepository, 
            ILoggerService loggerService, 
            DataContext dataContext, 
            UserManager<cor_useraccount> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _logger = loggerService;
            _dataContext = dataContext;
            _repo = adminRepository;
            _mapper = mapper;
        }
        public async Task<StaffRespObj> Handle(GetAllStaffQuery request, CancellationToken cancellationToken)
        {
            var responses = new StaffRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            var JobList = _dataContext.cor_jobtitles.Where(x => x.Deleted == false).ToList();
            var companystructureList = _dataContext.cor_companystructure.Where(c => c.Deleted == false).ToList();
            var StaffUserList = await _repo.GetAllStaffAsync();
            var UserList = await _userManager.Users.Where(d => d.Deleted == false).ToListAsync();

            var respList = new List<StaffObj>(); 
            responses.Staff = _mapper.Map<List<StaffObj>>(StaffUserList); 
            foreach (var item in responses.Staff)
            {
                item.JobTitleName = JobList?.FirstOrDefault(x => item.JobTitle == x.JobTitleId)?.Name;
                item.StaffOfficeName = companystructureList?.FirstOrDefault(x => x.CompanyStructureId == item.StaffOfficeId)?.Name;
                item.UserName = UserList?.FirstOrDefault(d => d.StaffId == item.StaffId)?.UserName;
                item.UserId = UserList?.FirstOrDefault(d => d.StaffId == item.StaffId)?.Id;
            } 
            responses.Status.Message.FriendlyMessage = respList.Count() == 0 ? "Search Complete! No Record Found" : null;
            return responses;   
        }
    }
}
