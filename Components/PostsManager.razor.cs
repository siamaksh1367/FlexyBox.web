using FlexyBox.core.Queries.GetCategories;
using FlexyBox.core.Queries.GetPost;
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
        public EventCallback<GetPostsQuery> SetSearch_Handler { get; set; }
        [Parameter]
        public EventCallback<GetPostsQuery> SetPage_Handler { get; set; }

        private async Task SetSearch_Handling(GetPostsQuery getPostQuery)
        {
            Console.WriteLine("SetSearch_Handling" + string.Join(",", getPostQuery.TagIds));

            await SetSearch_Handler.InvokeAsync(getPostQuery);
        }
        private async Task SetPage_Handling(GetPostsQuery getPostQuery)
        {
            Console.WriteLine("SetPage_Handling" + getPostQuery.Offset);
            await SetPage_Handler.InvokeAsync(getPostQuery);
        }
    }
}