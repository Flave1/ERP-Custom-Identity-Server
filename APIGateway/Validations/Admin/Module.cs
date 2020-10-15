using APIGateway.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Puchase_and_payables.Handlers.Supplier.Settup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Validations.Admin
{
    public class AddUpdateSolutionModuleCommandval : AbstractValidator<AddUpdateSolutionModuleCommand>
    {
        private readonly DataContext _dataContext;
        public AddUpdateSolutionModuleCommandval(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(a => a.SolutionName).NotEmpty();
            RuleFor(c => c).MustAsync(NoDuplicate).WithMessage("Duplicate Settup Detected");
        }
        private async Task<bool> NoDuplicate(AddUpdateSolutionModuleCommand request, CancellationToken cancellationToken)
        {
            var auth = await _dataContext.SolutionModule.Where(a => a.SolutionName == request.SolutionName && request.SolutionModuleId != a.SolutionModuleId).ToListAsync();
            if (auth.Count() > 0)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }


}
