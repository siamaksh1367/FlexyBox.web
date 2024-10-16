using FlexyBox.core.Queries.GetPostsIncludingDetails;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class PostCard
    {
        [Parameter]
        public GetPostsIncludingDetailsResponse GetPostsIncludingDetailsResponse { get; set; }

        private void NavigateToPostDetails()
        {
            NavigationManager.NavigateTo($"/post/{GetPostsIncludingDetailsResponse.Id}");
        }
    }
}