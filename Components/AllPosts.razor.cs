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
        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public ITagService TagService { get; set; }

        private GetPostsIncludingDetailsQuery _getPostsIncludingDetailsQuery = new();
        private List<GetPostsIncludingDetailsResponse> _posts;
        private List<GetTagsResponse> _tags;
        private bool _isloading;
        private List<GetCategoryResponse> _categories;
        private List<GetTagsResponse> _selectedTags;
        private GetCategoryResponse _selectedCategory;

        protected override async Task OnInitializedAsync()
        {
            _isloading = true;
            _getPostsIncludingDetailsQuery.Offset = 0;
            _getPostsIncludingDetailsQuery.Limit = 6;
            _posts = await PostService.GetPostsIncludingDetails().ExecuteAsync<List<GetPostsIncludingDetailsResponse>>();
            _tags = await TagService.GetAllTags().ExecuteAsync<List<GetTagsResponse>>();
            _categories = await CategoryService.GetAllCategories().ExecuteAsync<List<GetCategoryResponse>>();
            _selectedTags = new List<GetTagsResponse>();
            _selectedCategory = new GetCategoryResponse(0, string.Empty, string.Empty);
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