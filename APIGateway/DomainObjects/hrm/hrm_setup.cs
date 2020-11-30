using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.DomainObjects.hrm
{
    //public class hrm_setup_location : GeneralEntity
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    public string Address { get; set; }
    //    public int City { get; set; }
    //    public int State { get; set; }
    //    public int Country { get; set; }
    //    public string Additionalinformtion { get; set; }
    //}
    public class hrm_setup_jobdetails : GeneralEntity
    {
        [Key]
        public int Id { get; set; }

        public int Job_title { get; set; }
        public int Job_description { get; set; }
        public List<hrm_setup_sub_skill> Sub_Skills { get; set; }
    }
    public class hrm_setup_sub_skill : GeneralEntity
    {
        [Key]
        public int Id { get; set; }
        public int Job_details_Id { get; set; }
        public string Skill { get; set; }
        public string Description { get; set; }
        public string Weight { get; set; }
    }

    public class hrm_setup_jobgrade : GeneralEntity
    {
        [Key]
        public int Id { get; set; }

        public string Job_grade { get; set; }
        public int Job_grade_reporting_to { get; set; }
        public string Rank { get; set; }
        public int Probation_period_in_months { get; set; }
        public string Description { get; set; }
    }

    public class hrm_setup_employmenttype : GeneralEntity
    {
        [Key]
        public int Id { get; set; }

        public string Employment_type { get; set; }
        public string Description { get; set; }
    }

    public class hrm_setup_employmentlevel : GeneralEntity
    {
        [Key]
        public int Id { get; set; }

        public string Employment_level { get; set; }
        public string Description { get; set; }
    }

    public class hrm_setup_academic_qualification : GeneralEntity
    {
        [Key]
        public int Id { get; set; }

        public string Qualification { get; set; }
        public string Description { get; set; }
        public string Rank { get; set; }
    }

    public class hrm_setup_academic_discipline : GeneralEntity
    {
        [Key]
        public int Id { get; set; }

        public string Discipline { get; set; }
        public string Description { get; set; }
        public string Rank { get; set; }
    }

    public class hrm_setup_academic_grade : GeneralEntity
    {
        [Key]
        public int Id { get; set; }

        public string Grade { get; set; }
        public string Description { get; set; }
        public string Rank { get; set; }
    }

    public class hrm_setup_high_school_subjects : GeneralEntity
    {
        [Key]
        public int Id { get; set; }

        public string Subject { get; set; }
        public string Description { get; set; }
    }

    public class hrm_setup_high_school_grade : GeneralEntity
    {
        [Key]
        public int Id { get; set; }

        public string Grade { get; set; }
        public string Description { get; set; }
        public string Rank { get; set; }
    }
    public class hrm_setup_proffessional_certification : GeneralEntity
    {
        [Key]
        public int Id { get; set; }

        public string Certification { get; set; }
        public string Description { get; set; }
        public string Rank { get; set; }
    }
    public class hrm_setup_languages : GeneralEntity
    {
        [Key]
        public int Id { get; set; }

        public string Language { get; set; }
        public string Description { get; set; }
    }

    public class hrm_setup_proffesional_membership : GeneralEntity
    {
        [Key]
        public int Id { get; set; }

        public string Professional_membership { get; set; }
        public string Description { get; set; }
    }

    public class hrm_setup_hmo : GeneralEntity
    {
        [Key]
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
    public class hrm_setup_gym_workouts : GeneralEntity
    {
        [Key]
        public int Id { get; set; }

        public string Gym { get; set; }
        public string Contact_phone_number { get; set; }
        public string Address { get; set; }
        public string Ratings { get; set; }
        public string Other_comments { get; set; }

    }
}
