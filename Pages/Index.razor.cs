using FlexyBox.common;
using FlexyBox.contract.Services;
using FlexyBox.core.Queries.GetCategories;
using FlexyBox.core.Queries.GetPost;
using FlexyBox.core.Queries.GetPosts;
using FlexyBox.core.Queries.GetTags;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Pages
{
    public partial class Index
    {
        [Inject]
        public IPostService PostService { get; set; }
        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public ITagService TagService { get; set; }

        private WithCount<GetPostResponse> _posts;
        private List<GetTagResponse> _tags;
        private List<GetCategoryResponse> _categories;

        private GetPostsQuery _getPostQuery = new();
        private bool _isLoading;

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            _getPostQuery.Offset = 0;
            _getPostQuery.Limit = 6;

            _tags = await TagService.GetAllTags().ExecuteAsync<List<GetTagResponse>>();
            _categories = await CategoryService.GetAllCategories().ExecuteAsync<List<GetCategoryResponse>>();
            _posts = await PostService.GetPosts(_getPostQuery).ExecuteAsync<WithCount<GetPostResponse>>();

            await base.OnInitializedAsync();
        }
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            _isLoading = false;
            return base.OnAfterRenderAsync(firstRender);
        }

        public async Task SetSearch_Handling(GetPostsQuery getPostQuery)
        {
            Console.WriteLine("index:SetSearch_Handling" + string.Join(",", getPostQuery.TagIds));
            _isLoading = true;
            _getPostQuery.TagIds = getPostQuery.TagIds;
            _getPostQuery.CategoryId = getPostQuery.CategoryId;
            _posts = await PostService.GetPosts(_getPostQuery).ExecuteAsync<WithCount<GetPostResponse>>();
            _isLoading = false;
        }

        public async Task SetPage_Handling(GetPostsQuery getPostQuery)
        {
            Console.WriteLine("index:SetPage_Handling" + getPostQuery.Offset);
            _isLoading = true;
            _getPostQuery.Offset = getPostQuery.Offset;
            _posts = await PostService.GetPosts(_getPostQuery).ExecuteAsync<WithCount<GetPostResponse>>();
            _isLoading = false;
        }


    }
}