using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.MeanChords.ModelViews.MVDisc
{
    public class MVGetDiscRequest
    {
        public int DiscId { get; set; }
    }

    public class MVPostDiscRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DiscImgUrl { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public int AuthorID { get; set; }
        public List<PostCategory> Categories { get; set; }
    }

    public class PostCategory
    {
        public int CategoryId { get; set; }
    }
}
