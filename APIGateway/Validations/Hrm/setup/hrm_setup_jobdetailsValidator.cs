using APIGateway.Contracts.Response.HRM;
using APIGateway.Data; 
using FluentValidation; 
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Validations.Hrm.setup
{ 

    public class hrm_setup_academic_discipline_contractValidator : AbstractValidator<hrm_setup_academic_discipline_contract>
    {
        private readonly DataContext _dataContext;
        public hrm_setup_academic_discipline_contractValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.Discipline).NotEmpty();
            RuleFor(e => e.Rank).NotEmpty(); 
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Setup already exist");
        }

        private async Task<bool> NoDuplicateAsync(hrm_setup_academic_discipline_contract request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = _dataContext.hrm_setup_academic_discipline.FirstOrDefault(e => e.Discipline == request.Discipline.ToLower() && e.Id!= request.Id && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.hrm_setup_academic_discipline.Count(e => e.Discipline.ToLower() == request.Discipline.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }
    public class hrm_setup_academic_grade_contractValidator : AbstractValidator<hrm_setup_academic_grade_contract>
    {
        private readonly DataContext _dataContext;
        public hrm_setup_academic_grade_contractValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.Grade).NotEmpty();
            RuleFor(e => e.Rank).NotEmpty();
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Setup already exist");
        }

        private async Task<bool> NoDuplicateAsync(hrm_setup_academic_grade_contract request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = _dataContext.hrm_setup_academic_grade.FirstOrDefault(e => e.Grade == request.Grade.ToLower() && e.Id != request.Id && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.hrm_setup_academic_grade.Count(e => e.Grade.ToLower() == request.Grade.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    public class hrm_setup_academic_qualification_contractValidator : AbstractValidator<hrm_setup_academic_qualification_contract>
    {
        private readonly DataContext _dataContext;
        public hrm_setup_academic_qualification_contractValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();  
            RuleFor(e => e.Rank).NotEmpty();
            RuleFor(e => e.Qualification).NotEmpty();
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Setup already exist");
        }

        private async Task<bool> NoDuplicateAsync(hrm_setup_academic_qualification_contract request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = _dataContext.hrm_setup_academic_qualification.FirstOrDefault(e => e.Qualification == request.Qualification.ToLower() && e.Id != request.Id && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.hrm_setup_academic_qualification.Count(e => e.Qualification.ToLower() == request.Qualification.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    public class hrm_setup_employmenttype_contractValidator : AbstractValidator<hrm_setup_employmenttype_contract>
    {
        private readonly DataContext _dataContext;
        public hrm_setup_employmenttype_contractValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.Employment_type).NotEmpty(); 
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Setup already exist");
        }

        private async Task<bool> NoDuplicateAsync(hrm_setup_employmenttype_contract request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = _dataContext.hrm_setup_employmenttype.FirstOrDefault(e => e.Employment_type == request.Employment_type.ToLower() && e.Id != request.Id && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.hrm_setup_employmenttype.Count(e => e.Employment_type.ToLower() == request.Employment_type.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    public class hrm_setup_employmentlevel_contractValidator : AbstractValidator<hrm_setup_employmentlevel_contract>
    {
        private readonly DataContext _dataContext;
        public hrm_setup_employmentlevel_contractValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.Employment_level).NotEmpty(); 
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Setup already exist");
        }

        private async Task<bool> NoDuplicateAsync(hrm_setup_employmentlevel_contract request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = _dataContext.hrm_setup_employmentlevel.FirstOrDefault(e => e.Employment_level == request.Employment_level.ToLower() && e.Id != request.Id && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.hrm_setup_employmentlevel.Count(e => e.Employment_level.ToLower() == request.Employment_level.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    public class hrm_setup_gym_workouts_contractValidator : AbstractValidator<hrm_setup_gym_workouts_contract>
    {
        private readonly DataContext _dataContext;
        public hrm_setup_gym_workouts_contractValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Address).NotEmpty();
            RuleFor(e => e.Contact_phone_number).NotEmpty();
            RuleFor(e => e.Gym).NotEmpty();
            RuleFor(e => e.Other_comments).NotEmpty();
            RuleFor(e => e.Ratings).NotEmpty(); 
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Setup already exist");
        }

        private async Task<bool> NoDuplicateAsync(hrm_setup_gym_workouts_contract request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = _dataContext.hrm_setup_gym_workouts.FirstOrDefault(e => e.Gym == request.Gym.ToLower() && e.Id != request.Id && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.hrm_setup_gym_workouts.Count(e => e.Gym.ToLower() == request.Gym.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    public class hrm_setup_high_school_grade_contractValidator : AbstractValidator<hrm_setup_high_school_grade_contract>
    {
        private readonly DataContext _dataContext;
        public hrm_setup_high_school_grade_contractValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.Grade).NotEmpty();
            RuleFor(e => e.Rank).NotEmpty(); 
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Setup already exist");
        }

        private async Task<bool> NoDuplicateAsync(hrm_setup_high_school_grade_contract request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = _dataContext.hrm_setup_high_school_grades.FirstOrDefault(e => e.Grade == request.Grade.ToLower() && e.Id != request.Id && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.hrm_setup_high_school_grades.Count(e => e.Grade.ToLower() == request.Grade.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    public class hrm_setup_high_school_subjects_contractValidator : AbstractValidator<hrm_setup_high_school_subjects_contract>
    {
        private readonly DataContext _dataContext;
        public hrm_setup_high_school_subjects_contractValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.Subject).NotEmpty(); 
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Setup already exist");
        }

        private async Task<bool> NoDuplicateAsync(hrm_setup_high_school_subjects_contract request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = _dataContext.hrm_setup_high_school_subjects.FirstOrDefault(e => e.Subject == request.Subject.ToLower() && e.Id != request.Id && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.hrm_setup_high_school_subjects.Count(e => e.Subject.ToLower() == request.Subject.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    public class hrm_setup_hmo_contractValidator : AbstractValidator<hrm_setup_hmo_contract>
    {
        private readonly DataContext _dataContext;
        public hrm_setup_hmo_contractValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Address).NotEmpty();
            RuleFor(e => e.Contact_email).NotEmpty();
            RuleFor(e => e.Contact_phone_number).NotEmpty();
            RuleFor(e => e.Hmo_code).NotEmpty();
            RuleFor(e => e.Hmo_name).NotEmpty();
            RuleFor(e => e.Order_comments).NotEmpty();
            RuleFor(e => e.Rating).NotEmpty();
            RuleFor(e => e.Reg_date).NotEmpty(); 
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Setup already exist");
        }

        private async Task<bool> NoDuplicateAsync(hrm_setup_hmo_contract request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = _dataContext.hrm_setup_hmo.FirstOrDefault(e => e.Hmo_code == request.Hmo_code.ToLower() && e.Id != request.Id && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.hrm_setup_hmo.Count(e => e.Hmo_code.ToLower() == request.Hmo_code.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    public class hrm_setup_jobdetails_contractValidator : AbstractValidator<hrm_setup_jobdetails_contract>
    {
        private readonly DataContext _dataContext;
        public hrm_setup_jobdetails_contractValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Job_description).NotEmpty();
            RuleFor(e => e.Job_title).NotEmpty(); 
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Setup already exist");
        }

        private async Task<bool> NoDuplicateAsync(hrm_setup_jobdetails_contract request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = _dataContext.hrm_setup_jobdetails.FirstOrDefault(e => e.Job_title == request.Job_title && e.Id != request.Id && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.hrm_setup_jobdetails.Count(e => e.Job_title == request.Job_title && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    public class hrm_setup_jobgrade_contractValidator : AbstractValidator<hrm_setup_jobgrade_contract>
    {
        private readonly DataContext _dataContext;
        public hrm_setup_jobgrade_contractValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.Job_grade).NotEmpty(); 
            RuleFor(e => e.Rank).NotEmpty(); 
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Setup already exist");
        }

        private async Task<bool> NoDuplicateAsync(hrm_setup_jobgrade_contract request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = _dataContext.hrm_setup_jobgrade.FirstOrDefault(e => e.Job_grade == request.Job_grade.ToLower() && e.Id != request.Id && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.hrm_setup_jobgrade.Count(e => e.Job_grade.ToLower() == request.Job_grade.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    public class hrm_setup_languages_contractValidator : AbstractValidator<hrm_setup_languages_contract>
    {
        private readonly DataContext _dataContext;
        public hrm_setup_languages_contractValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.Language).NotEmpty();  
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Setup already exist");
        }

        private async Task<bool> NoDuplicateAsync(hrm_setup_languages_contract request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = _dataContext.hrm_setup_languages.FirstOrDefault(e => e.Language == request.Language.ToLower() && e.Id != request.Id && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.hrm_setup_languages.Count(e => e.Language.ToLower() == request.Language.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    public class hrm_setup_proffesional_membership_contractValidator : AbstractValidator<hrm_setup_proffesional_membership_contract>
    {
        private readonly DataContext _dataContext;
        public hrm_setup_proffesional_membership_contractValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.Professional_membership).NotEmpty(); 
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Setup already exist");
        }

        private async Task<bool> NoDuplicateAsync(hrm_setup_proffesional_membership_contract request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = _dataContext.hrm_setup_proffesional_membership.FirstOrDefault(e => e.Professional_membership == request.Professional_membership.ToLower() && e.Id != request.Id && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.hrm_setup_proffesional_membership.Count(e => e.Professional_membership.ToLower() == request.Professional_membership.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    public class hrm_setup_proffessional_certification_contractValidator : AbstractValidator<hrm_setup_proffessional_certification_contract>
    {
        private readonly DataContext _dataContext;
        public hrm_setup_proffessional_certification_contractValidator(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.Certification).NotEmpty();
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Setup already exist");
        }

        private async Task<bool> NoDuplicateAsync(hrm_setup_proffessional_certification_contract request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var item = _dataContext.hrm_setup_proffessional_certification.FirstOrDefault(e => e.Certification == request.Certification.ToLower() && e.Id != request.Id && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.hrm_setup_proffessional_certification.Count(e => e.Certification.ToLower() == request.Certification.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }
}
