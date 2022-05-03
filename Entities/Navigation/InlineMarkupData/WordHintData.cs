using Entities.Common;

namespace Entities.Navigation.InlineMarkupData
{
    public class WordHintData : IInlineMarkupData
    {
        public int WordId { get; set; }
        public Language WordLanguage { get; set; }
    }
}
