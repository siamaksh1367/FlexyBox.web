using Blazored.TextEditor;
using FlexyBox.contract.Services;
using FlexyBox.core.Commands.CreatePost;
using FlexyBox.core.Queries.GetCategories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FlexyBox.web.Components
{
    public partial class CreatePost
    {
        [Inject]
        public ICategoryService categoryService { get; set; }
        [Inject]
        public IPostService postService { get; set; }

        private CreatePostCommand CreatePostCommand;
        private List<GetCategoriesResponse> _categories;
        private BlazoredTextEditor BlazoredTextEditor;
        private string ImagePreviewUrl { get; set; } = string.Empty;
        private List<int> selectedTags = new();
        private bool isloading = false;

        protected override async Task OnInitializedAsync()
        {
            isloading = true;
            CreatePostCommand = new CreatePostCommand(string.Empty, string.Empty, new List<int>(), 0, null);
            _categories = await categoryService.GetAllCategories().ExecuteAsync<List<GetCategoriesResponse>>();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            isloading = false;
            return base.OnAfterRenderAsync(firstRender);
        }

        private void UpdateSelectedTags(List<int> tags)
        {
            CreatePostCommand.Tags = tags;
        }

        private void UpdateContent(string content)
        {
            CreatePostCommand.Content = content;
        }

        private async Task HandleImageUpload(InputFileChangeEventArgs e)
        {
            var file = e.GetMultipleFiles(1).FirstOrDefault();
            if (file != null)
            {
                using var stream = file.OpenReadStream(maxAllowedSize: 2 * 1024 * 1024); // Limit size to 2MB
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                // Store the image data in byte array
                CreatePostCommand.Image = memoryStream.ToArray();

                // Generate image preview URL
                ImagePreviewUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(CreatePostCommand.Image)}";
            }
        }

        private async Task HandleValidSubmit()
        {
            var text = await BlazoredTextEditor.GetHTML();
            CreatePostCommand.Content = text;

            if (!string.IsNullOrEmpty(CreatePostCommand.Content))
            {
                var createdPost = await postService.CreatePost(CreatePostCommand).ExecuteAsync<CreatePostResponse>();
            }
        }
    }
}
