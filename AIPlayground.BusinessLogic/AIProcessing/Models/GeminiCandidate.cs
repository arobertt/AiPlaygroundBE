using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIPlayground.BusinessLogic.AIProcessing.Models
{
    public class GeminiCandidate
    {
        public GeminiContent Content { get; set; } = new GeminiContent();
        public string FinishReason { get; set; } = string.Empty;
    }
}
