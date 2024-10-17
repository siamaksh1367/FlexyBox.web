using FlexyBox.web.Components.Models.Table;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace FlexyBox.web.Components
{
    public partial class Table<RowDataType, RowEditType>
    {
        [Parameter]
        public List<RowDataType> Rows { get; set; }
        [Parameter]
        public List<string> Headers { get; set; } = new List<string>();
        [Parameter]
        public List<Func<RowDataType, Column>> RowDataColumns { get; set; }
        [Parameter]
        public bool ShowActions { get; set; } = true;
        [Parameter]
        public EventCallback<RowEditType> Update_Handler { get; set; }
        [Parameter]
        public EventCallback<RowDataType> Delete_Handler { get; set; }
        [Parameter]
        public RowEditType RowEditItem { get; set; }

        private List<PropertyInfo> _rowEditTypeColumns = new List<PropertyInfo>();
        private List<Row<RowDataType>> _rowsWithEditFlag = new List<Row<RowDataType>>();
        protected override Task OnInitializedAsync()
        {
            _rowEditTypeColumns = typeof(RowEditType).GetProperties()
                        .Where(p => p.CanRead && p.CanWrite).ToList();
            _rowsWithEditFlag = Rows.Select(x => new Row<RowDataType>()
            {
                Data = x,
                EditMode = false
            }).ToList();
            return base.OnInitializedAsync();
        }

        private void Start_Editing(Row<RowDataType> editingRow)
        {
            for (int i = 0; i < _rowsWithEditFlag.Count(); i++)
            {
                var row = _rowsWithEditFlag[i]; ;

                if (Equals(row.Data, editingRow.Data))
                    _rowsWithEditFlag[i].EditMode = true;
                else
                    _rowsWithEditFlag[i].EditMode = false;
            }

            foreach (var rowEditTypeColumn in _rowEditTypeColumns)
            {
                foreach (var RowDataColumn in RowDataColumns)
                {
                    var column = RowDataColumn.Invoke(editingRow.Data);
                    if (rowEditTypeColumn.Name == column.EditPropertyName)
                    {
                        SetStringPropertyValue(rowEditTypeColumn, column.Value);
                    }
                }
            }
            StateHasChanged();
        }
        protected override Task OnParametersSetAsync()
        {
            _rowEditTypeColumns = typeof(RowEditType).GetProperties()
                        .Where(p => p.CanRead && p.CanWrite).ToList();
            _rowsWithEditFlag = Rows.Select(x => new Row<RowDataType>()
            {
                Data = x,
                EditMode = false
            }).ToList();
            return base.OnParametersSetAsync();
        }
        private void Cancel_Editing()
        {
            foreach (var row in _rowsWithEditFlag)
                row.EditMode = false;
            StateHasChanged();
        }

        private async Task Delete_Handling(RowDataType deleteItem)
        {
            await Delete_Handler.InvokeAsync(deleteItem);
        }

        private async Task Update_Handling(RowEditType editItem)
        {
            await Update_Handler.InvokeAsync(editItem);
        }

        private void SetStringPropertyValue(PropertyInfo property, string value)
        {
            if (property.PropertyType == typeof(string))
            {
                property.SetValue(RowEditItem, value);
            }
            else if (property.PropertyType == typeof(int) && int.TryParse(value, out int intValue))
            {
                property.SetValue(RowEditItem, intValue);
            }
            StateHasChanged();
        }
    }
}