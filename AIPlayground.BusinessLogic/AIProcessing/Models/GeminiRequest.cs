using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIPlayground.BusinessLogic.AIProcessing.Models
{
    public class GeminiRequest
    {
        public SystemInstruction System_instruction { get; set; } = new SystemInstruction();
        public List<GeminiContent> Contents { get; set; } = [];
        public GeminiGenerationConfig GenerationConfig { get; set; } = new GeminiGenerationConfig();
    }
}
