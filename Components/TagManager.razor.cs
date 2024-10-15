using FlexyBox.core.Commands.CreateTag;
using FlexyBox.core.Queries.GetTags;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace FlexyBox.web.Components
{
    public partial class TagManager
    {
        [Parameter]
        public List<GetTagsResponse> SelectedTags { get; set; }
        [Parameter]
        public List<GetTagsResponse> Tags { get; set; }

        [Parameter]
        public EventCallback<GetTagsResponse> ExistingTagsSelected_Handler { get; set; }

        [Parameter]
        public EventCallback<CreateTagCommand> CreatingTagsSelected_Handler { get; set; }

        [Parameter]
        public EventCallback<GetTagsResponse> TagDeleted_Handler { get; set; }

        private string _searchTerm = string.Empty;
        private const int MaxLengthAttribute = 6;
        private List<GetTagsResponse> _filteredTags = new();

        private void Chnage_Handling(ChangeEventArgs e)
        {
            _searchTerm = e.Value?.ToString() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(_searchTerm))
            {
                _filteredTags = Tags
                    .Where(item => item.Name.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) && !SelectedTags.Contains(item))
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
                if (SelectedTags.Count < MaxLengthAttribute)
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
            StateHasChanged();
        }

        private async Task SelectTag(GetTagsResponse existingTag)
        {
            if (SelectedTags.Count < MaxLengthAttribute)
            {
                await ExistingTagsSelected_Handler.InvokeAsync(existingTag);
            }
            resetSearchBar();
        }

        private async Task DeleteSelectedTag(GetTagsResponse deletedTag)
        {
            await TagDeleted_Handler.InvokeAsync(deletedTag);
        }
    }
}
