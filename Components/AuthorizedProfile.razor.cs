using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;



namespace FlexyBox.web.Components
{
    public partial class AuthorizedProfile
    {
        [Parameter]
        public string UserProfilePicture { get; set; }

        [Parameter]
        public string UserName { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        private async Task LogOut_ClickHandlerAsync(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
        {
            Navigation.NavigateToLogout("authentication/logout");
        }
    }
}