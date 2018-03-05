using System;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;

namespace QnAMakerBot.Dialogs
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-qnamaker
    [Serializable]
    public class BasicQnAMakerDialog : QnAMakerDialog
    {
        // Go to https://qnamaker.ai and feed data, train & publish your QnA Knowledge base.
        // Parameters to QnAMakerService are:
        // Required: subscriptionKey, knowledgebaseId, 
        // Optional: defaultMessage, scoreThreshold[Range 0.0 – 1.0]
        public BasicQnAMakerDialog() : base(
            new QnAMakerService(
                new QnAMakerAttribute(
                    Utils.GetAppSetting("QnASubscriptionKey"), Utils.GetAppSetting("QnAKnowledgebaseId"),
                    //ConfigurationManager.AppSettings["QnASubscriptionKey"], ConfigurationManager.AppSettings["QnAKnowledgebaseId"],
                    "我找不到適合的答案耶 >.<", 0.5)
            )
        )
        { }
    }
}