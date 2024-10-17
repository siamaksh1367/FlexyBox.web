using FlexyBox.core.Commands.DeleteCategory;
using FlexyBox.core.Commands.UpdateCategory;
using FlexyBox.core.Queries.GetCategories;
using FlexyBox.web.Components.Models.Table;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class CategoriesTable
    {
        [Parameter]
        public List<GetCategoryResponse> Categories { get; set; }

        [Parameter]
        public EventCallback<DeleteCategoryCommand> DeleteCategory_Handler { get; set; }

        [Parameter]
        public EventCallback<UpdateCategoryCommand> UpdateCategory_Handler { get; set; }


        private List<string> _headers = new List<string> { "Id", "Name", "Description" };
        private UpdateCategoryCommand _updateCategoryCommand = new UpdateCategoryCommand(0, string.Empty, string.Empty);
        private bool isloading = false;
        private List<Func<GetCategoryResponse, Column>> _rowDataColumns = new List<Func<GetCategoryResponse, Column>>
        {
            x=>new Column(){EditPropertyName="Id",Value= x.Id.ToString(),Editable=false },
            x=>new Column(){EditPropertyName="Name", Value= x.Name,Editable=true,Type="text"},
            x=>new Column(){EditPropertyName="Description",Value= x.Description,Editable=true,Type="text"}
        };
        public async Task UpdateCategory_Handling(UpdateCategoryCommand updateCategoryCommand)
        {
            isloading = true;
            await UpdateCategory_Handler.InvokeAsync(updateCategoryCommand);
            ToastService.ShowSuccess("Your action was successful!");
            isloading = false;
        }

        public async Task DeleteCategory_Handling(GetCategoryResponse getCategoryResponse)
        {
            isloading = true;
            var deleteCategoryCommand = new DeleteCategoryCommand(getCategoryResponse.Id);
            await DeleteCategory_Handler.InvokeAsync(deleteCategoryCommand);
            ToastService.ShowSuccess("Your action was successful!");
            isloading = false;

        }
    }
}