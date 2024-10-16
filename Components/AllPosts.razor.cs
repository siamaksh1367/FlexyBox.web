using FlexyBox.contract.Services;
using FlexyBox.core.Queries.GetCategories;
using FlexyBox.core.Queries.GetPostsIncludingDetails;
using FlexyBox.core.Queries.GetTags;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class AllPosts
    {
        [Inject]
        public IPostService PostService { get; set; }

        private List<GetPostsIncludingDetailsResponse> _posts;
        private List<GetTagsResponse> _selectedTags = new();
        private GetCategoryResponse _selectedCategory = null;
        private bool _isloading;

        protected override async Task OnInitializedAsync()
        {
            _isloading = true;
            _posts = await PostService.GetPostsIncludingDetails().ExecuteAsync<List<GetPostsIncludingDetailsResponse>>();
            await base.OnInitializedAsync();
        }
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            _isloading = false;
            return base.OnAfterRenderAsync(firstRender);
        }
        private async Task SetSearch_Handling(GetPostsIncludingDetailsQuery getPostsIncludingDetailsQuery)
        {
            _posts = await PostService.GetPostsIncludingDetailsWithCriteria(getPostsIncludingDetailsQuery).ExecuteAsync<List<GetPostsIncludingDetailsResponse>>();
        }
    }
}