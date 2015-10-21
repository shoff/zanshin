namespace Zanshin.WebApi.Filters
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    /// <summary>
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Occurs before the action method is invoked.
    /// </summary>
    /// <param name="actionContext">The action context.</param>
   public override void OnActionExecuting(HttpActionContext actionContext)
   {
      if (!actionContext.ModelState.IsValid)
      {
         actionContext.Response = actionContext.Request.CreateErrorResponse(
             HttpStatusCode.BadRequest, actionContext.ModelState);
      }
   }
}
}