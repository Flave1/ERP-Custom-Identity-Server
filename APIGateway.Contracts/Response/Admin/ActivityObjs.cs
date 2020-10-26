using GODPAPIs.Contracts.RequestResponse;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Response.Admin
{
    public class ActivityParentRespObj
    {
        public List<ActivityParentObj> ActivityParents { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    
    public class ActivityParentObj
    {
        public int ActivityParentId { get; set; }
        public string ActivityParentName { get; set; }
        public bool? Active { get; set; }
        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }

    public class ActivityRespObj
    {
        public List<ActivityObj> Activities { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class ActivityObj
    {
        public int ActivityId { get; set; }
        public int ActivityParentId { get; set; }
        public string ActivityName { get; set; }
        //.......................
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string RoleName { get; set; }
        public string ActivityParentName { get; set; }
        public bool CanAdd { get; set; }
        public bool CanApprove { get; set; }
        public bool CanDelete { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
        //........................
        public bool? Active { get; set; }
        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }


    public class QuestionsRegRespObj
    {
        public int QuestionId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class QuestionsRespObj   
    {
        public List<QuestionsObj> Questions { get; set; }
        public int UserCount { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class QuestionsObj
    {
        public int QuestionId { get; set; }
        public string Qiestion { get; set; }
    }

}
