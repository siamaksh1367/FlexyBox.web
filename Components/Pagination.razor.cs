using FlexyBox.core.Queries.GetPosts;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class Pagination
    {
        private int _PageCount;
        private List<int> _numbers;

        [Parameter]
        public int Offset { get; set; }
        [Parameter]
        public int Limit { get; set; }
        [Parameter]
        public int CountAll { get; set; }

        [Parameter]
        public EventCallback<GetPostQuery> SetPage_Handler { get; set; }

        protected override Task OnInitializedAsync()
        {
            _PageCount = (CountAll / Limit) + 1;
            _numbers = Enumerable.Range(0, _PageCount).ToList();
            return base.OnInitializedAsync();
        }
        private async Task Page_ChangedAsync(int selectedPage)
        {
            Console.WriteLine(selectedPage);
            await SetPage_Handler.InvokeAsync(new GetPostQuery() { Offset = selectedPage });
        }
    }


}