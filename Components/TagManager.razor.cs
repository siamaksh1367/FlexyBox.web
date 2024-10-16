using FlexyBox.core.Commands.CreateTag;
using FlexyBox.core.Queries.GetTags;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace FlexyBox.web.Components
{
    public partial class TagManager
    {
        private string _searchTerm = string.Empty;
        private const int MaxLengthAttribute = 6;
        private List<GetTagResponse> _filteredTags = new();
        private List<GetTagResponse> _selectedTags = new();
        [Parameter]
        public List<GetTagResponse> Tags { get; set; }

        [Parameter]
        public List<GetTagResponse> SelectedTags { get; set; }

        [Parameter]
        public EventCallback<GetTagResponse> ExistingTagsSelected_Handler { get; set; }

        [Parameter]
        public EventCallback<CreateTagCommand> CreatingTagsSelected_Handler { get; set; }

        [Parameter]
        public EventCallback<GetTagResponse> TagDeleted_Handler { get; set; }


        private void Change_Handling(ChangeEventArgs e)
        {
            _searchTerm = e.Value?.ToString() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(_searchTerm))
            {
                _filteredTags = Tags
                    .Where(item => item.Name.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) && !_selectedTags.Contains(item))
                    .ToList();
            }
            else
            {
                _filteredTags.Clear();
            }
        }

        private async Task KeyDown_Handling(KeyboardEventArgs e)
        {
            if (e.Key == "Enter" && !string.IsNullOrEmpty(_searchTerm))
            {
                if (_selectedTags.Count < MaxLengthAttribute)
                {
                    var existingTag = Tags.FirstOrDefault(t => t.Name.Equals(_searchTerm, StringComparison.OrdinalIgnoreCase));
                    if (existingTag != null)
                    {
                        await ExistingTagsSelected_Handler.InvokeAsync(existingTag);
                    }
                    else
                    {
                        var creatingTag = new CreateTagCommand(_searchTerm);
                        await CreatingTagsSelected_Handler.InvokeAsync(creatingTag);
                    }
                }
                resetSearchBar();
            }
        }

        private void resetSearchBar()
        {
            _searchTerm = string.Empty;
            _filteredTags.Clear();
        }

        private async Task SelectTag(GetTagResponse existingTag)
        {
            if (_selectedTags.Count < MaxLengthAttribute)
            {
                await ExistingTagsSelected_Handler.InvokeAsync(existingTag);
            }
            resetSearchBar();
        }

        private async Task DeleteSelectedTag(GetTagResponse deletedTag)
        {
            await TagDeleted_Handler.InvokeAsync(deletedTag);
        }
    }
}
