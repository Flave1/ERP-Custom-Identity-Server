using GODPAPIs.Contracts.GeneralExtension;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Contracts.Response.HRM
{
     
    public class hrm_setup_add_update_response
    {
        public hrm_setup_add_update_response()
        {
            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage()};
        }
        public int Setup_id { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class hrm_setup_jobdetails_contract_resp
    {
        public hrm_setup_jobdetails_contract_resp()
        {
            Setuplist = new List<hrm_setup_jobdetails_contract>();
        }
        public List<hrm_setup_jobdetails_contract> Setuplist { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class hrm_setup_jobdetails_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; }

        public int Job_title { get; set; }
        public int Job_description { get; set; }
        public List<hrm_setup_sub_skill_contract> Sub_Skills { get; set; }
    }
    public class hrm_setup_sub_skill_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; }
        public int Job_details_Id { get; set; }
        public string Skill { get; set; }
        public string Description { get; set; }
        public string Weight { get; set; }
    }

    public class hrm_setup_jobgrade_contract_resp
    {
        public List<hrm_setup_jobgrade_contract> Setuplist { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class hrm_setup_jobgrade_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; }

        public string Job_grade { get; set; }
        public int Job_grade_reporting_to { get; set; }
        public string Rank { get; set; }
        public int Probation_period_in_months { get; set; }
        public string Description { get; set; }

        public string Job_grade_reporting_to_name { get; set; }
    }

    public class hrm_setup_employmenttype_contract_resp
    {
        public List<hrm_setup_employmenttype_contract> Setuplist { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    
    public class hrm_setup_employmenttype_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; }

        public string Employment_type { get; set; }
        public string Description { get; set; }
    }

    public class hrm_setup_employmentlevel_contract_resp
    {
        public List<hrm_setup_employmentlevel_contract> Setuplist { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class hrm_setup_employmentlevel_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; }

        public string Employment_level { get; set; }
        public string Description { get; set; }
    }

    public class hrm_setup_academic_qualification_contract_resp
    {
        public List<hrm_setup_academic_qualification_contract> Setuplist { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class hrm_setup_academic_qualification_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; } 
        public string Qualification { get; set; }
        public string Description { get; set; }
        public string Rank { get; set; }
    }

    public class hrm_setup_academic_discipline_contract_resp
    {
        public List<hrm_setup_academic_discipline_contract> Setuplist { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class hrm_setup_academic_discipline_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; }

        public string Discipline { get; set; }
        public string Description { get; set; }
        public string Rank { get; set; }
    }

    public class hrm_setup_academic_grade_contract_resp
    {
        public List<hrm_setup_academic_grade_contract> Setuplist { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class hrm_setup_academic_grade_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; }

        public string Grade { get; set; }
        public string Description { get; set; }
        public string Rank { get; set; }
    }

    public class hrm_setup_high_school_subjects_contract_resp
    {
        public List<hrm_setup_high_school_subjects_contract> Setuplist { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class hrm_setup_high_school_subjects_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; }

        public string Subject { get; set; }
        public string Description { get; set; }
    }

    public class hrm_setup_high_school_grade_contract_resp
    {
        public List<hrm_setup_high_school_grade_contract> Setuplist { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class hrm_setup_high_school_grade_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; }

        public string Grade { get; set; }
        public string Description { get; set; }
        public string Rank { get; set; }
    }

    public class hrm_setup_proffessional_certification_contract_resp
    {
        public List<hrm_setup_proffessional_certification_contract> Setuplist { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class hrm_setup_proffessional_certification_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; }

        public string Certification { get; set; }
        public string Description { get; set; }
        public string Rank { get; set; }
    }

    public class hrm_setup_languages_contract_resp
    {
        public List<hrm_setup_languages_contract> Setuplist { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class hrm_setup_languages_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; }

        public string Language { get; set; }
        public string Description { get; set; }
    }

    public class hrm_setup_proffesional_membership_contract_resp
    {
        public List<hrm_setup_proffesional_membership_contract> Setuplist { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class hrm_setup_proffesional_membership_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; }

        public string Professional_membership { get; set; }
        public string Description { get; set; }
    }

    public class hrm_setup_hmo_contract_resp
    {
        public List<hrm_setup_hmo_contract> Setuplist { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class hrm_setup_hmo_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; }

        public string Hmo_name { get; set; }
        public string Hmo_code { get; set; }
        public string Contact_phone_number { get; set; }
        public string Contact_email { get; set; }
        public string Address { get; set; }
        public DateTime Reg_date { get; set; }
        public string Rating { get; set; }
        public string Order_comments { get; set; }
    }

    public class hrm_setup_gym_workouts_contract_resp
    {
        public List<hrm_setup_gym_workouts_contract> Setuplist { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class hrm_setup_gym_workouts_contract : IRequest<hrm_setup_add_update_response>
    {
        
        public int Id { get; set; }

        public string Gym { get; set; }
        public string Contact_phone_number { get; set; }
        public string Address { get; set; }
        public string Ratings { get; set; }
        public string Other_comments { get; set; }

    }
}
