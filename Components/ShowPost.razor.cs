using FlexyBox.contract.Services;
using FlexyBox.core.Commands.DeletePost;
using FlexyBox.core.Queries.GetPost;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FlexyBox.web.Components
{
    public partial class ShowPost
    {
        private string? _currentUserId;
        private bool _isLoading;
        private bool _postExists;

        [Parameter]
        public GetPostResponse Post { get; set; }

        [Inject]
        public IPostService PostService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _postExists = true;
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            _currentUserId = authState.User.FindFirst(c => c.Type == "sub")?.Value;
            await base.OnInitializedAsync();
        }
        private async Task DeletePost()
        {
            _isLoading = true;
            await PostService.DeletePost(new DeletePostCommand(Post.Id)).ExecuteAsync<int>();
            _isLoading = false;
            _postExists = false;
            NavigationManager.NavigateTo($"/");
        }
        private void EditPost(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
        {
            NavigationManager.NavigateTo($"/posts/edit/{Post.Id}");
        }
    }
}