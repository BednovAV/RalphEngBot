using System.Collections.Generic;

namespace Entities.Common.Grammar
{
    public class UserThemeExtendedItem : UserThemeItem
    {
        public List<LinkItem> TheoryLinks { get; set; } = new();
    }
}
