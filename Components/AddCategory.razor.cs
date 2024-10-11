using FlexyBox.core.Commands.CreateCategory;

namespace FlexyBox.web.Components
{
    public partial class AddCategory
    {
        private CreateCategoryCommand createCategoryCommand = new CreateCategoryCommand(string.Empty, string.Empty);

        private void HandleValidSubmit()
        {
            createCategoryCommand = new CreateCategoryCommand(string.Empty, string.Empty);
        }
    }
}