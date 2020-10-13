using APIGateway.AuthGrid;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers
{
    public class LoginFailedCommandHandler : IRequestHandler<LoginFailed, LogingFailedRespObj>
    {
		private readonly IMeasureService _measureService;
		public LoginFailedCommandHandler(IMeasureService measureService)
		{
			_measureService = measureService;
		}
        public async Task<LogingFailedRespObj> Handle(LoginFailed request, CancellationToken cancellationToken)
        {
			try
			{
				return await _measureService.CheckForFailedTrailsAsync(request.IsSuccessful, request.Module, request.UserId);
			}
			catch (Exception ex)
			{
				return null;
			}
        }
    }

	public class SessionTrailCommandHandler : IRequestHandler<SessionTrail, SessionCheckerRespObj>
	{
		private readonly IMeasureService _measureService;
		public SessionTrailCommandHandler(IMeasureService measureService)
		{
			_measureService = measureService;
		}
		public async Task<SessionCheckerRespObj> Handle(SessionTrail request, CancellationToken cancellationToken)
		{
			try
			{
				return await _measureService.CheckForSessionTrailAsync(request.UserId, request.Module);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
