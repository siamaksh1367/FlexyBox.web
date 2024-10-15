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

        private IEnumerable<PropertyInfo> _rowEditTypeColumns;
        private IEnumerable<Row<RowDataType>> _rowsWithEditFlag;

        protected override void OnInitialized()
        {
            _rowEditTypeColumns = typeof(RowEditType).GetProperties()
                                 .Where(p => p.CanRead && p.CanWrite);
            _rowsWithEditFlag = Rows.Select(x => new Row<RowDataType>()
            {
                Data = x,
                EditMode = false
            });

            base.OnInitialized();
        }
        private void Start_Editing(Row<RowDataType> editingRow)
        {
            foreach (var row in _rowsWithEditFlag)
                row.EditMode = false;
            editingRow.EditMode = true;


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

        private void Cancel_Editing()
        {
            foreach (var row in _rowsWithEditFlag)
                row.EditMode = false;
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

        }
    }
}