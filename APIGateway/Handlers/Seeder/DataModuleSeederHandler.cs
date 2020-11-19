using APIGateway.AuthGrid;
using APIGateway.Data;
using APIGateway.DomainObjects.Credit;
using GODP.APIsContinuation.DomainObjects.Operation;
using GODP.APIsContinuation.DomainObjects.Others;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GOSLibraries.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace APIGateway.Handlers.Seeder
{
    public static class DataSeederHandler
    {
        public static void SeedActivitParents(DataContext context)
        {
            var operationtypes = new List<cor_activityparent>
            {
                     new cor_activityparent{ ActivityParentId = 1, ActivityParentName = "Admin", Deleted = false, Active = true, },
                      new cor_activityparent{ ActivityParentId = 2, ActivityParentName = "Organization ", Deleted = false, Active = true, },
                       new cor_activityparent{ ActivityParentId = 3, ActivityParentName = "General setup", Deleted = false, Active = true, },
                        new cor_activityparent{ ActivityParentId = 4, ActivityParentName = "Purchase and payables", Deleted = false, Active = true, }, 
                          new cor_activityparent{ ActivityParentId = 5, ActivityParentName = "Finance", Deleted = false, Active = true, }, 
                            new cor_activityparent{ ActivityParentId = 6, ActivityParentName = "Credit", Deleted = false, Active = true, },
                             new cor_activityparent{ ActivityParentId = 7, ActivityParentName = "Investor Fund", Deleted = false, Active = true, },
                              new cor_activityparent{ ActivityParentId = 8, ActivityParentName = "Treasury", Deleted = false, Active = true, },
                               new cor_activityparent{ ActivityParentId = 9, ActivityParentName = "PPE", Deleted = false, Active = true, },
                               new cor_activityparent{ ActivityParentId = 10, ActivityParentName = "Deposit", Deleted = false, Active = true, },

            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in operationtypes)
                    {
                        if (!context.cor_activityparent.Any(w => w.ActivityParentId == item.ActivityParentId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedOperationType(DataContext context)
        {
            var operationtypes = new List<cor_operationtype>
            {
                 
                    new cor_operationtype { OperationTypeId = 2, ModuleId = 4, OperationTypeName = "Purchase", Deleted = false, Active = true },
                    new cor_operationtype { OperationTypeId = 3, ModuleId = 6, OperationTypeName = "Loan Origination", Deleted = false, Active = true },
                    new cor_operationtype { OperationTypeId = 4, ModuleId = 5, OperationTypeName = "Finance", Deleted = false, Active = true },
                    new cor_operationtype { OperationTypeId = 5, ModuleId = 6, OperationTypeName = "Loan management", Deleted = false, Active = true },
                    new cor_operationtype { OperationTypeId = 6, ModuleId = 7, OperationTypeName = "InvestorFund", Deleted = false, Active = true },
                    new cor_operationtype { OperationTypeId = 7, ModuleId = 8, OperationTypeName = "Treasury", Deleted = false, Active = true },
                     new cor_operationtype { OperationTypeId = 8, ModuleId = 9, OperationTypeName = "PPE", Deleted = false, Active = true },
                     new cor_operationtype { OperationTypeId = 9, ModuleId = 10, OperationTypeName = "Deposit", Deleted = false, Active = true }
            }; 
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in operationtypes)
                    {
                        if(!context.cor_operationtype.Any(w => w.OperationTypeId == item.OperationTypeId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        } 
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedActivities(DataContext context)
        {
            var operationtypes = new List<cor_activity>
            {

                 new cor_activity{ ActivityId = 1, ActivityParentId = 1, ActivityName = "admin", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 2, ActivityParentId = 1, ActivityName = "user role", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 3, ActivityParentId = 1, ActivityName = "staff information", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 4, ActivityParentId = 1, ActivityName = "security", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 6, ActivityParentId = 1, ActivityName = "security setting", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 7, ActivityParentId = 1, ActivityName = "security questions", Deleted = false, Active = true, },


                 new cor_activity{ ActivityId = 8, ActivityParentId = 2, ActivityName = "organization", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 9, ActivityParentId = 2, ActivityName = "organization setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 10, ActivityParentId = 2, ActivityName = "company structure definition", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 11, ActivityParentId = 2, ActivityName = "company structure", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 12, ActivityParentId = 2, ActivityName = "company information", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 13, ActivityParentId = 2, ActivityName = "workflow setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 14, ActivityParentId = 2, ActivityName = "workflow group", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 15, ActivityParentId = 2, ActivityName = "workflow level", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 16, ActivityParentId = 2, ActivityName = "workflow activation", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 17, ActivityParentId = 2, ActivityName = "workflow staff", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 18, ActivityParentId = 2, ActivityName = "workflow", Deleted = false, Active = true, },


                 new cor_activity{ ActivityId = 19, ActivityParentId = 3, ActivityName = "general setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 20, ActivityParentId = 3, ActivityName = "country information", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 21, ActivityParentId = 3, ActivityName = "state information", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 22, ActivityParentId = 3, ActivityName = "city information", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 23, ActivityParentId = 3, ActivityName = "currency information", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 24, ActivityParentId = 3, ActivityName = "job title", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 25, ActivityParentId = 3, ActivityName = "currency rate", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 26, ActivityParentId = 3, ActivityName = "document type", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 27, ActivityParentId = 3, ActivityName = "identification information", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 28, ActivityParentId = 3, ActivityName = "credit bureau", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 29, ActivityParentId = 3, ActivityName = "email configuration", Deleted = false, Active = true, },

                 new cor_activity{ ActivityId = 30, ActivityParentId = 4, ActivityName = "purchase and payables", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 31, ActivityParentId = 4, ActivityName = "purchases setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 32, ActivityParentId = 4, ActivityName = "tax setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 33, ActivityParentId = 4, ActivityName = "service terms", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 34, ActivityParentId = 4, ActivityName = "supplier type", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 35, ActivityParentId = 4, ActivityName = "supplier information", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 36, ActivityParentId = 4, ActivityName = "approved suppliers", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 37, ActivityParentId = 4, ActivityName = "pending suppliers", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 38, ActivityParentId = 4, ActivityName = "supplier approval", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 39, ActivityParentId = 4, ActivityName = "purchase information", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 40, ActivityParentId = 4, ActivityName = "purchase requsitions", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 41, ActivityParentId = 4, ActivityName = "prn list", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 42, ActivityParentId = 4, ActivityName = "prn approval", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 43, ActivityParentId = 4, ActivityName = "bids", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 44, ActivityParentId = 4, ActivityName = "bids list", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 45, ActivityParentId = 4, ActivityName = "bids approval", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 46, ActivityParentId = 4, ActivityName = "local purchase order", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 47, ActivityParentId = 4, ActivityName = "lpo list", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 48, ActivityParentId = 4, ActivityName = "lpo approval", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 49, ActivityParentId = 4, ActivityName = "purchase payment", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 50, ActivityParentId = 4, ActivityName = "invoice", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 51, ActivityParentId = 4, ActivityName = "invoice list", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 52, ActivityParentId = 4, ActivityName = "payment approval", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 53, ActivityParentId = 4, ActivityName = "purchase report", Deleted = false, Active = true, },



                 new cor_activity{ ActivityId = 54, ActivityParentId = 5, ActivityName = "finance", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 55, ActivityParentId = 5, ActivityName = "finance setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 56, ActivityParentId = 5, ActivityName = "statement type", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 57, ActivityParentId = 5, ActivityName = "account type", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 58, ActivityParentId = 5, ActivityName = "general ledger", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 59, ActivityParentId = 5, ActivityName = "sub ledger", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 60, ActivityParentId = 5, ActivityName = "bank setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 61, ActivityParentId = 5, ActivityName = "financial year", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 62, ActivityParentId = 5, ActivityName = "registry", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 63, ActivityParentId = 5, ActivityName = "translation setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 64, ActivityParentId = 5, ActivityName = "payment setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 65, ActivityParentId = 5, ActivityName = "finance operations", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 66, ActivityParentId = 5, ActivityName = "gl mapping", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 67, ActivityParentId = 5, ActivityName = "remapping", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 68, ActivityParentId = 5, ActivityName = "journals", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 69, ActivityParentId = 5, ActivityName = "journal approval", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 70, ActivityParentId = 5, ActivityName = "gl transaction", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 71, ActivityParentId = 5, ActivityName = "trial balance", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 72, ActivityParentId = 5, ActivityName = "payments and receipts", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 73, ActivityParentId = 5, ActivityName = "finance report", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 74, ActivityParentId = 5, ActivityName = "profit & loss", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 75, ActivityParentId = 5, ActivityName = "changes in equities", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 76, ActivityParentId = 5, ActivityName = "excel report", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 77, ActivityParentId = 5, ActivityName = "financial position", Deleted = false, Active = true, },


                 new cor_activity{ ActivityId = 78, ActivityParentId = 6, ActivityName = "credit", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 79, ActivityParentId = 6, ActivityName = "credit dashboard", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 80, ActivityParentId = 6, ActivityName = "credit setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 81, ActivityParentId = 6, ActivityName = "fee setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 82, ActivityParentId = 6, ActivityName = "credit bureau", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 83, ActivityParentId = 6, ActivityName = "operating account setup", Deleted = false, Active = true, }, 
                 new cor_activity{ ActivityId = 84, ActivityParentId = 6, ActivityName = "loan staging", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 85, ActivityParentId = 6, ActivityName = "credit classification", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 86, ActivityParentId = 6, ActivityName = "exposure setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 87, ActivityParentId = 6, ActivityName = "product type setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 88, ActivityParentId = 6, ActivityName = "product setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 89, ActivityParentId = 6, ActivityName = "credit risk category setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 90, ActivityParentId = 6, ActivityName = "credit risk attribute setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 91, ActivityParentId = 6, ActivityName = "credit rating pd setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 92, ActivityParentId = 6, ActivityName = "credit risk rating set up", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 93, ActivityParentId = 6, ActivityName = "credit score card setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 94, ActivityParentId = 6, ActivityName = "customer fs setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 95, ActivityParentId = 6, ActivityName = "collateral setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 96, ActivityParentId = 6, ActivityName = "loan origination", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 97, ActivityParentId = 6, ActivityName = "loan customer", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 98, ActivityParentId = 6, ActivityName = "start loan application", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 99, ActivityParentId = 6, ActivityName = "pending applications", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 100, ActivityParentId = 6, ActivityName = "loan application list", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 101, ActivityParentId = 6, ActivityName = "credit appraisal", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 102, ActivityParentId = 6, ActivityName = "loan schedule", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 103, ActivityParentId = 6, ActivityName = "offer letter generation", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 104, ActivityParentId = 6, ActivityName = "offer letter review", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 105, ActivityParentId = 6, ActivityName = "loan booking", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 106, ActivityParentId = 6, ActivityName = "ifrs", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 107, ActivityParentId = 6, ActivityName = "ifrs data setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 108, ActivityParentId = 6, ActivityName = "scenario setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 109, ActivityParentId = 6, ActivityName = "macro-economic variables", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 110, ActivityParentId = 6, ActivityName = "historical pd", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 111, ActivityParentId = 6, ActivityName = "historical lgd", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 112, ActivityParentId = 6, ActivityName = "run impairment", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 113, ActivityParentId = 6, ActivityName = "loan management", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 114, ActivityParentId = 6, ActivityName = "loan review application", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 115, ActivityParentId = 6, ActivityName = "loan review appraisal", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 116, ActivityParentId = 6, ActivityName = "loan review offer letter", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 117, ActivityParentId = 6, ActivityName = "loan review operation", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 118, ActivityParentId = 6, ActivityName = "collateral management", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 119, ActivityParentId = 6, ActivityName = "customer transaction", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 120, ActivityParentId = 6, ActivityName = "loan repayment", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 121, ActivityParentId = 6, ActivityName = "payment due loans", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 122, ActivityParentId = 6, ActivityName = "overdues", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 123, ActivityParentId = 6, ActivityName = "credit reports", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 124, ActivityParentId = 6, ActivityName = "customer reports", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 125, ActivityParentId = 6, ActivityName = "loan reports", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 126, ActivityParentId = 6, ActivityName = "summary reports", Deleted = false, Active = true, },

                 new cor_activity{ ActivityId = 127, ActivityParentId = 7, ActivityName = "investment fund", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 128, ActivityParentId = 7, ActivityName = "investment fund dashboard", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 129, ActivityParentId = 7, ActivityName = "investment fund setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 130, ActivityParentId = 7, ActivityName = "product type setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 131, ActivityParentId = 7, ActivityName = "product setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 132, ActivityParentId = 7, ActivityName = "investment fund operation", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 133, ActivityParentId = 7, ActivityName = "customer", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 134, ActivityParentId = 7, ActivityName = "pending investments", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 135, ActivityParentId = 7, ActivityName = "investment list", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 136, ActivityParentId = 7, ActivityName = "investment appraisal", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 137, ActivityParentId = 7, ActivityName = "pending collections", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 138, ActivityParentId = 7, ActivityName = "collection appraisal", Deleted = false, Active = true, }, 
                 new cor_activity{ ActivityId = 139, ActivityParentId = 7, ActivityName = "pending rollover", Deleted = false, Active = true, }, 
                 new cor_activity{ ActivityId = 140, ActivityParentId = 7, ActivityName = "pending liquidations", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 141, ActivityParentId = 7, ActivityName = "liquidation appraisal", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 142, ActivityParentId = 7, ActivityName = "placement certificate", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 143, ActivityParentId = 7, ActivityName = "investment fund reports", Deleted = false, Active = true, },

                 new cor_activity{ ActivityId = 144, ActivityParentId = 8, ActivityName = "treasury", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 145, ActivityParentId = 8, ActivityName = "product type setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 146, ActivityParentId = 8, ActivityName = "product setup", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 147, ActivityParentId = 8, ActivityName = "tre investment list", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 148, ActivityParentId = 8, ActivityName = "tre investment appraisal", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 150, ActivityParentId = 8, ActivityName = "tre liquidation appraisal", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 151, ActivityParentId = 8, ActivityName = "tre placement certificate", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 152, ActivityParentId = 8, ActivityName = "treasury origination", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 153, ActivityParentId = 8, ActivityName = "issuer registration", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 154, ActivityParentId = 8, ActivityName = "tre collection appraisal", Deleted = false, Active = true, },

                 new cor_activity{ ActivityId = 155, ActivityParentId = 9, ActivityName = "ppe", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 156, ActivityParentId = 9, ActivityName = "ppe set up", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 157, ActivityParentId = 9, ActivityName = "asset classification", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 158, ActivityParentId = 9, ActivityName = "addition", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 159, ActivityParentId = 9, ActivityName = "addition appraisal", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 160, ActivityParentId = 9, ActivityName = "register", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 161, ActivityParentId = 9, ActivityName = "reassessment", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 162, ActivityParentId = 9, ActivityName = "reassessment appraisal", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 163, ActivityParentId = 9, ActivityName = "revaluation", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 164, ActivityParentId = 9, ActivityName = "revaluation appraisal", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 165, ActivityParentId = 9, ActivityName = "disposal", Deleted = false, Active = true, },
                 new cor_activity{ ActivityId = 166, ActivityParentId = 9, ActivityName = "disposal approval", Deleted = false, Active = true, },

                 new cor_activity{ ActivityId = 167, ActivityParentId = 10, ActivityName = "deposit", Deleted = false, Active = true, },


            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in operationtypes)
                    {
                        if (!context.cor_activity.Any(w => w.ActivityId == item.ActivityId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedOperation(DataContext context)
        {
            var opertaions = new List<cor_operation>
                {
                    new cor_operation { OperationId =  1, OperationName = "User Account Approval", OperationTypeId = 1, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  2, OperationName = "Staff Creation Approval", OperationTypeId = 1, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  3, OperationName = "Supplier Registration Approval", OperationTypeId = 2, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  4, OperationName = "Customer Registration Approval", OperationTypeId = 1, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  5, OperationName = "Purchase LPO Approval", OperationTypeId = 2, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  6, OperationName = "Purchase PRN Approval", OperationTypeId = 2, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  7, OperationName = "ProductDefinitionApproval", OperationTypeId = 3, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  8, OperationName = "Loan Customer Approval", OperationTypeId = 3, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  9, OperationName = "Loan Application Approval", OperationTypeId = 3, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  10, OperationName = "Loan Booking Approval", OperationTypeId = 3, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  11, OperationName = "Daily Interest Accural", OperationTypeId = 3, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  12, OperationName = "Interest Loan Repayment", OperationTypeId = 3, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  13, OperationName = "Principal Loan Repayment", OperationTypeId = 3, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  14, OperationName = "Customer Collateral Approval", OperationTypeId = 3, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  15, OperationName = "Casa Account Approval", OperationTypeId = 3, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  16, OperationName = "Loan Review Application", OperationTypeId = 5, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  17, OperationName = "Charge Reversal", OperationTypeId = 5, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  18, OperationName = "Principal Frequency Change", OperationTypeId = 5, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  19, OperationName = "Contractual Interest Rate Change", OperationTypeId = 5, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  20, OperationName = "Prepayment", OperationTypeId = 5, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  21, OperationName = "Principal Frequency Change", OperationTypeId = 5, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  22, OperationName = "Interest Frequency Change", OperationTypeId = 5, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  23, OperationName = "Interestand Principal Frequency Change", OperationTypeId = 5, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  24, OperationName = "Payment Date Change", OperationTypeId = 5, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  25, OperationName = "Tenor Change", OperationTypeId = 5, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  26, OperationName = "Loan Termination", OperationTypeId = 5, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  27, OperationName = "Restructured", OperationTypeId = 5, EnableWorkflow = false, Deleted = true, Active = true},
                    new cor_operation { OperationId =  28, OperationName = "Loan Disburment Transaction", OperationTypeId = 4, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  29, OperationName = "Deposit Form Submit", OperationTypeId = 4, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  30, OperationName = "Journal Entries", OperationTypeId = 4, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  31, OperationName = "Investor Fund Approval", OperationTypeId = 6, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  32, OperationName = "Liquidation Approval", OperationTypeId = 6, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  33, OperationName = "Collection Approval", OperationTypeId = 6, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  34, OperationName = "Treasury Investment Approval", OperationTypeId = 7, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  35, OperationName = "Treasury Liquidation Approval", OperationTypeId = 7, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  36, OperationName = "Treasury Collection Approval", OperationTypeId = 7, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  37, OperationName = "PPE Addition Approval", OperationTypeId = 8, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  38, OperationName = "Payment Approval", OperationTypeId = 2, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  39, OperationName = "PPEReassessment", OperationTypeId = 8, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  40, OperationName = "PPEDisposal", OperationTypeId = 8, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  41, OperationName = "BidAndTenders", OperationTypeId = 2, EnableWorkflow = true, Deleted = false, Active = true},
                    new cor_operation { OperationId =  42, OperationName = "PPERevaluation", OperationTypeId = 8, EnableWorkflow = true, Deleted = false, Active = true},
                     new cor_operation { OperationId =  43, OperationName = "Bank Account Closure", OperationTypeId = 9, EnableWorkflow = true, Deleted = false, Active = true},
                     new cor_operation { OperationId =  44, OperationName = "Change of Rate", OperationTypeId = 9, EnableWorkflow = true, Deleted = false, Active = true},

                }; 
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in opertaions)
                    {
                        if (!context.cor_operation.Any(w => w.OperationId == item.OperationId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedAuthenticationSecuritySetting(DataContext context)
        {
            var auth = new List<ScrewIdentifierGrid>
                {
                
                new ScrewIdentifierGrid
                    {
                        ScrewIdentifierGridId = 1,
                        Media = (int)Media.EMAIL,
                        Module = (int)Modules.CENTRAL,
                        ActiveDirectory = "www.godp.co.uk",
                        ActiveOnMobileApp = false,
                        ActiveOnWebApp = false,
                        EnableLoadBalance = false,
                        EnableLoginFailedLockout = false,
                        EnableRetryOnMobileApp = false,
                        EnableRetryOnWebApp = false,
                        InActiveSessionTimeout = TimeSpan.FromSeconds(600),
                        LoadBalanceInHours = 24,
                        NumberOfFailedLoginBeforeLockout = 5,
                        PasswordUpdateCycle = 24,
                        RetryTimeInMinutes = TimeSpan.FromMinutes(1),
                        SecuritySettingActiveOnMobileApp = false,
                        SecuritySettingsActiveOnWebApp = false,
                        ShouldAthenticate = false,
                        ShouldRetryAfterLockoutEnabled = false,
                        UseActiveDirectory = false,
                        NumberOfFailedAttemptsBeforeSecurityQuestions = 5
                    },
                  
                new ScrewIdentifierGrid
                    {
                        ScrewIdentifierGridId = 2,
                        Media = (int)Media.SMS,
                        Module = (int)Modules.CENTRAL,
                        ActiveDirectory = "www.godp.co.uk",
                        ActiveOnMobileApp = false,
                        ActiveOnWebApp = false,
                        EnableLoadBalance = false,
                        EnableLoginFailedLockout = false,
                        EnableRetryOnMobileApp = false,
                        EnableRetryOnWebApp = false,
                        InActiveSessionTimeout = TimeSpan.FromSeconds(600),
                        LoadBalanceInHours = 24,
                        NumberOfFailedLoginBeforeLockout = 5,
                        PasswordUpdateCycle = 24,
                        RetryTimeInMinutes = TimeSpan.FromMinutes(1),
                        SecuritySettingActiveOnMobileApp = false,
                        SecuritySettingsActiveOnWebApp = false,
                        ShouldAthenticate = false,
                        ShouldRetryAfterLockoutEnabled = false,
                        UseActiveDirectory = false,
                        NumberOfFailedAttemptsBeforeSecurityQuestions = 5
                    },

                   
                new ScrewIdentifierGrid
                    {
                        ScrewIdentifierGridId = 3,
                        Media = (int)Media.EMAIL,
                        Module = (int)Modules.CREDIT,
                        ActiveDirectory = "www.godp.co.uk",
                        ActiveOnMobileApp = false,
                        ActiveOnWebApp = false,
                        EnableLoadBalance = false,
                        EnableLoginFailedLockout = false,
                        EnableRetryOnMobileApp = false,
                        EnableRetryOnWebApp = false,
                        InActiveSessionTimeout = TimeSpan.FromSeconds(600),
                        LoadBalanceInHours = 24,
                        NumberOfFailedLoginBeforeLockout = 5,
                        PasswordUpdateCycle = 24,
                        RetryTimeInMinutes = TimeSpan.FromMinutes(1),
                        SecuritySettingActiveOnMobileApp = false,
                        SecuritySettingsActiveOnWebApp = false,
                        ShouldAthenticate = false,
                        ShouldRetryAfterLockoutEnabled = false,
                        UseActiveDirectory = false,
                        NumberOfFailedAttemptsBeforeSecurityQuestions = 5
                    },

                
                new ScrewIdentifierGrid
                    {
                        ScrewIdentifierGridId = 4,
                        Media = (int)Media.SMS,
                        Module = (int)Modules.CREDIT,
                        ActiveDirectory = "www.godp.co.uk",
                        ActiveOnMobileApp = false,
                        ActiveOnWebApp = false,
                        EnableLoadBalance = false,
                        EnableLoginFailedLockout = false,
                        EnableRetryOnMobileApp = false,
                        EnableRetryOnWebApp = false,
                        InActiveSessionTimeout = TimeSpan.FromSeconds(600),
                        LoadBalanceInHours = 24,
                        NumberOfFailedLoginBeforeLockout = 5,
                        PasswordUpdateCycle = 24,
                        RetryTimeInMinutes = TimeSpan.FromMinutes(1),
                        SecuritySettingActiveOnMobileApp = false,
                        SecuritySettingsActiveOnWebApp = false,
                        ShouldAthenticate = false,
                        ShouldRetryAfterLockoutEnabled = false,
                        UseActiveDirectory = false,
                        NumberOfFailedAttemptsBeforeSecurityQuestions = 5
                    },

                
                new ScrewIdentifierGrid
                    {
                        ScrewIdentifierGridId = 5,
                        Media = (int)Media.EMAIL,
                        Module = (int)Modules.PURCHASE_AND_PAYABLES,
                        ActiveDirectory = "www.godp.co.uk",
                        ActiveOnMobileApp = false,
                        ActiveOnWebApp = false,
                        EnableLoadBalance = false,
                        EnableLoginFailedLockout = false,
                        EnableRetryOnMobileApp = false,
                        EnableRetryOnWebApp = false,
                        InActiveSessionTimeout = TimeSpan.FromSeconds(600),
                        LoadBalanceInHours = 24,
                        NumberOfFailedLoginBeforeLockout = 5,
                        PasswordUpdateCycle = 24,
                        RetryTimeInMinutes = TimeSpan.FromMinutes(1),
                        SecuritySettingActiveOnMobileApp = false,
                        SecuritySettingsActiveOnWebApp = false,
                        ShouldAthenticate = false,
                        ShouldRetryAfterLockoutEnabled = false,
                        UseActiveDirectory = false,
                        NumberOfFailedAttemptsBeforeSecurityQuestions = 5
                    },

                 
                new ScrewIdentifierGrid
                    {
                        ScrewIdentifierGridId = 6,
                        Media = (int)Media.SMS,
                        Module = (int)Modules.PURCHASE_AND_PAYABLES,
                        ActiveDirectory = "www.godp.co.uk",
                        ActiveOnMobileApp = false,
                        ActiveOnWebApp = false,
                        EnableLoadBalance = false,
                        EnableLoginFailedLockout = false,
                        EnableRetryOnMobileApp = false,
                        EnableRetryOnWebApp = false,
                        InActiveSessionTimeout = TimeSpan.FromSeconds(600),
                        LoadBalanceInHours = 24,
                        NumberOfFailedLoginBeforeLockout = 5,
                        PasswordUpdateCycle = 24,
                        RetryTimeInMinutes = TimeSpan.FromMinutes(1),
                        SecuritySettingActiveOnMobileApp = false,
                        SecuritySettingsActiveOnWebApp = false,
                        ShouldAthenticate = false,
                        ShouldRetryAfterLockoutEnabled = false,
                        UseActiveDirectory = false,
                        NumberOfFailedAttemptsBeforeSecurityQuestions = 5
                    },

                };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in auth)
                    {
                        if (!context.ScrewIdentifierGrid.Any(w => w.ScrewIdentifierGridId == item.ScrewIdentifierGridId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedGender(DataContext context)
        {
            var genders = new List<cor_gender>
            {
                    new cor_gender { GenderId = 1,  Gender = "Male", Deleted = false, Active = true },
                    new cor_gender { GenderId = 2,  Gender = "Female", Deleted = false, Active = true },
            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in genders)
                    {
                        if (!context.cor_gender.Any(w => w.GenderId == item.GenderId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedMaritalStatus(DataContext context)
        {
            var status = new List<cor_maritalstatus>
            { 
                    new cor_maritalstatus { MaritalStatusId = 1,  Status = "Single", Deleted = false, Active = true },
                     new cor_maritalstatus { MaritalStatusId = 2,  Status = "Married", Deleted = false, Active = true },
                      new cor_maritalstatus { MaritalStatusId = 3,  Status = "Divorced", Deleted = false, Active = true },
                       new cor_maritalstatus { MaritalStatusId = 4,  Status = "Widow", Deleted = false, Active = true },
                        new cor_maritalstatus { MaritalStatusId = 5,  Status = "Widower", Deleted = false, Active = true }
            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in status)
                    {
                        if (!context.cor_maritalstatus.Any(w => w.MaritalStatusId == item.MaritalStatusId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedTitles(DataContext context)
        {
            var genders = new List<cor_title>
            {
                    new cor_title { TitleId = 1,  Title = "Mr.", Deleted = false, Active = true },
                    new cor_title { TitleId = 2,  Title = "Mrs.", Deleted = false, Active = true },
                    new cor_title { TitleId = 3,  Title = "Miss.", Deleted = false, Active = true },
                    new cor_title { TitleId = 4,  Title = "Dr.", Deleted = false, Active = true },
                    new cor_title { TitleId = 5,  Title = "Prof.", Deleted = false, Active = true },
                    new cor_title { TitleId = 6,  Title = "Chief", Deleted = false, Active = true },
                    new cor_title { TitleId = 7,  Title = "Alhaji", Deleted = false, Active = true },
                    new cor_title { TitleId = 8,  Title = "Alhaja", Deleted = false, Active = true },
                    new cor_title { TitleId = 9,  Title = "HRH", Deleted = false, Active = true },
                    new cor_title { TitleId = 10,  Title = "HRM", Deleted = false, Active = true }
            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in genders)
                    {
                        if (!context.cor_title.Any(w => w.TitleId == item.TitleId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedDoctypes(DataContext context)
        {
            var doctypes = new List<credit_documenttype>
            {
                    new credit_documenttype { DocumentTypeId = 1,  Name = "Signature", Deleted = false, Active = true }, 
            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in doctypes)
                    {
                        if (!context.credit_documenttype.Any(w => w.DocumentTypeId == item.DocumentTypeId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static void SeedEmploymentType(DataContext context)
        {
            var genders = new List<cor_employertype>
            {
                    new cor_employertype { EmployerTypeId = 1,  Type= "Employed", Deleted = false, Active = true },
                    new cor_employertype { EmployerTypeId = 2,  Type= "Self Employed", Deleted = false, Active = true },
                    new cor_employertype { EmployerTypeId = 3,  Type= "Unemployed", Deleted = false, Active = true },
            };
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in genders)
                    {
                        if (!context.cor_employertype.Any(w => w.EmployerTypeId == item.EmployerTypeId))
                        {
                            context.Add(item);
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally { transaction.Dispose(); }
            }
        }

        public static IWebHost SeedData(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DataContext>();

                SeedGender(context);
                SeedMaritalStatus(context);
                SeedTitles(context);
                SeedEmploymentType(context);
                SeedActivitParents(context);
                SeedActivities(context);
                SeedOperationType(context);
                SeedOperation(context);
                SeedAuthenticationSecuritySetting(context);
                SeedDoctypes(context);
            }
            return host;
        }
    }
}
