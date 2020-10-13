using APIGateway.Contracts.Commands.Company;
using APIGateway.Contracts.Queries.Company;
using AutoMapper;
using GODP.APIsContinuation.DomainObjects.Ifrs;
using GODP.APIsContinuation.DomainObjects.UserAccount; 
using GOSLibraries.GOS_Error_logger.Service;
using GODP.APIsContinuation.Repository.Interface.Setup.Company.CompanyStructure;
using GODPAPIs.Contracts.Commands.Company; 
using GODPAPIs.Contracts.Response.CompanySetup;
using GODPAPIs.Contracts.V1;
using GOSLibraries.GOS_API_Response;
using MediatR; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System; 
using System.IO;
using System.Linq; 
using System.Threading.Tasks; 
using APIGateway.Handlers.Workflow;
using APIGateway.Handlers.Company;
using APIGateway.Handlers.Ccompany;
using APIGateway.Handlers.Common.Uploads_Downloads;
using GODP.APIsContinuation.Handlers.Ccompany;
using APIGateway.Contracts.Response.Supplier;
using APIGateway.ActivityRequirement;
using GOSLibraries.Enums;

namespace GODP.APIsContinuation.Controllers.V1
{ 
   [ERPAuthorize]
    public class CompanyController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ICompanyRepository _compRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<cor_useraccount> _userManger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private IWebHostEnvironment _env; 

        private static string FSTemplateFolder = "~/Files/FSTemplate/";
        private static TimeSpan epochTicks = new TimeSpan(new DateTime(1970, 1, 1).Ticks);
        TimeSpan unixTicks = new TimeSpan(DateTime.UtcNow.Ticks) - epochTicks; 
        public CompanyController(IWebHostEnvironment env, ILoggerService loggerService, UserManager<cor_useraccount> userManager,  IHttpContextAccessor httpContextAccessor, IMapper mapper, IMediator mediator, ICompanyRepository companyRepository)
        {
            _compRepo = companyRepository;
            _mediator = mediator;
            _mapper = mapper;
            _userManger = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = loggerService;
            _env = env;
        }


