using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Interface.Bll.Dtos.Error;

namespace CaffStore.Backend.Api.Controllers
{
	[Route("api")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi = true)]
	public class ErrorController : ControllerBase
	{
		private readonly IWebHostEnvironment _hostEnvironment;

		public ErrorController(IWebHostEnvironment hostEnvironment)
		{
			_hostEnvironment = hostEnvironment;
		}

		[Route("error")]
		public ErrorResponse Error()
		{
			var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
			var exception = context?.Error;
			IEnumerable<string> errors = null;
			var statusCode = HttpStatusCode.InternalServerError;

			if (exception is CaffStoreBusinessException businessException)
			{
				statusCode = businessException.StatusCode;
				errors = businessException.Errors ?? Enumerable.Empty<string>();
			}

			Response.StatusCode = (int)statusCode;

			var errorResponse = new ErrorResponse
			{
				StatusCode = (int)statusCode,
				Type = exception.GetType().Name,
				Message = exception.Message,
				Errors = errors,
				StackTrace = null,
			};

			if (_hostEnvironment.IsDevelopment())
				errorResponse.StackTrace = exception.StackTrace;

			return errorResponse;
		}
	}
}
