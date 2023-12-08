using EonData.FileShare.Models;
using EonData.FileShare.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EonData.Api.Controllers
{
    [Route("fshare")]
    [ApiController]
    public class FileShareController : ControllerBase
    {
        private IEonShareService eonShare;

        public FileShareController(IEonShareService fileShare) => eonShare = fileShare;

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ShareFolderModel>>> GetFileShareDetails(CancellationToken cancellationToken)
        {
            IEnumerable<ShareFolderModel> result;
            try
            {
                result = await eonShare.GetFileShareAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("files/{*objectKey}")]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadFile(string objectKey, CancellationToken cancellationToken)
        {
            string signedUrl;
            try
            {
                signedUrl = await eonShare.GetSignedUrlAsync(objectKey, cancellationToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            if (string.IsNullOrEmpty(signedUrl))
            {
                return NotFound();
            }

            return Redirect(signedUrl);
        }
    }
}
