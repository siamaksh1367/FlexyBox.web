using FlexyBox.contract.Services;
using FlexyBox.core.Commands.CreateTag;
using FlexyBox.core.Queries.GetCategories;
using FlexyBox.core.Queries.GetPostsIncludingDetails;
using FlexyBox.core.Queries.GetTags;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class SearchBar
    {
        [Inject]
        public ICategoryService CategoryService { get; set; }

        [Inject]
        public ITagService TagService { get; set; }

        private List<GetCategoryResponse> _categories;
        private List<GetTagsResponse> _tags;
        private List<GetTagsResponse> _selectedTags = new();
        private GetCategoryResponse _selectedCategory;

        [Parameter]
        public EventCallback<GetPostsIncludingDetailsQuery> SetGetPostsIncludingDetailsQuery_Handler { get; set; }


        private async Task CreatingTagsSelected_Handling(CreateTagCommand createTagCommand)
        {
            return;
        }

        private void TagDeleted_Handling(GetTagsResponse deletedTag)
        {
            _selectedTags.Remove(deletedTag);
        }

        private void ExistingTagsSelected_Handling(GetTagsResponse existingTag)
        {
            if (!_selectedTags.Contains(existingTag))
                _selectedTags.Add(existingTag);
        }

        private async Task HandleValidSubmit()
        {
            var getPostsIncludingDetailsQuery = new GetPostsIncludingDetailsQuery()
            {
                CategoryId = _selectedCategory.Id,
                TagIds = _selectedTags.Select(x => x.Id).ToList()
            };

            await SetGetPostsIncludingDetailsQuery_Handler.InvokeAsync(getPostsIncludingDetailsQuery);
        }
    }
}