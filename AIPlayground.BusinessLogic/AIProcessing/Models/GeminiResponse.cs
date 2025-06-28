using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIPlayground.BusinessLogic.AIProcessing.Models
{
    public class GeminiResponse
    {
        public List<GeminiCandidate> Candidates { get; set; } = [];
    }
}
