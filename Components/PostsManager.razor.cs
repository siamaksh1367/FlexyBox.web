using FlexyBox.core.Queries.GetCategories;
using FlexyBox.core.Queries.GetPosts;
using FlexyBox.core.Queries.GetTags;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class PostsManager
    {

        [Parameter]
        public List<GetPostResponse> Posts { get; set; }
        [Parameter]
        public int CountAll { get; set; }
        [Parameter]
        public int Offset { get; set; }
        [Parameter]
        public List<GetCategoryResponse> Categories { get; set; }
        [Parameter]
        public List<GetTagResponse> Tags { get; set; }
        [Parameter]
        public EventCallback<GetPostQuery> SetSearch_Handler { get; set; }
        [Parameter]
        public EventCallback<GetPostQuery> SetPage_Handler { get; set; }

        private async Task SetSearch_Handling(GetPostQuery getPostQuery)
        {
            await SetSearch_Handler.InvokeAsync(getPostQuery);
        }
        private async Task SetPage_Handling(GetPostQuery getPostQuery)
        {
            await SetPage_Handler.InvokeAsync(getPostQuery);
        }
    }
}