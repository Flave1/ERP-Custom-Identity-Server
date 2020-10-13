using APIGateway.Contracts.Response.Workflow;
using APIGateway.Data;
using APIGateway.DomainObjects.Workflow;
using GOSLibraries.Enums;
using APIGateway.Extensions;
using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.DomainObjects.Operation;
using GODP.APIsContinuation.DomainObjects.Workflow; 
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Repository.Inplimentation.Workflow
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly DataContext _dataContext;
        public WorkflowRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        #region Workflow group
        public async Task<bool> AddUpdateWorkflowGroupAsync(cor_workflowgroup model)
        {
            if (model.WorkflowGroupId > 0)
            {
                var item = await _dataContext.cor_workflowgroup.FindAsync(model.WorkflowGroupId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_workflowgroup.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteWorkflowGroupAsync(int workflowGroupId)
        {
            var item = await _dataContext.cor_workflowgroup.FindAsync(workflowGroupId);
            _dataContext.cor_workflowgroup.Remove(item);
            return await _dataContext.SaveChangesAsync() > 0;
        }


        public async Task<byte[]> GenerateExportWorkflowGroupAsync()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Workflow Group Name");

            var statementType = await (from a in _dataContext.cor_workflowgroup
                                       where a.Deleted == false
                                       select new cor_workflowgroup
                                       {
                                           WorkflowGroupId = a.WorkflowGroupId,
                                           WorkflowGroupName = a.WorkflowGroupName,

                                       }).ToListAsync();

            foreach (var item in statementType)
            {
                var row = dt.NewRow();
                row["Workflow Group Name"] = item.WorkflowGroupName;

                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (statementType != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("WorkflowGroup");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }


        public async Task<IEnumerable<cor_operation>> GetAllOperationAsync()
        {
            return await _dataContext.cor_operation.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_operationtype>> GetAllOperationTypesAsync()
        {
            return await _dataContext.cor_operationtype.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_workflowgroup>> GetAllWorkflowGroupAsync()
        {
            return await _dataContext.cor_workflowgroup.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<cor_workflowgroup> GetWorkflowGroupAsync(int workflowGroupId)
        {
            return await _dataContext.cor_workflowgroup.FirstOrDefaultAsync(x => x.Deleted == false && x.WorkflowGroupId == workflowGroupId);
        }

        public async Task<bool> UploadWorkflowGroupAsync(byte[] record, string createdBy)
        {
            try
            { 
                List<cor_workflowgroup> uploadedRecord = new List<cor_workflowgroup>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                { 
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;

                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new cor_workflowgroup
                        {
                            WorkflowGroupName = workSheet.Cells[i, 1].Value.ToString()

                        });
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var row in uploadedRecord)
                    {

                        //var workflowGroupId = _context.cor_workflowgroup.Where(x => x.WorkflowGroupName == entity.workflowGroupName).FirstOrDefault().WorkflowGroupId;

                        var accountNumber = RandomCharacters.GenerateByAnyLength(10);
                        var accountTypeExist = await _dataContext.cor_workflowgroup.FirstOrDefaultAsync(x => x.WorkflowGroupName.ToLower() == row.WorkflowGroupName.ToLower());
                        if (accountTypeExist != null)
                        {
                            //accountTypeExist.WorkflowGroupId = entity.workflowGroupId;
                            accountTypeExist.WorkflowGroupName = row.WorkflowGroupName;
                            accountTypeExist.Active = true;
                            accountTypeExist.Deleted = false;
                            accountTypeExist.UpdatedBy = row.CreatedBy;
                            accountTypeExist.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var accountType = new cor_workflowgroup
                            {
                                WorkflowGroupId = row.WorkflowGroupId,
                                WorkflowGroupName = row.WorkflowGroupName,
                                Active = true,
                                Deleted = false,
                                UpdatedBy = row.CreatedBy,
                                UpdatedOn = DateTime.Now,
                            };
                            await _dataContext.cor_workflowgroup.AddAsync(accountType);
                        }
                    }
                }
                var response = await _dataContext.SaveChangesAsync() > 0;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> WorkflowGroupExistAsync(string name)
        {
            return await _dataContext.cor_workflowgroup.AnyAsync(x => x.WorkflowGroupName.Trim().ToLower() == name.Trim().ToLower());
        }
        #endregion

        #region Workflow level

        public async Task<bool> AddUpdateWorkflowLevelAsync(cor_workflowlevel model)
        {
            if (model.WorkflowLevelId > 0)
            {
                var item = await _dataContext.cor_workflowlevel.FindAsync(model.WorkflowLevelId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_workflowlevel.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteWorkflowLevelAsync(int workflowLevelId)
        {
            var item = await _dataContext.cor_workflowlevel.FindAsync(workflowLevelId);
            _dataContext.cor_workflowlevel.Remove(item);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<byte[]> GenerateExportWorkflowLevelAsync()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Workflow Level Name");
            dt.Columns.Add("Workflow Group Name");
            dt.Columns.Add("Position");
            dt.Columns.Add("Required Limit");
            dt.Columns.Add("Limit Amount");
            dt.Columns.Add("Can Modify");

            var statementType = await (from a in _dataContext.cor_workflowlevel where a.Deleted == false
                                       select new WorkflowLevelObj
                                       {
                                           WorkflowLevelName = a.WorkflowLevelName,
                                           WorkflowLevelId = a.WorkflowLevelId,
                                           WorkflowGroupId = a.WorkflowGroupId,
                                           WorkflowGroupName = _dataContext.cor_workflowgroup.FirstOrDefault(x => x.WorkflowGroupId == a.WorkflowGroupId).WorkflowGroupName,
                                           Position = a.Position,
                                           RequiredLimit = a.RequiredLimit,
                                           LimitAmount = a.LimitAmount,
                                           JobTitleId = a.RoleId,
                                           CanModify = a.CanModify

                                       }).ToListAsync();


            foreach (var srow in statementType)
            {
                var row = dt.NewRow();
                row["Workflow Level Name"] = srow.WorkflowLevelName;
                row["Workflow Group Name"] = srow.WorkflowGroupName;
                row["Position"] = srow.Position;
                row["Required Limit"] = srow.RequiredLimit;
                row["Limit Amount"] = srow.LimitAmount == null ? 0 : srow.LimitAmount;
                row["Can Modify"] = srow.CanModify;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (statementType != null)
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
        public async Task<IEnumerable<cor_workflowlevel>> GetAllWorkflowLevelAsync()
        {
            return await _dataContext.cor_workflowlevel.Where(x => x.Deleted == false).ToListAsync();
        }
        public async Task<cor_workflowlevel> GetWorkflowLevelAsync(int workflowLevelId)
        {
            return await _dataContext.cor_workflowlevel.FirstOrDefaultAsync(x => x.Deleted == false && x.WorkflowLevelId == workflowLevelId);
        }
        public async Task<IEnumerable<cor_workflowlevel>> GetWorkflowLevelsByWorkflowGroupAsync(int workflowGroupId)
        {
            return await _dataContext.cor_workflowlevel.Where(x => x.Deleted == false && x.WorkflowLevelId == workflowGroupId).ToListAsync();
        }
        public async Task<bool> UploadWorkflowLevelAsync(byte[] record, string createdBy)
        {

            try
            {
                if (record == null) return false;
                List<WorkflowLevelObj> uploadedRecord = new List<WorkflowLevelObj>();
                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new WorkflowLevelObj
                        {
                            WorkflowLevelName = workSheet.Cells[i, 1].Value.ToString(),
                            WorkflowGroupName = workSheet.Cells[i, 2].Value.ToString(),
                            Position = int.Parse(workSheet.Cells[i, 3].Value.ToString()),
                            RequiredLimit = bool.Parse(workSheet.Cells[i, 4].Value.ToString()),
                            LimitAmount = decimal.Parse(workSheet.Cells[i, 5].Value.ToString()),
                            CanModify = bool.Parse(workSheet.Cells[i, 6].Value.ToString())

                        });
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var entity in uploadedRecord)
                    {
                        var workflowLevel = _dataContext.cor_workflowlevel.Where(x => x.WorkflowLevelName == entity.WorkflowLevelName).FirstOrDefault();
                        var workflowGroup = _dataContext.cor_workflowgroup.Where(x => x.WorkflowGroupName == entity.WorkflowGroupName).FirstOrDefault();
                        if (workflowLevel == null)
                        {
                            entity.WorkflowLevelId = 0;
                        }
                        else
                        {
                            entity.WorkflowLevelId = workflowLevel.WorkflowLevelId;
                        }

                        if (workflowGroup == null)
                            throw new Exception("Please upload a valid workflowgroup name");

                        var accountTypeExist = _dataContext.cor_workflowlevel.Find(entity.WorkflowLevelId);
                        if (accountTypeExist != null)
                        {
                            accountTypeExist.WorkflowLevelId = workflowLevel.WorkflowLevelId;
                            accountTypeExist.WorkflowLevelName = entity.WorkflowLevelName;
                            accountTypeExist.WorkflowGroupId = workflowGroup.WorkflowGroupId;
                            accountTypeExist.Position = entity.Position;
                            accountTypeExist.RequiredLimit = entity.RequiredLimit;
                            accountTypeExist.LimitAmount = entity.LimitAmount;
                            accountTypeExist.RoleId = entity.JobTitleId;
                            accountTypeExist.CanModify = entity.CanModify;
                            accountTypeExist.Active = true;
                            accountTypeExist.Deleted = false;
                            accountTypeExist.UpdatedBy = entity.CreatedBy;
                            accountTypeExist.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var accountType = new cor_workflowlevel
                            {
                                CreatedBy = entity.WorkflowGroupName,
                                //WorkflowLevelId = workflowLevel.WorkflowLevelId,
                                WorkflowLevelName = entity.WorkflowLevelName,
                                WorkflowGroupId = workflowGroup.WorkflowGroupId,
                                Position = entity.Position,
                                RequiredLimit = entity.RequiredLimit,
                                LimitAmount = entity.LimitAmount,
                                RoleId = entity.JobTitleId,
                                CanModify = entity.CanModify,
                                Active = true,
                                Deleted = false,
                                UpdatedBy = entity.CreatedBy,
                                UpdatedOn = DateTime.Now,
                            };
                            await _dataContext.cor_workflowlevel.AddAsync(accountType);
                        }
                    }
                }
                var response = _dataContext.SaveChanges() > 0;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Workflow level staff

        public async Task<bool> AddUpdateWorkflowLevelStaffAsync(cor_workflowlevelstaff model)
        {
            if (model.WorkflowLevelStaffId > 0)
            {
                var item = await _dataContext.cor_workflowlevelstaff.FindAsync(model.WorkflowLevelStaffId); 
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_workflowlevelstaff.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<cor_workflowlevelstaff>> GetAllWorkflowLevelStaffAsync()
        {
            return await _dataContext.cor_workflowlevelstaff.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<cor_workflowlevelstaff> GetWorkflowLevelStaffAsync(int workflowLevelStaffId)
        {
            return await _dataContext.cor_workflowlevelstaff.FirstOrDefaultAsync(x => x.Deleted == false && x.WorkflowLevelStaffId == workflowLevelStaffId);
        }

        public async Task<bool> DeleteWorkflowLevelStaffAsync(int workflowLevelStaffId)
        {
            var item = await _dataContext.cor_workflowlevelstaff.FindAsync(workflowLevelStaffId);
            _dataContext.cor_workflowlevelstaff.Remove(item);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<cor_workflowlevelstaff> GetWorkflowLevelStaffsByStaffAsync(int staffId)
        {
            return await _dataContext.cor_workflowlevelstaff.FirstOrDefaultAsync(x => x.Deleted == false && x.StaffId == staffId);
        }

        public async Task<IEnumerable<cor_workflowlevelstaff>> GetWorkflowLevelStaffsByWorkflowLevelAsync(int workflowLevelId)
        {
            return await _dataContext.cor_workflowlevelstaff.Where(x => x.Deleted == false && x.WorkflowLevelId == workflowLevelId).ToListAsync();
        }

        public async Task<byte[]> GenerateExportWorkflowLevelStaffAsync()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Workflow Level Name");
            dt.Columns.Add("Workflow Group Name");
            dt.Columns.Add("Staff Name");
            dt.Columns.Add("Staff Code");
            dt.Columns.Add("Access Level");

            var staffs = await (from a in _dataContext.cor_workflowlevelstaff
                                join b in _dataContext.cor_staff on a.StaffId equals b.StaffId
                                join c in _dataContext.cor_workflowlevel on a.WorkflowLevelId equals c.WorkflowLevelId
                                where a.Deleted == false
                                select new WorkflowlevelStaffObj
                                {
                                    WorkflowLevelStaffId = a.WorkflowLevelStaffId,
                                    WorkflowLevelId = c.WorkflowLevelId,
                                    WorkflowLevelName = c.WorkflowLevelName,
                                    WorkflowGroupId = a.WorkflowGroupId,
                                    WorkflowGroupName = a.cor_workflowgroup.WorkflowGroupName,
                                    StaffId = a.StaffId,
                                    StaffName = b.FirstName + " " + b.MiddleName + " " + b.LastName,
                                    StaffCode = b.StaffCode,
                                    AccessLevel = _dataContext.cor_companystructuredefinition.FirstOrDefault(o => o.StructureDefinitionId == b.AccessLevel).Definition
                                }).ToListAsync();


            foreach (var kk in staffs)
            {
                var row = dt.NewRow();
                row["Workflow Level Name"] = kk.WorkflowLevelName;
                row["Workflow Group Name"] = kk.WorkflowGroupName;
                row["Staff Name"] = kk.StaffName;
                row["Staff Code"] = kk.StaffCode;
                row["Access Level"] = kk.AccessLevel;

                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (staffs != null)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("WorkflowLevelStaff");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

        public async Task<bool> UploadWorkflowLevelStaffAsync(byte[] record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                List<WorkflowlevelStaffObj> uploadedRecord = new List<WorkflowlevelStaffObj>();
                using (MemoryStream stream = new MemoryStream(record))
                using (ExcelPackage excelPackage = new ExcelPackage(stream))
                {
                    //Use first sheet by default
                    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[1];
                    int totalRows = workSheet.Dimension.Rows;
                    //First row is considered as the header
                    for (int i = 2; i <= totalRows; i++)
                    {
                        uploadedRecord.Add(new WorkflowlevelStaffObj
                        {
                            WorkflowLevelName = workSheet.Cells[i, 1].Value.ToString(),
                            WorkflowGroupName = workSheet.Cells[i, 2].Value.ToString(),
                            StaffCode = workSheet.Cells[i, 3].Value.ToString(),
                        });
                    }
                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var entity in uploadedRecord)
                    {

                        var workflowGroup = _dataContext.cor_workflowgroup.Where(x => x.WorkflowGroupName == entity.WorkflowGroupName).FirstOrDefault();
                        var workflowLevel = _dataContext.cor_workflowlevel.Where(x => x.WorkflowLevelName == entity.WorkflowLevelName).FirstOrDefault();
                        var staff = _dataContext.cor_staff.Where(x => x.StaffCode == entity.StaffCode).FirstOrDefault();
                        if (workflowGroup == null)
                        {
                            throw new Exception("Please upload a valid workflowGroup name");
                        }
                        if (workflowLevel == null)
                        {
                            throw new Exception("Please upload a valid workflowLevel name");
                        }
                        if (staff == null)
                        {
                            throw new Exception("Please upload a valid staffcode");
                        }

                        var accountTypeExist = _dataContext.cor_workflowlevelstaff.Find(entity.WorkflowLevelStaffId);
                        if (accountTypeExist != null)
                        {
                            accountTypeExist.WorkflowLevelStaffId = entity.WorkflowLevelStaffId;
                            accountTypeExist.WorkflowLevelId = workflowLevel.WorkflowLevelId;
                            accountTypeExist.WorkflowGroupId = workflowGroup.WorkflowGroupId;
                            accountTypeExist.StaffId = staff.StaffId;
                            accountTypeExist.Active = true;
                            accountTypeExist.Deleted = false;
                            accountTypeExist.UpdatedBy = entity.CreatedBy;
                            accountTypeExist.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            var accountType = new cor_workflowlevelstaff
                            {
                                WorkflowLevelStaffId = entity.WorkflowLevelStaffId,
                                WorkflowLevelId = workflowLevel.WorkflowLevelId,
                                WorkflowGroupId = workflowGroup.WorkflowGroupId,
                                StaffId = staff.StaffId,
                                Active = true,
                                Deleted = false,
                                CreatedBy = entity.CreatedBy,
                                CreatedOn = DateTime.Now,
                            };
                            _dataContext.cor_workflowlevelstaff.Add(accountType);
                        }
                    }
                }
                var response = await _dataContext.SaveChangesAsync() > 0;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion

        #region Workflow 

        public async Task<IEnumerable<cor_workflow>> GetWorkflowByOperationAsync(int operationId)
        {
            var data = await _dataContext.cor_workflow.Where(x => x.Deleted == false).ToListAsync();
            return data.Where(x => x.OperationId == operationId);
        }

        public async Task<cor_workflow> GetSingleWorkflowAsync(int workflowId)
        {
            var queryAble = _dataContext.cor_workflow.AsQueryable();
            var data = await queryAble.FirstOrDefaultAsync(x => x.Deleted == false && x.WorkflowId == workflowId);
            return data;
        }

        public async Task<bool> AddUpdateWorkflowAsync(cor_workflow model)
        {
            if (model.WorkflowId > 0)
            {
                var item = await _dataContext.cor_workflow.FindAsync(model.WorkflowId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_workflow.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> CheckIfOperationAndAccessExistAsync(int operationId, int[] accessLevelIds)
        {
            return await _dataContext.cor_workflowaccess.AnyAsync(x => x.OperationId == operationId && accessLevelIds.Contains(x.OfficeAccessId) && x.Deleted == false);
        }

        public bool DeleteWorkflowAsync(int workflowId)
        {
            var access = _dataContext.cor_workflowaccess.Where(x => x.WorkflowId == workflowId).ToList();
            foreach (var acc in access)
            {
                if(acc != null)
                {
                    _dataContext.cor_workflowaccess.Remove(acc);
                } 
            }
            var details = _dataContext.cor_workflowdetails.Where(x => x.WorkflowId == workflowId).ToList();
            foreach (var det in details)
            {
                if(det != null)
                {
                    _dataContext.cor_workflowdetails.Remove(det);
                } 
            }
            
            var task = _dataContext.cor_workflowtask.Where(x => x.WorkflowId == workflowId).ToList();
            foreach (var tas in task)
            {
                if(tas != null)
                {
                    _dataContext.cor_workflowtask.Remove(tas);
                }
                
            }

            var item = _dataContext.cor_workflow.Find(workflowId);
            if(item != null)
            {
                _dataContext.cor_workflow.Remove(item);
            } 
            return _dataContext.SaveChanges() > 0;

        }

        #endregion

        public class cor_workflowdetailsTemporalObj
        {
            public int WorkflowDetailId { get; set; }
            public string CreatedBy { get; set; }
            public int[] AccessOfficeIds { get; set; }
        }
        public async Task<bool> AddWorkFlowDetailsAccessAsync()
        {
            try
            {
                bool response = false;
                List<cor_workflowdetailsTemporalObj> wkfDetailAccess = new List<cor_workflowdetailsTemporalObj>();
                var details = (from a in _dataContext.cor_workflowdetails where a.Active == false && a.Deleted == false select a).ToList();
                if (details.Count > 0)
                {
                    foreach (var item in details)
                    {
                        int n;
                        var access = new cor_workflowdetailsTemporalObj
                        {
                            WorkflowDetailId = item.WorkflowDetailId,
                            CreatedBy = item.CreatedBy,
                            AccessOfficeIds = item.OfficeAccessIds.Split(',').Select(s => int.TryParse(s, out n) ? n : 0).ToArray()
                        };
                        item.Active = true;
                        wkfDetailAccess.Add(access);
                    };
                    if (wkfDetailAccess.Count > 0)
                    {
                        foreach (var subitem in wkfDetailAccess)
                        {
                            foreach (var item in subitem.AccessOfficeIds)
                            {
                                if (item > 0)
                                {
                                    var subaccess = new cor_workflowdetailsaccess
                                    {
                                        WorkflowDetailId = subitem.WorkflowDetailId,
                                        OfficeAccessId = item,
                                        Active = true,
                                        Deleted = false,
                                        CreatedBy = subitem.CreatedBy,
                                        CreatedOn = DateTime.Now,
                                        UpdatedBy = subitem.CreatedBy,
                                        UpdatedOn = DateTime.Now,
                                    };
                                    await _dataContext.cor_workflowdetailsaccess.AddAsync(subaccess);
                                }
                            }
                        }
                        response = await _dataContext.SaveChangesAsync() > 0;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<IEnumerable<cor_operation>> GetAllWorkflowOperationAsync()
        {
            return await _dataContext.cor_operation.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<bool> UpdateWorkflowOperationAsync(List<cor_operation> entity)
        {

            foreach (var item in entity)
            {
                var operation = _dataContext.cor_operation.Find(item.OperationId);
                if (operation != null)
                {
                    operation.EnableWorkflow = (bool)item.EnableWorkflow;
                    operation.UpdatedOn = DateTime.Now;
                    _dataContext.Entry(operation).CurrentValues.SetValues(operation);
                }
              
            }
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<cor_workflowaccess>> GetAllWorkflowAccessAsync()
        {
            return await _dataContext.cor_workflowaccess.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_workflowdetails>> GetAllWorkflowdetailsAsync()
        {
            return await _dataContext.cor_workflowdetails.Where(d => d.Deleted == false).ToListAsync();
        }

        public async Task<bool> CreateUpdateWorkflowTaskAsync(cor_workflowtask model)
        {
            if (model.WorkflowTaskId > 0)
            {
                var item = await _dataContext.cor_workflowtask.FindAsync(model.WorkflowTaskId);
                _dataContext.Entry(item).CurrentValues.SetValues(model);
            }
            else
                await _dataContext.cor_workflowtask.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<cor_workflowtask>> GetAllWorkflowTaskAsync()
        {
            return await _dataContext.cor_workflowtask.Where(d => d.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_workflowtask>> GetCurrentWorkTaskAsync(int operationId, int targetId, int workflowId, string token)
        {
            var res = await GetAllWorkflowTaskAsync();
            return res.Where(x => 
            x.OperationId == operationId && 
            targetId == x.TargetId && 
            x.WorkflowId == workflowId && 
            x.WorkflowToken.Trim().ToLower() == token.Trim().ToLower()).ToList();
        }

        public async Task<IEnumerable<cor_workflowtask>> GetAnApproverWorkflowTaskAsync(string email, int accessId)
        {
            var data = await _dataContext.cor_workflowtask.Where(d => 
            d.WorkflowTaskStatus == (int)WorkflowTaskStatus.Created &&
            d.StaffAccessId == accessId &&
            d.IsMailSent == true).ToListAsync();

            var taskByStaffEmail = data.Where(d => d.StaffEmail.Trim().ToLower().Split(',').ToList().Contains(email.Trim().ToLower())).ToList();
            return taskByStaffEmail;
        }

        public async Task<IEnumerable<cor_workflowdetails>> GetWorkflowdetailsAsync(int workflowId)
        {
            return await _dataContext.cor_workflowdetails.Where(d => d.WorkflowId == workflowId && d.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_workflow>> GetAllWorkflowAsync()
        {
            return await _dataContext.cor_workflow.Where(d => d.Deleted == false).ToListAsync();
        }

        public async Task<IEnumerable<cor_workflowdetailsaccess>> GetAllWorkflowdetailAccessAsync()
        {
            return await _dataContext.cor_workflowdetailsaccess.Where(d => d.Deleted == false).ToListAsync();
        }
    }
       
}
