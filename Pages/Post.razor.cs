using FlexyBox.contract.Services;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Pages
{
    public partial class Post
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        public IPostService PostService { get; set; }
        private int PostId;

        protected override async Task OnParametersSetAsync()
        {
            if (int.TryParse(Id, out int postId))
            {
                PostId = postId;

                //var post = await PostService.GetOnePostWithAllDetails(PostId).ExecuteAsync<GetPostQuery>();
            }
            else
            {
                // Handle invalid 'id' scenario
            }
        }
    }
}