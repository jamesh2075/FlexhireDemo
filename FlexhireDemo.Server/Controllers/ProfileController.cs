using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FlexhireDemo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly GraphQLService _graphQLService;

        public ProfileController(GraphQLService graphQLService)
        {
            _graphQLService = graphQLService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var response = await _graphQLService.GetCurrentUserDataAsync();
            var profile = response;

            return Ok(new
            {
                name = profile?.name,
                avatarUrl = profile?.avatarUrl,
                industry = profile?.profile?.industry?.name,
                skills = profile?.userSkills,
                answers = profile?.answers.Select(a => a.answer?.video?.url).ToArray()
            });
        }
    }

}
