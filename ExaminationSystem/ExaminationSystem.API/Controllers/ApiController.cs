

using ExaminationSystem.Domin.Common.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExaminationSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    protected ActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }

       /* if (errors.All(error => error.Type == ErrorKind.Validation))
        {
            return ValidationProblem(errors);
        }*/

        return Problem(errors[0]);
    }

    private ObjectResult Problem(Error error)
    {
     
        int errorCode = (int)error.Code;
        return Problem(statusCode: errorCode, title: error.Description);
    }

   /* private ActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        errors.ForEach(error => modelStateDictionary.AddModelError(error.Code, error.Description));

        return ValidationProblem(modelStateDictionary);
    }*/
}