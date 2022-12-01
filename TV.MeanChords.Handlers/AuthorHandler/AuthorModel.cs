using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.MeanChords.Handlers.AuthorHandler
{
    public class AuthorResponse
    {
        public int AuthorId { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
    }
}
