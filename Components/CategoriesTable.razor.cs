
using FlexyBox.contract.Services;
using FlexyBox.core.Commands.DeleteCategory;
using FlexyBox.core.Commands.UpdateCategory;
using FlexyBox.core.Queries.GetCategories;
using FlexyBox.web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using static FlexyBox.web.Components.Table<FlexyBox.core.Queries.GetCategories.GetCategoriesResponse, FlexyBox.core.Commands.UpdateCategory.UpdateCategoryCommand>;

namespace FlexyBox.web.Components
{
    public partial class CategoriesTable
    {
        private List<GetCategoriesResponse> _categories;
        private List<string> _headers = new List<string> { "Id", "Name", "Description" };
        private List<Func<GetCategoriesResponse, PropertyObject>> _itemProperties = new List<Func<GetCategoriesResponse, PropertyObject>>
        {
            x=>new PropertyObject(){MappingProperty="Id",Value= x.Id.ToString(),Editable=false },
            x=>new PropertyObject(){MappingProperty="Name", Value= x.Name,Editable=true,Type="text"},
            x=>new PropertyObject(){MappingProperty="Description",Value= x.Description,Editable=true,Type="text"}
        };
        private UpdateCategoryCommand _updateCategoryCommand = new UpdateCategoryCommand(0, string.Empty, string.Empty);

        [Inject]
        public ICategoryService categoryService { get; set; }
        [Inject]
        public ITokenProvider tokenProvider { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private bool isloading = false;
        protected override async Task<Task> OnInitializedAsync()
        {
            isloading = true;
            await getCategories();
            return base.OnInitializedAsync();
        }

        public async Task getCategories()
        {
            _categories = (await categoryService.GetAllCategories()
                .ExecuteAsync<IEnumerable<GetCategoriesResponse>>()).ToList();
            StateHasChanged();
            isloading = false;
        }

        public async Task HandleEdit(UpdateCategoryCommand updateCategoryCommand)
        {
            var authenticationState = await authenticationStateTask;

            if (authenticationState.User.IsInRole("admin"))
            {
                isloading = true;
                var editedCategory = (await categoryService.UpdateCategory(updateCategoryCommand)
                    .ExecuteAsync<UpdateCategoryResponse>());
                await getCategories();
            }
        }

        public async Task HandleDelete(DeleteCategoryCommand deleteCategoryCommand)
        {
            var authenticationState = await authenticationStateTask;

            if (authenticationState.User.IsInRole("admin"))
            {
                isloading = true;
                var deletedCategory = (await categoryService.DeleteCategory(deleteCategoryCommand)
                    .ExecuteAsync<int>());
                await getCategories();
            }
        }
    }
}