using APIGateway.Contracts.Commands.Workflow;
using APIGateway.Contracts.Response.Workflow;
using APIGateway.Repository.Interface.Common;
using APIGateway.Repository.Interface.Workflow;
using MediatR;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class GenerateExportWorkflowLevelCommandHandler : IRequestHandler<GenerateExportWorkflowLevelCommand, byte[]>
    {
        private readonly IWorkflowRepository _repo;
        private readonly ICommonRepository _commonRepo;
        public GenerateExportWorkflowLevelCommandHandler(IWorkflowRepository repository, ICommonRepository commonRepository)
        {
            _commonRepo = commonRepository;
            _repo = repository;
        }
        public async Task<byte[]> Handle(GenerateExportWorkflowLevelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Workflow Level Name");
                dt.Columns.Add("Workflow Group Name");
                dt.Columns.Add("Position");
                dt.Columns.Add("Job Title");
                dt.Columns.Add("Required Limit");
                dt.Columns.Add("Limit Amount");
                dt.Columns.Add("Can Modify"); 

                var _DomainList = await _repo.GetAllWorkflowLevelAsync();
                var _WorkflowGroupList = await _repo.GetAllWorkflowGroupAsync();
                var _Jobtitles = await _commonRepo.GetAllJobTitleAsync();

                var contractList = new List<WorkflowLevelObj>();
                contractList = _DomainList.Select(a => new WorkflowLevelObj
                {
                    WorkflowLevelName = a.WorkflowLevelName,
                    WorkflowLevelId = a.WorkflowLevelId, 
                    WorkflowGroupId = a.WorkflowGroupId,
                    WorkflowGroupName = _WorkflowGroupList.FirstOrDefault(x => x.WorkflowGroupId == a.WorkflowGroupId)?.WorkflowGroupName,
                    Position = a.Position,
                    RequiredLimit = a.RequiredLimit,
                    LimitAmount = a.LimitAmount,
                    JobTitleId = a.RoleId,
                    JobTitleName = _Jobtitles.FirstOrDefault(s => s.JobTitleId == (int.Parse(a.RoleId)))?.Name,
                    CanModify = a.CanModify
                }).ToList();
                  
                foreach (var srow in contractList)
                {
                    var row = dt.NewRow();
                    row["Workflow Level Name"] = srow.WorkflowLevelName;
                    row["Workflow Group Name"] = srow.WorkflowGroupName;
                    row["Position"] = srow.Position;
                    row["Job Title"] = srow.JobTitleName;
                    row["Required Limit"] = srow.RequiredLimit;
                    row["Limit Amount"] = srow.LimitAmount == null ? 0 : srow.LimitAmount;
                    row["Can Modify"] = (bool)srow.CanModify ? "Yes" : "No";
                    dt.Rows.Add(row);
                }

                Byte[] fileBytes = new Byte[0];

                if (contractList != null)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("WorkflowLevel");
                        ws.DefaultColWidth = 20;
                        ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                        fileBytes = pck.GetAsByteArray();
                    }
                }
                return fileBytes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
