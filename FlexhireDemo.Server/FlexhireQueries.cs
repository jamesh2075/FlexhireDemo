namespace FlexhireDemo.Server
{
    public static class GraphQLQueries
    {
        public const string FullQuery = @"
        {
            currentUser {
                name
                profile {
                    industry {
                        name
                    }
                }
                avatarUrl
                userSkills {
                    skill {
                        name
                    }
                    experience
                }
                answers {
                    answer {
                      video {
                        url
                      }
                    }
                }
                webhooks {
                      id
                      url
                      enabled
                    }
                jobApplications(first: 100) {
                    edges {
                        node {
                            status
                            job {
                                title
                                freelancerRate {
                                  formatted
                                }
                            }
                            client {
                                name
                                email
                            }
                            firm {
                                name
                            }
                            contractRequests {
                                question {
                                    title
                                    videoAnswer {
                                        video {
                                            videoType
                                            url
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                freelancerJobApplications(first: 100) {
                    edges {
                        node {
                            status
                            job {
                                title
                                freelancerRate {
                                  formatted
                                }
                            }
                            client {
                                name
                                email
                            }
                            firm {
                                name
                            }
                            contractRequests {
                                question {
                                    title
                                    videoAnswer {
                                        video {
                                            videoType
                                            url
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }";

        public const string CreateWebhookQuery = @"
            mutation CreateWebhook($input: CreateWebhookInput!) {
              createWebhook(input: $input) {
                clientMutationId
                webhook {
                  id
                  url
                  enabled
                }
                errors {
                  message
                }
              }
            }";

        public const string DeleteWebhookQuery = @"
            mutation DeleteWebhook($input: DeleteWebhookInput!) {
              deleteWebhook(input: $input) {
                clientMutationId
                webhook {
                  id
                  url
                  enabled
                }
                errors {
                  message
                }
              }
            }";
    }

}
