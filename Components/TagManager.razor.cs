using FlexyBox.contract.Services;
using FlexyBox.core.Commands.CreateTag;
using FlexyBox.core.Queries.SearchTag;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace FlexyBox.web.Components
{
    public partial class TagManager
    {
        private string searchTerm = string.Empty;
        private List<GetAllTagsResponse> _tags = new();
        private List<GetAllTagsResponse> _filteredTags = new();
        private List<GetAllTagsResponse> _selectedTags = new();
        [Inject]
        public ITagService TagService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadItemsAsync();
        }

        private async Task LoadItemsAsync()
        {
            _tags = await TagService.GetAllTag().ExecuteAsync<List<GetAllTagsResponse>>();
        }

        private void OnInputChange(ChangeEventArgs e)
        {
            searchTerm = e.Value?.ToString() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                _filteredTags = _tags
                    .Where(item => item.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                _filteredTags.Clear();
            }
        }

        private async Task HandleKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                var existingTag = _tags.FirstOrDefault(t => t.Name.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));
                if (existingTag != null)
                {
                    // If tag exists, just add it to the selected list
                    SelectItem(existingTag);
                }
                else
                {
                    // Add new tag to the database
                    var newTag = new CreateTagCommand(searchTerm);
                    var addedTag = await TagService.CreateTag(newTag).ExecuteAsync<GetAllTagsResponse>(); // Implement this method in your service

                    // Add the new tag to the selected list
                    _selectedTags.Add(addedTag);
                    searchTerm = string.Empty;
                    _filteredTags.Clear(); // Clear the dropdown
                }
            }
        }

        private void SelectItem(GetAllTagsResponse item)
        {
            // Prevent adding duplicates
            if (!_selectedTags.Any(t => t.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase)))
            {
                _selectedTags.Add(item);
            }
            searchTerm = string.Empty;
            _filteredTags.Clear(); // Hide the dropdown
        }
    }
}