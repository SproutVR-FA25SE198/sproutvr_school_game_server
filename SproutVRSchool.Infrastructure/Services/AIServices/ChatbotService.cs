using Google.Api.Gax;
using Google.Api.Gax.Grpc;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Dialogflow.Cx.V3;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using SproutVRSchool.Application.Abstractions.AIServices;
using SproutVRSchool.Application.RequestHandlers.Authorized.AIChatbot.Commands;

namespace SproutVRSchool.Infrastructure.Services.AIServices;

public class ChatbotService(IConfiguration configuration) : IChatbotService
{
    public async Task<ChatResponseDto> Chat(string chatMessage, Guid userId, string? organizationId)
    {
        string credentialPath = configuration["Dialogflow:CredentialsPath"]!;
        GoogleCredential credential;

        using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
        {
            var raw = ServiceAccountCredential.FromServiceAccountData(stream);
            credential = raw.ToGoogleCredential();
        }


        // 2. Get Config from appsettings
        string projectId = configuration["Dialogflow:ProjectId"]!;
        string locationId = configuration["Dialogflow:LocationId"]!;
        string agentId = configuration["Dialogflow:AgentId"]!;

        // 3. Config client with endpoint
        var clientBuilder = new SessionsClientBuilder
        {
            Endpoint = locationId == "global"
                ? "dialogflow.googleapis.com"
                : $"{locationId}-dialogflow.googleapis.com:443",
            GoogleCredential = credential
        };
        SessionsClient client = await clientBuilder.BuildAsync();

        // 4. Create session name
        var sessionName = SessionName.FromProjectLocationAgentSession(
            projectId, locationId, agentId, userId.ToString());

        // 5. create request object
        var request = new DetectIntentRequest
        {
            SessionAsSessionName = sessionName,
            QueryInput = new QueryInput
            {
                Text = new TextInput { Text = chatMessage },
                LanguageCode = "vi"
            },
            QueryParams = new QueryParameters()
        };

        // 6. Add organizationId to request
        if (!string.IsNullOrEmpty(organizationId))
        {
            request.QueryParams.Parameters = new Struct
            {
                Fields = { { "organizationId", Value.ForString(organizationId) } }
            };
        }
        else
        {
            request.QueryParams.Parameters = new Struct
            {
                Fields = { { "organizationId", Value.ForNull() } }
            };
        }

        var callSettings = CallSettings.FromExpiration(
            Expiration.FromTimeout(TimeSpan.FromMinutes(3))
        );

        // 7. Call API
        DetectIntentResponse response = await client.DetectIntentAsync(request, callSettings);

        // 8. Handle response from AI agent
        string botReply = string.Join(" ", response.QueryResult.ResponseMessages
            .Where(m => m.Text != null)
            .SelectMany(m => m.Text.Text_));

        // if botReply is empty, return "..."
        return new ChatResponseDto() { ChatbotReply = string.IsNullOrEmpty(botReply) ? "..." : botReply };
    }
}
