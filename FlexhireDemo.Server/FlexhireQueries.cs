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
                jobApplications(first: 20) {
                    edges {
                        node {
                            status
                            job {
                                title
                            }
                            client {
                                name
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
                freelancerJobApplications(first: 20) {
                    edges {
                        node {
                            status
                            job {
                                title
                            }
                            client {
                                name
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
    }

}
