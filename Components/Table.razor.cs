using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class Table<TItem>
    {
        [Parameter]
        public List<TItem> Items { get; set; } = new List<TItem>();
        [Parameter]
        public List<string> Headers { get; set; } = new List<string>();
        [Parameter]
        public List<Func<TItem, object>> ItemProperties { get; set; } = new List<Func<TItem, object>>();
        [Parameter]
        public bool ShowActions { get; set; } = true;


        [Parameter]
        public EventCallback<TItem> OnEdit { get; set; }
        [Parameter]
        public EventCallback<TItem> OnDelete { get; set; }


        private void OnEditClicked(TItem item)
        {
            OnEdit.InvokeAsync(item);
        }

        private void OnDeleteClicked(TItem item)
        {
            OnDelete.InvokeAsync(item);
        }
    }
}