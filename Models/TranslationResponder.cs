using Microsoft.Extensions.FileProviders.Composite;

namespace AFS_L33tSp34k3r_App.Models
{
    /// <summary>
    /// Model holding data for responses
    /// </summary>
    public class TranslationResponder
    {
        public Contents? Contents { get; set; }
    }
    public class Contents
    {
        public string? Translated { get; set; }
        public string? Text { get; set; }
        public string? Translation { get; set; }
    }
}
