using Blazored.TextEditor;
using FlexyBox.contract.Services;
using FlexyBox.core.Commands.CreateComment;
using FlexyBox.core.Commands.CreatePost;
using FlexyBox.core.Commands.CreateTag;
using FlexyBox.core.Queries.GetCategories;
using FlexyBox.core.Queries.GetPost;
using FlexyBox.core.Queries.GetTags;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FlexyBox.web.Components
{
    public partial class ModifyPost
    {
        [Inject]
        public ICategoryService CategoryService { get; set; }
        [Inject]
        public ITagService TagService { get; set; }
        [Inject]
        public IPostService PostService { get; set; }

        [Parameter]
        public GetPostResponse? GetPostResponse { get; set; }

        private CreatePostCommand _createPostCommand;
        private List<GetCategoryResponse> _categories;
        private List<GetTagResponse> _tags;

        private string _imagePreviewUrl { get; set; } = string.Empty;
        private BlazoredTextEditor _blazoredTextEditor;
        private List<GetTagResponse> _selectedTags = new();
        private bool _isloading = false;

        protected override async Task OnInitializedAsync()
        {
            _isloading = true;
            _createPostCommand = new CreatePostCommand(string.Empty, string.Empty, new List<int>(), 0, null);
            _categories = await CategoryService.GetAllCategories().ExecuteAsync<List<GetCategoryResponse>>();
            _tags = await TagService.GetAllTags().ExecuteAsync<List<GetTagResponse>>();
            if (GetPostResponse != null)
            {
                _selectedTags = _tags.Where(x => GetPostResponse.Tags.Contains(x.Name)).ToList();
                _createPostCommand.Content = GetPostResponse.Content;
                _createPostCommand.CategoryId = _categories.FirstOrDefault(x => GetPostResponse.Category == x.Name).Id;
                _createPostCommand.Title = GetPostResponse.Title;
                _createPostCommand.Image = GetPostResponse.Image;
                _imagePreviewUrl = $"data:image/png;base64,{Convert.ToBase64String(_createPostCommand.Image)}";
            }

        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            _isloading = false;
            return base.OnAfterRenderAsync(firstRender);
        }

        private async Task CreatingTagsSelected_Handling(CreateTagCommand createTagCommand)
        {
            var createdTag = await TagService.CreateTag(createTagCommand).ExecuteAsync<GetTagResponse>();
            if (!_selectedTags.Contains(createdTag))
                _selectedTags.Add(createdTag);
            _tags.Add(createdTag);
            StateHasChanged();
        }

        private void TagDeleted_Handling(GetTagResponse deletedTag)
        {
            _selectedTags.Remove(deletedTag);
            StateHasChanged();
        }

        private void ExistingTagsSelected_Handling(GetTagResponse existingTag)
        {
            if (!_selectedTags.Contains(existingTag))
                _selectedTags.Add(existingTag);
            StateHasChanged();
        }
        private async Task HandleImageUpload(InputFileChangeEventArgs e)
        {
            var file = e.GetMultipleFiles(1).FirstOrDefault();
            if (file != null)
            {
                using var stream = file.OpenReadStream(maxAllowedSize: 2 * 1024 * 1024);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                _createPostCommand.Image = memoryStream.ToArray();
                _imagePreviewUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(_createPostCommand.Image)}";
            }
        }

        private async Task Save_Handler()
        {
            var text = await _blazoredTextEditor.GetHTML();
            _createPostCommand.Content = text;
            _createPostCommand.Tags.AddRange(_selectedTags.Select(x => x.Id));
            var postId = 0;
            if (!string.IsNullOrEmpty(_createPostCommand.Content))
            {
                if (GetPostResponse == null)
                {
                    var createdPost = await PostService.CreatePost(_createPostCommand).ExecuteAsync<CreatePostResponse>();
                    postId = createdPost.Id;
                }
                else
                {
                    var updatePostCommand = new UpdatePostCommand(GetPostResponse.Id, _createPostCommand.Title, text, _selectedTags.Select(x => x.Id).ToList(), _createPostCommand.CategoryId, _createPostCommand.Image);
                    var editedPostId = await PostService.Update(updatePostCommand).ExecuteAsync<int>();
                    postId = editedPostId;
                }
                ToastService.ShowSuccess("Your action was successful!");
                NavigationManager.NavigateTo($"/post/{postId}");

            }
        }
    }
}
