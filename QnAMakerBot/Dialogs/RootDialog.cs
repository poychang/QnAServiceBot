using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using QnAMakerBot.AgentModule;
using QnAMakerBot.AgentModule.Interface;

namespace QnAMakerBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private readonly IUserToAgent _userToAgent;

        public RootDialog(IUserToAgent userToAgent)
        {
            _userToAgent = userToAgent;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceived);
            return Task.CompletedTask;
        }

        private async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (await _userToAgent.IsWantToTalkWithHuman(message as Activity, default(CancellationToken)))
            {
                var agent = await _userToAgent.IntitiateConversationWithAgentAsync(message as Activity, default(CancellationToken));
                if (agent == null)
                    await context.PostAsync(ConversationText.AllAgentAreBusy);
            }
            else if (!string.IsNullOrEmpty(message.Text))
            {
                var qnaSubscriptionKey = ConfigurationManager.AppSettings["QnASubscriptionKey"];
                var qnaKBId = ConfigurationManager.AppSettings["QnAKnowledgebaseId"];

                // QnA Subscription Key and KnowledgeBase Id null verification
                if (!string.IsNullOrEmpty(qnaSubscriptionKey) && !string.IsNullOrEmpty(qnaKBId))
                {
                    await context.Forward(new BasicQnAMakerDialog(), AfterAnswerAsync, message, CancellationToken.None);
                }
                else
                {
                    await context.PostAsync(ConversationText.NeedQnAMakerSetting);
                }
            }
            else
            {
                await context.PostAsync(message.Text);
            }
            context.Done(true);
        }

        private Task AfterAnswerAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            // wait for the next user message
            context.Wait(MessageReceived);
            return Task.CompletedTask;
        }
    }
}