using System.Linq;
using Domain.Compartilhado;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Compartilhado
{
    public class ValidateActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage));

                context.Result = new BadRequestObjectResult(new Resposta("Erro de validação", false, errors));

            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {


        }
    }
}