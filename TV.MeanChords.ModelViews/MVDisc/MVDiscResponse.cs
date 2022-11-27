using System.Collections.Generic;
using TV.MeanChords.Handlers;

namespace TV.MeanChords.ModelViews.MVDisc
{
    public class MVGetDiscResponse
    {
        public int DiscId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DiscImgUrl { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public MVGetDiscResponseAuthor Author { get; set; }
        public List<Category> Categories { get; set; }
    }

    public class MVPostDiscResponse
    {
        public int DiscId { get; set; }
    }

    public class MVGetDiscResponseAuthor
    {
        public string FullName { get; set; }
        public string ShortName { get; set; }
    }
}
