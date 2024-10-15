using FlexyBox.core.Commands.CreateCategory;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FlexyBox.web.Components
{
    public partial class AddCategory
    {
        [Parameter]
        public EventCallback<CreateCategoryCommand> AddCategory_Handler { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private CreateCategoryCommand _createCategoryCommand = new CreateCategoryCommand(string.Empty, string.Empty);

        private async Task AddCategory_Handling()
        {
            var authenticationState = await authenticationStateTask;

            if (authenticationState.User.IsInRole("admin"))
            {
                await AddCategory_Handler.InvokeAsync(_createCategoryCommand);
                _createCategoryCommand = new CreateCategoryCommand(string.Empty, string.Empty);
                StateHasChanged();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}