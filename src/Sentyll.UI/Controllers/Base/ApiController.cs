using System.Net;
using Sentyll.UI.Core.Models.Failure;

namespace Sentyll.UI.Controllers.Base;

[ApiController]
[Produces("application/json")]
[ProducesResponseType<InternalServerFailureResult>((int)HttpStatusCode.InternalServerError)]
[ProducesResponseType<BadRequestFailureResult>((int)HttpStatusCode.BadRequest)]
[ProducesResponseType<InternalServerFailureResult>((int)HttpStatusCode.Unauthorized)]
public abstract class ApiController : ControllerBase
{
}