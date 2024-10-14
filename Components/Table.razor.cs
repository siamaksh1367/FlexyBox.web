using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace FlexyBox.web.Components
{
    public partial class Table<TDataItem, TEditItem>
    {
        private IEnumerable<PropertyInfo> _editProperties;

        [Parameter]
        public List<TableDataWrapper<TDataItem>> Items { get; set; } = new List<TableDataWrapper<TDataItem>>();
        [Parameter]
        public List<string> Headers { get; set; } = new List<string>();
        [Parameter]
        public List<Func<TDataItem, PropertyObject>> ItemProperties { get; set; } = new List<Func<TDataItem, PropertyObject>>();
        [Parameter]
        public bool ShowActions { get; set; } = true;


        [Parameter]
        public EventCallback<TDataItem> OnUpdate { get; set; }
        [Parameter]
        public EventCallback<TDataItem> OnDelete { get; set; }
        [Parameter]
        public TEditItem EditItem { get; set; }

        private void OnEditModeClicked(TableDataWrapper<TDataItem> editingItem)
        {
            foreach (var item in Items)
                item.EditMode = false;
            editingItem.EditMode = true;
            foreach (var property in _editProperties)
            {
                foreach (var item in ItemProperties)
                {
                    var PropertyObject = item.Invoke(editingItem.Item);
                    if (property.Name == PropertyObject.MappingProperty)
                    {
                        Console.WriteLine(property.Name);
                        SetStringPropertyValue(property, PropertyObject.Value);
                    }
                }
            }
            StateHasChanged();
        }

        private void OnCancelClicked()
        {
            foreach (var item in Items)
                item.EditMode = false;
        }

        private void OnDeleteClicked(TableDataWrapper<TDataItem> item)
        {
            OnDelete.InvokeAsync(item.Item);
        }
        private void OnUpdateClicked(TableDataWrapper<TDataItem> item)
        {
            OnUpdate.InvokeAsync(item.Item);
        }


        [Parameter]
        public EventCallback<TEditItem> OnValidSubmit { get; set; }

        protected override void OnInitialized()
        {
            _editProperties = typeof(TEditItem).GetProperties()
                                 .Where(p => p.CanRead && p.CanWrite);
            base.OnInitialized();
        }

        private string GetStringPropertyValue(PropertyInfo property)
        {
            if (EditItem == null)
            {
                Console.WriteLine("thi is null");
            }
            return (string)property.GetValue(EditItem) ?? string.Empty;
        }


        private void SetStringPropertyValue(PropertyInfo property, string value)
        {
            if (property.PropertyType == typeof(string))
            {
                property.SetValue(EditItem, value);
            }
            else if (property.PropertyType == typeof(int) && int.TryParse(value, out int intValue))
            {
                property.SetValue(EditItem, intValue);
            }

        }

        public class TableDataWrapper<TDataItem>
        {
            public TDataItem Item { get; set; }
            public bool EditMode { get; set; }
        }

        public class PropertyObject
        {
            public string Value { get; set; }
            public string MappingProperty { get; set; }
            public bool Editable { get; set; }
            public string Type { get; set; }
        }
    }
}