        #region Company structure definition

        
        [HttpGet(ApiRoutes.ComapnyEndPoints.GET_ALL_COMPANY_STRUCTURE_DEFINITION)]
        public async Task<ActionResult<CompanyStructureDefinitionRespObj>> GetAllCompanyStructDef()
        {
            try
            {
                var list = await _compRepo.GetAllCompanyStructureDefinitionAsync();
              return  Ok( new CompanyStructureDefinitionRespObj
                {

                  CompanyStructureDefinitions = list.Select( x=> new CompanyStructureDefinitionObj
                  {
                      UpdatedOn =x.UpdatedOn,
                      UpdatedBy = x.UpdatedBy,
                      Active = x.Active, 
                      CreatedOn = x.CreatedOn,
                      CreatedBy = x.CreatedBy,
                      Definition = x.Definition,
                      Deleted = x.Deleted,
                      Description = x.Description,
                      OperatingLevel = x.OperatingLevel,
                      IsMultiCompany = x.IsMultiCompany,
                      StructureDefinitionId = x.StructureDefinitionId,
                      StructureLevel = x.StructureLevel
                  }).ToList(),
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = list.Count() > 0 ? null : "Search Complete! No record found"
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : GetAllCompanyStructDef{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CompanyStructureDefinitionRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process request",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : GetAllCompanyStructDef{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }

        [ERPActivity(Action = UserActions.Add, Activity = 10)]
        [HttpPost(ApiRoutes.ComapnyEndPoints.ADD_UPDATE_COMPANY_STRUCTURE_DEFINITION)]
        public async Task<ActionResult<CompanyStructureDefinitionRegRespObj>> AddUpdpateCompanyStructDef([FromBody] CompanyStructureDefinitionObj request)
        {
            try
            {
                if(request.StructureDefinitionId < 1)
                {
                    var compStrDef = await _compRepo.GetCompanyStructureDefinitionAsync(request.StructureDefinitionId);
                    if (compStrDef != null)
                        if (compStrDef.StructureLevel == request.StructureLevel
                            && compStrDef.OperatingLevel == request.OperatingLevel
                            && compStrDef.Definition == request.Definition
                            && compStrDef.IsMultiCompany == request.IsMultiCompany
                            )
                            return BadRequest(new CompanyStructureDefinitionRegRespObj
                            {
                                Status = new APIResponseStatus
                                {
                                    IsSuccessful = false,
                                    Message = new APIResponseMessage
                                    {
                                        FriendlyMessage = "This Structure already exist"
                                    }
                                }
                            });
                }

                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var user = await _userManger.FindByIdAsync(currentUserId);
                 
                request.UpdatedBy = user.UserName;
                request.UpdatedOn = DateTime.Now;
                request.CreatedBy = user.UserName;
                request.Deleted = false;
                request.Active = true;
                request.CompanyId = request.CompanyId;
                request.Description = request.Description;
                request.IsMultiCompany = request.IsMultiCompany;

                if (request.StructureDefinitionId < 1) { request.StructureDefinitionId = 0; request.Deleted = false; }
                await _compRepo.AddUpdateCompanyStructureDefinitionAsync(_mapper.Map<cor_companystructuredefinition>(request));

                var actionTaken = request.StructureDefinitionId > 0 ? "updated" : "added";
                return Ok(new CompanyStructureDefinitionRegRespObj
                {
                    StructureDefinitionId = request.StructureDefinitionId,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = $"Company structure successfully  {actionTaken}",
                        }
                    }
                });


            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : AddUpdqateCompany{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CompanyStructureDefinitionRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process request",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : AddUpdqateCompany{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }


        [ERPActivity(Action = UserActions.Delete, Activity = 10)]
        [HttpPost(ApiRoutes.ComapnyEndPoints.DELETE_COMPANY_STRUCTURE_DEF)]
        public async Task<IActionResult> DeleteCompanyStructDefinition([FromBody] DeleteCompanyStructureDefinitionCommand command)
        {
            var res = await _mediator.Send(command);
            if (res.Status.IsSuccessful)
                return Ok(res);
            return BadRequest(res);
        }

        //[HttpPost(ApiRoutes.ComapnyEndPoints.GENERATE_EXCEL_FOR_COMP_STRU_DEF)]
        //public async Task<ActionResult<byte[]>> GenerateExcel()
        //{
        //    try
        //    {
        //        return Ok(await _compRepo.GenerateExportCompanyStructureDefinitionAsync());
        //    }
        //    catch (Exception ex)
        //    {
        //        #region Log error to file 
        //        var errorCode = ErrorID.Generate(4);
        //        _logger.Error($"ErrorID : GenerateExcel{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
        //        return BadRequest(new CompanyStructureDefinitionRegRespObj
        //        {

        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = false,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Error occured!! Unable to process request",
        //                    MessageId = errorCode,
        //                    TechnicalMessage = $"ErrorID : GenerateExcel{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
        //                }
        //            }
        //        });
        //        #endregion
        //    }
        //}

        //[HttpPost(ApiRoutes.ComapnyEndPoints.UPLOAD_COMPANY_STRUCTURE_DEF)]
        //public async Task<IActionResult> UploadCompanyStructureDefinition()
        //{
        //    try
        //    {
        //        var httpRequest = HttpContext.Request;

        //        if (httpRequest == null)
        //        {
        //            return BadRequest();
        //        }
        //        var postedFile = httpRequest.Form.Files["Image"];
        //        var fileName = httpRequest.Form.Files["Image"].FileName;
        //        var fileExtention = Path.GetExtension(fileName);
        //        var image = new byte[postedFile.Length];
        //        var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
        //        var user = await _userManger.FindByIdAsync(currentUserId);
        //        var response = await _compRepo.UploadCompanyStructureDefinitionAsync(image, user.UserName);

        //        if (!response)
        //        {
        //            return Ok( new CompanyRespObj {
        //                Status = new APIResponseStatus
        //                {
        //                    IsSuccessful = true,
        //                    Message = new APIResponseMessage
        //                    {
        //                        FriendlyMessage = "Record uploaded successfully"
        //                    }
        //                }
        //            });
        //        }
        //        return BadRequest(new CompanyRespObj
        //        {
        //            Status = new APIResponseStatus
        //            {
        //                IsSuccessful = true,
        //                Message = new APIResponseMessage
        //                {
        //                    FriendlyMessage = "Failure uploading record"
        //                }
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

         
        #endregion
        
        #region Company structure



        [HttpGet(ApiRoutes.ComapnyEndPoints.GET_ALL_COMPANY_STRUCTURE_BY_ACCESSID)]
        public async Task<ActionResult<CompanyStructureDefinitionRegRespObj>> GetCompanyByAccesId([FromQuery] CompanyStructureDefinitionSearchObj request)
        {
            try
            {
                if (request.AccessId > 0)
                {
                    return Ok(await _compRepo.GetCompanyStructureByAccessIdAsync(request.AccessId));
                }
                return BadRequest(new CompanyStructureDefinitionRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Access ID required"
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : GetCompanyByAccesId{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CompanyStructureDefinitionRegRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process request",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : GetCompanyByAccesId{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        } 
      
        [HttpPost(ApiRoutes.ComapnyEndPoints.ADD_UPDATE_COMPANY_STRUCTURE)]
        public async Task<IActionResult> ADD_UPDATE_COMPANY_STRUCTURE([FromForm] AddUpdateCompanyStructureCommand command)
        {
            var res = await _mediator.Send(command);
            if (res.Status.IsSuccessful)
                return Ok(res);
            return BadRequest(res);
        }

        [HttpPost(ApiRoutes.ComapnyEndPoints.DELETE_COMPANY_STRUCTURE)]
        public async Task<IActionResult> DeleteCompanyStructDefinition([FromBody] DeleteCompanyStructureCommand command)
        {
            var res = await _mediator.Send(command);
            if (res.Status.IsSuccessful)
                return Ok(res);
            return BadRequest(res);
        }
        [AllowAnonymous]
        [HttpGet(ApiRoutes.ComapnyEndPoints.GET_ALL_COMPANY_STRUCTURE)]
        public async Task<IActionResult> GET_ALL_COMPANY_STRUCTURE()
        {
            var query = new GetAllCompanyStructureQuery();
            var res = await _mediator.Send(query); 
            return Ok(res); 
        }


        [HttpPost(ApiRoutes.ComapnyEndPoints.UPDATE_COMPANY_STRUCTURE)]
        public async Task<IActionResult> UPDATE_COMPANY_STRUCTURE([FromBody] UpdateCompanyStructureCommand command)
        { 
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        //[HttpPost(ApiRoutes.ComapnyEndPoints.ADD_UPDATE_ADD_COMPANY_STRUCTURE_INFO)]
        //public async Task<IActionResult> AddUpCompStrucInfor([FromBody] AddUpdateCompanyStructureInfoCommand command)
        //{
        //    var res = await _mediator.Send(command);
        //    if (res.Status.IsSuccessful)
        //        return Ok(res);
        //    return BadRequest(res);
        //}

        [HttpGet(ApiRoutes.ComapnyEndPoints.GET_COMPANY_STRUCTURE)]
        public async Task<IActionResult> GET_COMPANY_STRUCTURE_BY_DEF(GetCompanyStructureQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.ComapnyEndPoints.GET_COMPANY_STRUCTURE_BY_DEF)]
        public async Task<IActionResult> GET_COMPANY_STRUCTURE_BY_DEF(GetCompanyStructureByDefinitionQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.ComapnyEndPoints.GET_COMPANY_STRUCTURE_INFO)]
        public async Task<IActionResult> GetCompanyStructureByDefinitionInfo([FromQuery] GetCompanyStructureByDefinitionInfoQuery query)
        {
            var res = await _mediator.Send(query);
            if (res.Status.IsSuccessful)
                return Ok(res);
            return BadRequest(res);
        }

        [HttpGet(ApiRoutes.ComapnyEndPoints.GET_COMPANY_STRUCTURE_BY_STAFFID)]
        public async Task<IActionResult> GetCompanyStructureByStaffId(GetCompanyStructureByStaffIdQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
 


        #endregion

        [AllowAnonymous]
        [HttpGet(ApiRoutes.ComapnyEndPoints.GET_COMPANY_EXCHANGE)]
        public async Task<IActionResult> GET_COMPANY_EXCHANGE([FromQuery]GetExchangeRateQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.ComapnyEndPoints.GET_ALL_COMPANY_STRUCTURE2)]
        public async Task<IActionResult> GET_ALL_COMPANY_STRUCTURE2()
        {
            var query = new GetCompanyStructureByDefinition2Query();
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        
        [HttpGet(ApiRoutes.ComapnyEndPoints.GET_COMPANY_STRUCTUREDEFINITION_EXCHANGE)]
        public async Task<IActionResult> GET_COMPANY_STRUCTUREDEFINITION_EXCHANGE([FromQuery]GetCompanyStructureDefinitionQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [ERPActivity(Action = UserActions.Add, Activity = 11)]
        [HttpPost(ApiRoutes.ComapnyEndPoints.UPLOAD_COMPANY_STRUCTURE)]
        public async Task<IActionResult> UPLOAD_COMPANY_STRUCTURE()
        {
            var query = new UploadCompanyStructureCommand();
            var res = await _mediator.Send(query);
            if(!res.Status.IsSuccessful)
                return BadRequest(res);
            return Ok(res);
        }

        [ERPActivity(Action = UserActions.View, Activity = 11)]
        [HttpGet(ApiRoutes.ComapnyEndPoints.DOWNLOAD_COMPANY_STRUCTURE)]
        public async Task<IActionResult> DOWNLOAD_COMPANY_STRUCTURE()
        {
            var query = new DownloadCompanyStructureQuery();
            return Ok(await _mediator.Send(query));
        }

        //UploadCompanyStructureDefinitionCommand
        //DownloadCompanyDefinitionQuery
        [ERPActivity(Action = UserActions.Add, Activity = 10)]
        [HttpPost(ApiRoutes.ComapnyEndPoints.UPLOAD_COMPANY_STRUCTURE_DEF)]
        public async Task<IActionResult> UPLOAD_COMPANY_STRUCTURE_DEF()
        {
            var query = new UploadCompanyStructureCommand();
            var res = await _mediator.Send(query);
            if (!res.Status.IsSuccessful)
                return BadRequest(res);
            return Ok(res);
        }

        [ERPActivity(Action = UserActions.View, Activity = 10)]
        [HttpGet(ApiRoutes.ComapnyEndPoints.GENERATE_EXCEL_FOR_COMP_STRU_DEF)]
        public async Task<IActionResult> GENERATE_EXCEL_FOR_COMP_STRU_DEF()
        {
            var query = new DownloadCompanyStructureQuery();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.ComapnyEndPoints.GET_COMPANY_STRUCTURE_FSTEMPLATE)]
        public async Task<Fstemplate> GET_COMPANY_STRUCTURE_FSTEMPLATE([FromQuery] GetFsTemplateQuery request)
        {
            try
            {
                var response = new Fstemplate { Status = new APIResponseStatus { Message = new APIResponseMessage() } };
                if (request.CompanyStructureId < 1)
                {
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = "Company Id Required";
                    return response;
                }
                var tmp = await _compRepo.GetCompanyStructureAsync(request.CompanyStructureId);
                var file = tmp?.FSTemplate ?? null;
                var name = tmp?.FSTemplateName; 

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/"+ tmp.FSTemplate);
                byte[] bytes = System.IO.File.ReadAllBytes(path);
                return new Fstemplate
                {
                    FsTemplate = bytes,
                    FsTempalteName = name,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                    }
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
           // return Ok(await _mediator.Send(query));
        }

         
        [HttpPost(ApiRoutes.ComapnyEndPoints.UPLOAD_FS_TEMPLATE)]
        public async Task<IActionResult> UPLOAD_FS_TEMPLATE()
        {
            var filer = new FSTempalteUploader { Status = new APIResponseStatus { Message = new APIResponseMessage() } }; 
            try
            {
                var uploader = _httpContextAccessor.HttpContext.Request.Form.Files["Image"];
                var companyId = _httpContextAccessor.HttpContext.Request.Form["CompanyId"];
                if (string.IsNullOrWhiteSpace(companyId))
                {
                    filer.Status.IsSuccessful = false;
                    filer.Status.Message.FriendlyMessage = "Unable to Identify company";
                    //return BadRequest(filer);
                }
                if (uploader == null)
                {
                    filer.Status.IsSuccessful = false;
                    filer.Status.Message.FriendlyMessage = "Unable to Identify File";
                    // BadRequest(filer);
                }
                if (uploader.Length > 0)
                {
                    if (!Directory.Exists(_env.WebRootPath + "/Files"))
                    {
                        Directory.CreateDirectory(_env.WebRootPath + "/Files");
                    }
                }
                using (FileStream filestrem = System.IO.File.Create(_env.WebRootPath + "/Files/" + uploader.FileName))
                {
                    await uploader.CopyToAsync(filestrem);
                    await filestrem.FlushAsync();
                   
                }
                var comp = await _compRepo.GetCompanyStructureAsync(int.Parse(companyId));
                if (comp != null)
                {
                    comp.FSTemplate = "/Files/" + uploader.FileName;
                    comp.CompanyStructureId =  int.Parse(companyId);
                    comp.FSTemplateName = uploader.FileName;
                    await _compRepo.AddUpdateCompanyStructureAsync(comp);
                }
                filer.Status.IsSuccessful = true; 

                return Ok(filer);
            }
            catch (IOException ex)
            {
                throw ex;
            }
            
        }
    }
}
