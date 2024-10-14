using FlexyBox.contract.Services;
using FlexyBox.core.Commands.CreateCategory;
using FlexyBox.web.Services;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class AddCategory
    {
        [Inject]
        public ICategoryService categoryService { get; set; }
        [Inject]
        public ITokenProvider tokenProvider { get; set; }

        private CreateCategoryCommand createCategoryCommand = new CreateCategoryCommand(string.Empty, string.Empty);

        [Parameter]
        public EventCallback OnCategoryAdded { get; set; }

        private async Task HandleValidSubmitAsync()
        {
            var accessToken = await tokenProvider.GetAccessTokenAsync();
            await categoryService.CreateCategory(createCategoryCommand)
                .ExecuteAsync<CreateCategoryResponse>();

            // Notify the parent component that a category was added
            await OnCategoryAdded.InvokeAsync(null);

            // Reset the form
            createCategoryCommand = new CreateCategoryCommand(string.Empty, string.Empty);
        }
    }
}