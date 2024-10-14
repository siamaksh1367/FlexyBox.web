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
        private CreatePostCommand CreatePostCommand;
        private List<GetCategoriesResponse> _categories;
        private BlazoredTextEditor QuillHtml;
        private string ImagePreviewUrl { get; set; } = string.Empty;
        private List<int> selectedTags = new();
        [Inject]
        public ICategoryService categoryService { get; set; }
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
                // Set the uploaded file to CreatePostCommand
                CreatePostCommand.Image = file;

                // Create a FileReader to read the file as a base64 string
                using var stream = file.OpenReadStream(maxAllowedSize: 2 * 1024 * 1024); // Limit size to 2MB
                var buffer = new byte[file.Size];
                await stream.ReadAsync(buffer);

                // Convert to base64 string for preview
                ImagePreviewUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";
            }
        }

        private async Task HandleValidSubmit()
        {
            // Continue with creating the post, using CreatePostCommand
            // Example: await _postService.CreatePost(CreatePostCommand);
        }
    }
}
