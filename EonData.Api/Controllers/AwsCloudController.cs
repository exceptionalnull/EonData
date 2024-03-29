﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EonData.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AwsCloudController : ControllerBase
    {
        [HttpGet]
        [Route("/bastion")]
        public ActionResult BastionStatus()
        {
            return Ok();
        }
    }
}
