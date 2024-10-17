using FlexyBox.core.Queries.GetComments;
using Microsoft.AspNetCore.Components;

namespace YourNamespace.Components
{
    public partial class Comments
    {
        [Parameter]
        public List<GetCommentResponse> AllComments { get; set; } = new();
    }
}