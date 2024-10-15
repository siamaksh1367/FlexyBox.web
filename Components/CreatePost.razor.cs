using Blazored.TextEditor;
using FlexyBox.contract.Services;
using FlexyBox.core.Commands.CreatePost;
using FlexyBox.core.Commands.CreateTag;
using FlexyBox.core.Queries.GetCategories;
using FlexyBox.core.Queries.SearchTag;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FlexyBox.web.Components
{
    public partial class CreatePost
    {
        [Inject]
        public ICategoryService _categoryService { get; set; }

        [Inject]
        public ITagService _tagService { get; set; }
        [Inject]
        public IPostService _postService { get; set; }

        private BlazoredTextEditor BlazoredTextEditor;
        private CreatePostCommand _createPostCommand;
        private List<GetCategoriesResponse> _categories;
        private List<GetTagsResponse> _tags;

        private string _imagePreviewUrl { get; set; } = string.Empty;
        private List<GetTagsResponse> _selectedTags = new();
        private bool _isloading = false;

        protected override async Task OnInitializedAsync()
        {
            _isloading = true;
            _createPostCommand = new CreatePostCommand(string.Empty, string.Empty, new List<int>(), 0, null);
            _categories = await _categoryService.GetAllCategories().ExecuteAsync<List<GetCategoriesResponse>>();
            _tags = await _tagService.GetAllTags().ExecuteAsync<List<GetTagsResponse>>();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            _isloading = false;
            return base.OnAfterRenderAsync(firstRender);
        }

        private async Task CreatingTagsSelected_Handling(CreateTagCommand createTagCommand)
        {
            var createdTag = await _tagService.CreateTag(createTagCommand).ExecuteAsync<GetTagsResponse>();
            if (!_selectedTags.Contains(createdTag))
                _selectedTags.Add(createdTag);
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
        private async Task HandleImageUpload(InputFileChangeEventArgs e)
        {
            var file = e.GetMultipleFiles(1).FirstOrDefault();
            if (file != null)
            {
                using var stream = file.OpenReadStream(maxAllowedSize: 2 * 1024 * 1024); // Limit size to 2MB
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                _createPostCommand.Image = memoryStream.ToArray();
                _imagePreviewUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(_createPostCommand.Image)}";
            }
        }

        private async Task HandleValidSubmit()
        {
            var text = await BlazoredTextEditor.GetHTML();
            _createPostCommand.Content = text;
            _createPostCommand.Tags.AddRange(_selectedTags.Select(x => x.Id));
            if (!string.IsNullOrEmpty(_createPostCommand.Content))
            {
                var createdPost = await _postService.CreatePost(_createPostCommand).ExecuteAsync<CreatePostResponse>();
            }
        }
    }
}
