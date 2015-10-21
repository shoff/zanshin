using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Zanshin.Areas.RouteDebugger.Models;

namespace Zanshin.Areas.RouteDebugger
{
    /// <summary>
    /// Hijacks the original invoker. It examines the header before 
    /// executing the action. If the inspect header exists, returns the inspection data in a 200 response.
    /// If the inspection header does not exist, the delegate calls the default InvokeActionAsync method.
    /// 
    /// The inspection data saved in the request property are collected when the request is passed
    /// along the stack.
    /// </summary>
    public class InspectActionInvoker : IHttpActionInvoker
    {
        private readonly IHttpActionInvoker innerInvoker;

        /// <summary>
        /// Initializes a new instance of the <see cref="InspectActionInvoker"/> class.
        /// </summary>
        /// <param name="innerInvoker">The inner invoker.</param>
        public InspectActionInvoker(IHttpActionInvoker innerInvoker)
        {
            this.innerInvoker = innerInvoker;
        }

        /// <summary>
        /// Executes asynchronously the HTTP operation.
        /// </summary>
        /// <param name="actionContext">The execution context.</param>
        /// <param name="cancellationToken">The cancellation token assigned for the HTTP operation.</param>
        /// <returns>
        /// The newly started task.
        /// </returns>
        public Task<HttpResponseMessage> InvokeActionAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext.Request.IsInspectRequest())
            {
                var inspectData = new InspectData(actionContext.Request);
                inspectData.RealHttpStatus = HttpStatusCode.OK;
                return Task.FromResult<HttpResponseMessage>(actionContext.Request.CreateResponse<InspectData>(
                    HttpStatusCode.OK, inspectData));
            }
            return this.innerInvoker.InvokeActionAsync(actionContext, cancellationToken);
        }
    }
}
