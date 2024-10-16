using FlexyBox.core.Queries.GetPosts;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class AllPosts
    {
        [Parameter]
        public List<GetPostResponse> Posts { get; set; }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }
    }
}