using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Application.Helpers
{
    public static class ErrorResponseFactory
    {
        public static ProblemDetails CreateProblemDetails(string title, string detail, int status, string instance = null)
        {
            return new ProblemDetails
            {
                Title = title,
                Detail = detail,
                Status = status,
                Instance = instance
            };
        }
    }

}
