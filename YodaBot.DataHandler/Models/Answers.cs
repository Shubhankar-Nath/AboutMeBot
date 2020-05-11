using System;
using System.Collections.Generic;
using System.Text;

namespace YodaBot.DataHandler.Models
{
    public class Answers
    {
        public string QnaId { get; set; }
        public string Type { get; set; }
        public IList<string> Content { get; set; }
        public IList<string> Media { get; set; }
        public string Property { get; set; }
    }
}
