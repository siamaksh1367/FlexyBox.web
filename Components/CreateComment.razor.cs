using FlexyBox.core.Commands.CreateComment;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class CreateComment
    {
        private string newCommentContent = string.Empty;

        [Parameter]
        public EventCallback<CreateCommentCommand> OnCommentSubmit { get; set; }

        private async Task SubmitComment()
        {
            if (!string.IsNullOrWhiteSpace(newCommentContent))
            {
                var comment = new CreateCommentCommand(newCommentContent, 0);

                await OnCommentSubmit.InvokeAsync(comment);
                newCommentContent = string.Empty;
            }
        }
    }
}