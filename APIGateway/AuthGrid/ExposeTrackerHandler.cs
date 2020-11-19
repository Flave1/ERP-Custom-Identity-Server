using APIGateway.Data;
using GODP.APIsContinuation.DomainObjects.UserAccount; 
using MediatR; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;  
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.AuthGrid
{
    public class ExposeTracker : IRequest<HttpStatusCode>
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public class ExposeTrackerHandler : IRequestHandler<ExposeTracker, HttpStatusCode>
        {
            private readonly DataContext _context; 
            private readonly UserManager<cor_useraccount> _userManager;
            public ExposeTrackerHandler(
                DataContext context,  
                UserManager<cor_useraccount> userManager)
            {
                _context = context;
                _userManager = userManager; 
            }
            public async Task<HttpStatusCode> Handle(ExposeTracker request, CancellationToken cancellationToken)
            {  
                var userAccount = await _userManager.FindByIdAsync(request.UserId);
                if(userAccount == null)
                {
                    return HttpStatusCode.OK;
                }
                var tracked = await _context.Tracker.Where(e => e.UserId == request.UserId && e.Token == request.Token).ToListAsync();
                if(tracked.Count() == 0)
                {
                    return HttpStatusCode.Unauthorized; 
                }
                return HttpStatusCode.OK;
            }
        }

    }
}