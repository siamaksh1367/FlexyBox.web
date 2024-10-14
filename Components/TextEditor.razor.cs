using Blazored.TextEditor;
using Microsoft.AspNetCore.Components;

namespace FlexyBox.web.Components
{
    public partial class TextEditor
    {
        private bool isEditorInitialized = false;

        [Parameter]
        public BlazoredTextEditor Reference { get; set; }

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public EventCallback<string> GetContent { get; set; }

    }
}
