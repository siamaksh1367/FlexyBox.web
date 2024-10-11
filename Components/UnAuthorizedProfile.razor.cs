using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace FlexyBox.web.Components
{
    public partial class UnAuthorizedProfile
    {
        [Inject]
        public NavigationManager Navigation { get; set; }

        private async Task Login_ClickHandler(MouseEventArgs args)
        {
            Navigation.NavigateToLogin("/authentication/login");
        }
    }
}