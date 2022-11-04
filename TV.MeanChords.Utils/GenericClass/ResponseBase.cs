using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.MeanChords.Utils.GenericClass
{
    public class ResponseBase<T>
    {
        public ResponseBase()
        {
            this.Errors = new List<string>();
        }

        public static ResponseBase<T> Create(T Data) => new ResponseBase<T>
        {
            Status = true,
            Data = Data
        };

        public static ResponseBase<T> Create(List<string> Errors) => new ResponseBase<T>
        {
            Status = false,
            Errors = Errors
        };

        public static ResponseBase<T> Create(ResponseBase<T> response) => new ResponseBase<T>
        {
            Data = response.Data
        };

        public bool Status { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
    }
}
