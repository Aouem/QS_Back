using System.Collections.Generic;
using QS.Models;

namespace QS.Models
{
    public class FormResultDto
    {
        public int Service { get; set; }
        public RepondantDto Repondant { get; set; } = new RepondantDto();
        public List<ReponseDto> Reponses { get; set; } = new List<ReponseDto>();
    }
}
