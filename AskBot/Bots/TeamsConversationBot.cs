﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
using System.Text;
using AskBot.Services.Search;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class TeamsConversationBot : TeamsActivityHandler
    {
        private string _appId;
        private string _appPassword;
        private string _fileCollectionId;
        private string url = "https://dev.askopenaidev.api.speciphic.ai/api/v1/file-collections/6463b04c2b66a3283ae7c6e3/search?q=";
        private string fileBaseUrl = "https://dev.askopenaidev.api.speciphic.ai/api/v1/file-collections/6463b04c2b66a3283ae7c6e3/";
        private readonly string _adaptiveCardTemplate = Path.Combine(".", "Resources", "UserMentionCardTemplate.json");
        private readonly string _immersiveReaderCardTemplate = Path.Combine(".", "Resources", "ImmersiveReaderCard.json");
        private ISearchService _searchService;

        public TeamsConversationBot(IConfiguration config, ISearchService searchService)
        {
            _appId = config["MicrosoftAppId"];
            _appPassword = config["MicrosoftAppPassword"];
            _fileCollectionId = config["DefaultFileCollectionId"];
            _searchService = searchService;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            turnContext.Activity.RemoveRecipientMention();
            var text = turnContext.Activity.Text.Trim().ToLower();
            string result = await GetResponseFromService(text);
            await MentionActivityAsync(turnContext, result, cancellationToken);
        }

        private async Task<string> GetResponseFromService(string query)
        {
            try
            {
                var result = await _searchService.SearchAsync(_fileCollectionId, query);

                if (result.Any())
                {
                    var sb = new StringBuilder();
                    var response = result.First();
                    string responseStr = response.Answer;
                    sb.AppendLine(response.Answer + Environment.NewLine);
                    sb.AppendLine("Related files:" + Environment.NewLine);
                    sb.AppendLine(response.GetFilesSummary(3));
                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


            return "No result found";
        }

        private async Task MentionActivityAsync(ITurnContext<IMessageActivity> turnContext, string result, CancellationToken cancellationToken)
        {

            await turnContext.SendActivityAsync(MessageFactory.Text(result), cancellationToken);
        }

        protected override async Task OnTeamsMembersAddedAsync(IList<TeamsChannelAccount> membersAdded, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var teamMember in membersAdded)
            {
                if (teamMember.Id != turnContext.Activity.Recipient.Id && turnContext.Activity.Conversation.ConversationType != "personal")
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Welcome to the team {teamMember.GivenName} {teamMember.Surname}."), cancellationToken);
                }
            }
        }

        protected override async Task OnInstallationUpdateActivityAsync(ITurnContext<IInstallationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Conversation.ConversationType == "channel")
            {
                await turnContext.SendActivityAsync($"Hello, I can help you understand your HR policies of your company. This bot is configured in {turnContext.Activity.Conversation.Name}");
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text($"Hello, I can help you understand your HR policies of your company."), cancellationToken);
            }
        }

        //-----Subscribe to Conversation Events in Bot integration
        protected override async Task OnTeamsChannelCreatedAsync(ChannelInfo channelInfo, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var heroCard = new HeroCard(text: $"{channelInfo.Name} is the Channel created");
            await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
        }

        protected override async Task OnTeamsChannelRenamedAsync(ChannelInfo channelInfo, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var heroCard = new HeroCard(text: $"{channelInfo.Name} is the new Channel name");
            await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
        }

        protected override async Task OnTeamsChannelDeletedAsync(ChannelInfo channelInfo, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var heroCard = new HeroCard(text: $"{channelInfo.Name} is the Channel deleted");
            await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
        }

        protected override async Task OnTeamsMembersRemovedAsync(IList<TeamsChannelAccount> membersRemoved, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (TeamsChannelAccount member in membersRemoved)
            {
                if (member.Id == turnContext.Activity.Recipient.Id)
                {
                    // The bot was removed
                    // You should clear any cached data you have for this team
                }
                else
                {
                    var heroCard = new HeroCard(text: $"{member.Name} was removed from {teamInfo.Name}");
                    await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
                }
            }
        }

        protected override async Task OnTeamsTeamRenamedAsync(TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var heroCard = new HeroCard(text: $"{teamInfo.Name} is the new Team name");
            await turnContext.SendActivityAsync(MessageFactory.Attachment(heroCard.ToAttachment()), cancellationToken);
        }
        protected override async Task OnReactionsAddedAsync(IList<MessageReaction> messageReactions, ITurnContext<IMessageReactionActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var reaction in messageReactions)
            {
                var newReaction = $"You reacted with '{reaction.Type}' to the following message: '{turnContext.Activity.ReplyToId}'";
                var replyActivity = MessageFactory.Text(newReaction);
                await turnContext.SendActivityAsync(replyActivity, cancellationToken);
            }
        }

        protected override async Task OnReactionsRemovedAsync(IList<MessageReaction> messageReactions, ITurnContext<IMessageReactionActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var reaction in messageReactions)
            {
                var newReaction = $"You removed the reaction '{reaction.Type}' from the following message: '{turnContext.Activity.ReplyToId}'";
                var replyActivity = MessageFactory.Text(newReaction);
                await turnContext.SendActivityAsync(replyActivity, cancellationToken);
            }
        }

        // This method is invoked when message sent by user is updated in chat.
        protected override async Task OnTeamsMessageEditAsync(ITurnContext<IMessageUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyActivity = MessageFactory.Text("Message is updated");
            await turnContext.SendActivityAsync(replyActivity, cancellationToken);
        }

        // This method is invoked when message sent by user is undeleted in chat.
        protected override async Task OnTeamsMessageUndeleteAsync(ITurnContext<IMessageUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyActivity = MessageFactory.Text("Message is undeleted");
            await turnContext.SendActivityAsync(replyActivity, cancellationToken);
        }

        // This method is invoked when message sent by user is soft deleted in chat.
        protected override async Task OnTeamsMessageSoftDeleteAsync(ITurnContext<IMessageDeleteActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyActivity = MessageFactory.Text("Message is soft deleted");
            await turnContext.SendActivityAsync(replyActivity, cancellationToken);
        }
    }
}
