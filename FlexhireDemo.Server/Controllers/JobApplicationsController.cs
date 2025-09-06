using Microsoft.AspNetCore.Mvc;

namespace FlexhireDemo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobApplicationsController : ControllerBase
    {
        private readonly GraphQLService _graphQLService;

        public JobApplicationsController(GraphQLService graphQLService)
        {
            _graphQLService = graphQLService;
        }

        [HttpGet]
        public async Task<IActionResult> GetJobApplications()
        {
            var response = await _graphQLService.GetCurrentUserDataAsync();
            var jobs = response?.freelancerJobApplications?.edges?
                  .Select(e => e.node)
                  .ToList();

            return Ok(jobs);
        }
    }

}
