// TagManager.razor.cs
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
        private const int MaxLengthAttribute = 6;

        [Inject]
        public ITagService TagService { get; set; }

        [Parameter]
        public EventCallback<List<int>> OnTagsSelected { get; set; }

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
            if (e.Key == "Enter" && !string.IsNullOrEmpty(searchTerm))
            {
                var existingTag = _tags.FirstOrDefault(t => t.Name.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));
                if (existingTag != null)
                {
                    SelectItem(existingTag);
                }
                else
                {
                    var newTag = new CreateTagCommand(searchTerm);
                    var addedTag = await TagService.CreateTag(newTag).ExecuteAsync<GetAllTagsResponse>();
                    _tags.Add(addedTag);
                    _selectedTags.Add(addedTag);
                    searchTerm = string.Empty;
                    _filteredTags.Clear();
                }
            }
        }

        private async Task SelectItem(GetAllTagsResponse item)
        {
            if (_selectedTags.Count < MaxLengthAttribute && !_selectedTags.Any(t => t.Name == item.Name))
            {
                _selectedTags.Add(item);
                await OnTagsSelected.InvokeAsync(_selectedTags.Select(t => t.Id).ToList());
                searchTerm = string.Empty;
                _filteredTags.Clear();
            }
        }

        private void DeleteSelectedTag(GetAllTagsResponse tag)
        {
            _selectedTags.Remove(tag);
            OnTagsSelected.InvokeAsync(_selectedTags.Select(t => t.Id).ToList());
        }
    }
}
