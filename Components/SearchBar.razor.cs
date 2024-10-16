using FlexyBox.core.Commands.CreateTag;
using FlexyBox.core.Queries.GetCategories;
using FlexyBox.core.Queries.GetPosts;
using FlexyBox.core.Queries.GetTags;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class SearchBar
    {
        private GetCategoryResponse _selectedCategory = new GetCategoryResponse(0, string.Empty, string.Empty);
        private List<GetTagResponse> _selectedTags = new List<GetTagResponse>();
        private GetPostQuery _getPostQuery = new GetPostQuery();

        [Parameter]
        public List<GetCategoryResponse> Categories { get; set; }
        [Parameter]
        public List<GetTagResponse> Tags { get; set; }

        [Parameter]
        public EventCallback<GetPostQuery> SetSearch_Handler { get; set; }

        private async Task CreatingTagsSelected_Handling(CreateTagCommand createTagCommand)
        {
            return;
        }

        private void TagDeleted_Handling(GetTagResponse deletedTag)
        {
            _selectedTags.Remove(deletedTag);
            StateHasChanged();
        }

        private void ExistingTagsSelected_Handling(GetTagResponse existingTag)
        {
            Console.WriteLine("ExistingTagsSelected_Handling" + string.Join(",", _selectedTags.Select(x => x.Id.ToString())));
            if (!_selectedTags.Contains(existingTag))
                _selectedTags.Add(existingTag);
            StateHasChanged();
        }
        private void Search_Changed(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
        {
            Console.WriteLine("Search_Changed" + string.Join(",", _selectedTags.Select(x => x.Id.ToString())));
            _getPostQuery.TagIds = _selectedTags.Select(x => x.Id).ToList();
            _getPostQuery.CategoryId = _selectedCategory.Id;
            SetSearch_Handler.InvokeAsync(_getPostQuery);
        }
    }
}