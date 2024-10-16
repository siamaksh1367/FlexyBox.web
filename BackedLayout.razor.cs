using Microsoft.AspNetCore.Components;

namespace FlexyBox.web
{
    public partial class BackedLayout
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public void GoBack_Handling()
        {
            NavigationManager.NavigateTo("javascript:history.back()");
        }
    }
}