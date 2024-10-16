using FlexyBox.core.Queries.GetPostsIncludingDetails;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FlexyBox.web.Components
{
    public partial class PostCard
    {
        [Parameter]
        public GetPostsIncludingDetailsResponse GetPostsIncludingDetailsResponse { get; set; }

        private void ShowModal()
        {
            JSRuntime.InvokeVoidAsync("bootstrapModal.show");
        }
        private void NavigateToPostDetails()
        {
            NavigationManager.NavigateTo($"/post/{GetPostsIncludingDetailsResponse.Id}");
        }
    }
}