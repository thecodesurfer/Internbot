﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Logging;

namespace Internbot.API.Dialogs
{
    public class RootDialog : ComponentDialog
    {
        protected readonly ILogger _logger;

        public RootDialog(AskQuestionDialog askQuestionDialog, ILogger<RootDialog> logger)
            : base(nameof(RootDialog))
        {
            _logger = logger;

            AddDialog(askQuestionDialog);
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                WhoAreYouQuestionsStepAsync,
                PersonalityQuestionsStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> WhoAreYouQuestionsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var question = new QuestionDetails
            {
                Question = "Zou je wat over jezelf kunnen vertellen?"
            };

            return await stepContext.BeginDialogAsync(nameof(AskQuestionDialog), question, cancellationToken);
        }

        private async Task<DialogTurnResult> PersonalityQuestionsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var question = new QuestionDetails
            {
                Question = "Wat is jouw rol binnen een team?",
                KeepAskingResponse = "Kun je dit verder toelichten?"
            };

            return await stepContext.BeginDialogAsync(nameof(AskQuestionDialog), question, cancellationToken);
        }
    }
}
