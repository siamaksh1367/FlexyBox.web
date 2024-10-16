
using FlexyBox.contract.Services;
using FlexyBox.core.Commands.CreateCategory;
using FlexyBox.core.Commands.DeleteCategory;
using FlexyBox.core.Commands.UpdateCategory;
using FlexyBox.core.Queries.GetCategories;
using FlexyBox.web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FlexyBox.web.Components
{
    public partial class CategoryManager
    {
        private List<GetCategoryResponse> _categories;

        [Inject]
        public ICategoryService CategoryService { get; set; }

        [Inject]
        public ITokenProvider tokenProvider { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        protected override async Task OnInitializedAsync()
        {
            _categories = await CategoryService.GetAllCategories().ExecuteAsync<List<GetCategoryResponse>>();
            await base.OnInitializedAsync();
        }

        public async Task AddCategory_Handling(CreateCategoryCommand createCategoryCommand)
        {
            var authenticationState = await authenticationStateTask;

            if (authenticationState.User.IsInRole("admin"))
            {
                var createdCategory = await CategoryService.CreateCategory(createCategoryCommand).ExecuteAsync<GetCategoryResponse>();
                _categories.Add(createdCategory);
            }
            StateHasChanged();
        }

        public async Task DeleteCategory_Handling(DeleteCategoryCommand deleteCategoryCommand)
        {
            var authenticationState = await authenticationStateTask;

            if (authenticationState.User.IsInRole("admin"))
            {
                var deletedId = await CategoryService.DeleteCategory(deleteCategoryCommand).ExecuteAsync<int>();
                _categories.RemoveAll(x => x.Id == deletedId);
            }
        }

        public async Task UpdateCategory_Handling(UpdateCategoryCommand updateCategoryCommand)
        {
            var authenticationState = await authenticationStateTask;

            if (authenticationState.User.IsInRole("admin"))
            {
                var updatedCategory = await CategoryService.UpdateCategory(updateCategoryCommand).ExecuteAsync<GetCategoryResponse>();
                _categories.RemoveAll(x => x.Id == updatedCategory.Id);
                _categories.Add(updatedCategory);
            }
        }
    }
}