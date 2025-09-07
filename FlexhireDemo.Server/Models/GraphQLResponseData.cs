using GraphQL;
using Microsoft.AspNetCore;
using Newtonsoft.Json;

namespace FlexhireDemo.Server.Models
{
    public class CurrentUserResponse
    {
        public CurrentUser currentUser { get; set; }
    }

    public class CurrentUser
    {
        public string name { get; set; }
        public string avatarUrl { get; set; }
        public Profile profile { get; set; }
        public List<UserSkill> userSkills { get; set; }
        public List<AnswerWrapper> answers { get; set; }
        public List<Webhook> webhooks { get; set; }
        public JobApplicationConnection jobApplications { get; set; }
        public JobApplicationConnection freelancerJobApplications { get; set; }
    }

    public class Profile
    {
        public Industry industry { get; set; }
    }

    public class Industry
    {
        public string name { get; set; }
    }

    public class UserSkill
    {
        public Skill skill { get; set; }
        public int experience { get; set; }
    }

    public class Skill
    {
        public string name { get; set; }
    }

    public class AnswerWrapper
    {
        public Answer answer { get; set; }
    }

    public class Answer
    {
        public Video video { get; set; }
    }

    public class Video
    {
        public string url { get; set; }
    }

    public class JobApplicationConnection
    {
        public List<JobApplicationEdge> edges { get; set; }
    }

    public class JobApplicationEdge
    {
        public JobApplication node { get; set; }
    }

    public class JobApplication
    {
        public string status { get; set; }
        public Job job { get; set; }
        public Client client { get; set; }
        public Firm firm { get; set; }
        public List<ContractRequest> contractRequests { get; set; }
    }

    public class Job
    {
        public string title { get; set; }
        public FreelancerRate freelancerRate { get; set; }
    }

    public class FreelancerRate
    {
        public string formatted { get; set; }
    }

    public class Client
    {
        public string name { get; set; }
        public string email { get; set; }
    }

    public class Firm
    {
        public string name { get; set; }
    }

    public class ContractRequest
    {
        public Question question { get; set; }
    }

    public class Question
    {
        public string title { get; set; }
        public VideoAnswer videoAnswer { get; set; }
    }

    public class VideoAnswer
    {
        public Video video { get; set; }
    }

    public class CreateWebhookInput
    {
        public string clientMutationId { get; set; }
        public bool enabled { get; set; } = true;
        public string url { get; set; }
    }

    public class CreateWebhookVariables
    {
        public CreateWebhookInput input { get; set; }
    }

    public class CreateWebhookRequest
    {
        public string query { get; set; }
        public CreateWebhookVariables variables { get; set; }
    }

    public class CreateWebhookResponse
    {
        [JsonProperty("createWebhook")]
        public CreateWebhookPayload CreateWebhook { get; set; }
    }

    public class CreateWebhookPayload
    {
        [JsonProperty("clientMutationId")]
        public string ClientMutationId { get; set; }

        [JsonProperty("webhook")]
        public Webhook Webhook { get; set; }

        [JsonProperty("errors")]
        public List<GraphQLError> Errors { get; set; }
    }

    public class DeleteWebhookResponse
    {
        [JsonProperty("deleteWebhook")]
        public CreateWebhookPayload DeleteWebhook { get; set; }
    }

    public class Webhook
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }

    public class GraphQLError
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }

}
