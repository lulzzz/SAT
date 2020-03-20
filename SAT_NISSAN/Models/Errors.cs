using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SAT_NISSAN.Models
{
    public class Errors
    {
        public int code { get; set; }
        public Source source { get; set; }
        public string title { get; set; }
        public string detail { get; set; }
    }

    public class Source
    {
        public string pointer { get; set; }
    }
}