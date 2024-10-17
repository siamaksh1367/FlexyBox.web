using Blazored.Toast.Services;
using FlexyBox.contract.Services;
using FlexyBox.core.Commands.CreateComment;
using FlexyBox.core.Queries.GetComments;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class CommentManager
    {
        [Parameter]
        public List<GetCommentResponse> Comments { get; set; }

        [Parameter]
        public int PostId { get; set; }
        [Parameter]
        public EventCallback<CreateCommentCommand> CreateCommand_Handler { get; set; }

        [Inject]
        public ICommentService CommentService { get; set; }

        public async Task CreateCommand_Handling(CreateCommentCommand command)
        {
            command.PostId = PostId;
            await CreateCommand_Handler.InvokeAsync(command);
            ToastService.ShowSuccess("Your action was successful!");
        }
    }
}