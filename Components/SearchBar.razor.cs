using FlexyBox.core.Commands.CreateTag;
using FlexyBox.core.Queries.GetCategories;
using FlexyBox.core.Queries.GetPostsIncludingDetails;
using FlexyBox.core.Queries.GetTags;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class SearchBar
    {
        [Parameter]
        public List<GetCategoryResponse> Categories { get; set; }
        [Parameter]
        public GetCategoryResponse SelectedCategory { get; set; }
        [Parameter]
        public List<GetTagsResponse> Tags { get; set; }
        [Parameter]
        public List<GetTagsResponse> SelectedTags { get; set; }
        [Parameter]
        public EventCallback<GetPostsIncludingDetailsQuery> SetGetPostsIncludingDetailsQuery_Handler { get; set; }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }
        private async Task CreatingTagsSelected_Handling(CreateTagCommand createTagCommand)
        {
            return;
        }

        private void TagDeleted_Handling(GetTagsResponse deletedTag)
        {
            //_selectedTags.Remove(deletedTag);
        }

        private void ExistingTagsSelected_Handling(GetTagsResponse existingTag)
        {
            //if (!_selectedTags.Contains(existingTag))
            //    _selectedTags.Add(existingTag);
        }

        private async Task HandleValidSubmit()
        {
            //var getPostsIncludingDetailsQuery = new GetPostsIncludingDetailsQuery()
            //{
            //    CategoryId = _selectedCategory.Id,
            //    TagIds = _selectedTags.Select(x => x.Id).ToList()
            //};

            //await SetGetPostsIncludingDetailsQuery_Handler.InvokeAsync(getPostsIncludingDetailsQuery);
        }
    }
}