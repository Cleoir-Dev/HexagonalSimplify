using Domain.Entities.Whisper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Whisper
{
    public class OutputWhisper
    {
        public object translation { get; set; }
        public string transcription { get; set; }
        public string detected_language { get; set; }
    }
 }
