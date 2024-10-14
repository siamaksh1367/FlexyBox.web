
using FlexyBox.contract.Services;
using FlexyBox.core.Queries.GetCategories;
using FlexyBox.web.Services;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class CategoriesTable
    {
        private List<GetCategoriesResponse> _categories;
        private List<string> _headers = new List<string> { "Id", "Name", "Description" };
        private List<Func<GetCategoriesResponse, object>> _itemProperties = new List<Func<GetCategoriesResponse, object>>
        {
            x=>x.Id.ToString(),
            x=>x.Name,
            x=>x.Description,
        };

        [Inject]
        public ICategoryService categoryService { get; set; }
        [Inject]
        public ITokenProvider tokenProvider { get; set; }
        protected override async Task<Task> OnInitializedAsync()
        {
            var accessToken = await tokenProvider.GetAccessTokenAsync();
            _categories = (await categoryService.GetAllCategories()
                .AddBearerToken(accessToken)
                .ExecuteAsync<IEnumerable<GetCategoriesResponse>>()).ToList();
            return base.OnInitializedAsync();
        }

        public async Task HanldeEdit(GetCategoriesResponse getCategoriesResponse)
        {
            Console.WriteLine("Edited");
        }

        public async Task HanldeDelete(GetCategoriesResponse getCategoriesResponse)
        {
            Console.WriteLine("Deleted");
        }
    }
}