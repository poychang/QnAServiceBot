using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using QnAMakerBot.AgentModule;
using QnAMakerBot.Dialogs;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using QnAMakerBot.AgentModule.Interface;

namespace QnAMakerBot.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        // POST: api/Messages
        /// <summary>
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await SendAsync(activity, (scope) => new RootDialog(scope.Resolve<IUserToAgent>()));
            }
            else
            {
                await HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<Activity> HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed Use
                // Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info Not
                // available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists Activity.From + Activity.Action represent
                // what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing that the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }
            else if (message.Type == ActivityTypes.Event)
            {
                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, message))
                {
                    var cancellationToken = default(CancellationToken);
                    var agentService = scope.Resolve<IAgentService>();
                    switch (message.AsEventActivity().Name)
                    {
                        case "connect":
                            await agentService.RegisterAgentAsync(message, cancellationToken);
                            break;

                        case "disconnect":
                            await agentService.UnregisterAgentAsync(message, cancellationToken);
                            break;

                        case "stopConversation":
                            await StopConversation(agentService, message, cancellationToken);
                            await agentService.RegisterAgentAsync(message, cancellationToken);
                            break;

                        default:
                            break;
                    }
                }
            }

            return null;
        }

        private async Task StopConversation(IAgentService agentService, Activity agentActivity, CancellationToken cancellationToken)
        {
            var user = await agentService.GetUserInConversationAsync(agentActivity, cancellationToken);
            var agentReply = agentActivity.CreateReply();
            if (user == null)
            {
                agentReply.Text = ConversationText.NobodyTalkingToYou;
                await Utility.SendToConversationAsync(agentReply);
                return;
            }

            var userActivity = user.ConversationReference.GetPostToBotMessage();
            await agentService.StopAgentUserConversationAsync(
                userActivity,
                agentActivity,
                cancellationToken);

            var userReply = userActivity.CreateReply();
            userReply.Text = ConversationText.DisconnectWithAgent;
            await Utility.SendToConversationAsync(userReply);
            userReply.Text = ConversationText.StillCanTalkWithBot;
            await Utility.SendToConversationAsync(userReply);

            agentReply.Text = ConversationText.StopTheConversation;
            await Utility.SendToConversationAsync(agentReply);
        }

        private async Task SendAsync(IMessageActivity toBot, Func<ILifetimeScope, IDialog<object>> makeRoot, CancellationToken token = default(CancellationToken))
        {
            using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, toBot))
            {
                DialogModule_MakeRoot.Register(scope, () => makeRoot(scope));
                var task = scope.Resolve<IPostToBot>();
                await task.PostAsync(toBot, token);
            }
        }
    }
}
