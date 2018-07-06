using System.Collections.Generic;

namespace Windows10.Common
{
    public class NavigationModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public List<NavigationModel> Items { get; set; }
    }
}
