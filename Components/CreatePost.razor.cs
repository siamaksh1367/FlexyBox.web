using Blazored.TextEditor;
using FlexyBox.contract.Services;
using FlexyBox.core.Commands.CreatePost;
using FlexyBox.core.Commands.CreateTag;
using FlexyBox.core.Queries.GetCategories;
using FlexyBox.core.Queries.GetTags;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FlexyBox.web.Components
{
    public partial class CreatePost
    {
        [Inject]
        public ICategoryService CategoryService { get; set; }

        [Inject]
        public ITagService TagService { get; set; }
        [Inject]
        public IPostService PostService { get; set; }

        private CreatePostCommand _createPostCommand;
        private List<GetCategoryResponse> _categories;
        private List<GetTagsResponse> _tags;

        private string _imagePreviewUrl { get; set; } = string.Empty;
        private BlazoredTextEditor _blazoredTextEditor;
        private List<GetTagsResponse> _selectedTags = new();
        private bool _isloading = false;

        protected override async Task OnInitializedAsync()
        {
            _isloading = true;
            _createPostCommand = new CreatePostCommand(string.Empty, string.Empty, new List<int>(), 0, null);
            _categories = await CategoryService.GetAllCategories().ExecuteAsync<List<GetCategoryResponse>>();
            _tags = await TagService.GetAllTags().ExecuteAsync<List<GetTagsResponse>>();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            _isloading = false;
            return base.OnAfterRenderAsync(firstRender);
        }

        private async Task CreatingTagsSelected_Handling(CreateTagCommand createTagCommand)
        {
            var createdTag = await TagService.CreateTag(createTagCommand).ExecuteAsync<GetTagsResponse>();
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
            var text = await _blazoredTextEditor.GetHTML();
            _createPostCommand.Content = text;
            _createPostCommand.Tags.AddRange(_selectedTags.Select(x => x.Id));
            if (!string.IsNullOrEmpty(_createPostCommand.Content))
            {
                var createdPost = await PostService.CreatePost(_createPostCommand).ExecuteAsync<CreatePostResponse>();
            }
        }
    }
}
