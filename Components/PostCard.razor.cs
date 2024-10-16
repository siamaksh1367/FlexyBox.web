using FlexyBox.core.Queries.GetPosts;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class PostCard
    {
        [Parameter]
        public GetPostResponse GetPostsIncludingDetailsResponse { get; set; }

        private void NavigateToPostDetails()
        {
            NavigationManager.NavigateTo($"/post/{GetPostsIncludingDetailsResponse.Id}");
        }
    }
}