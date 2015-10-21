namespace Zanshin.WebApi.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckModelForNullAttribute : ActionFilterAttribute
    {
        private readonly Func<Dictionary<string, object>, bool> validate;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckModelForNullAttribute"/> class.
        /// </summary>
        public CheckModelForNullAttribute()
            : this(arguments => arguments.ContainsValue(null))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckModelForNullAttribute"/> class.
        /// </summary>
        /// <param name="checkCondition">The check condition.</param>
        public CheckModelForNullAttribute(Func<Dictionary<string, object>, bool> checkCondition)
        {
            this.validate = checkCondition;
        }

        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (this.validate(actionContext.ActionArguments))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The argument cannot be null");
            }
        }
    }
}