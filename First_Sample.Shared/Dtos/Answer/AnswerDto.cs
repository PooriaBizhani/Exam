using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Shared.Dtos.Answer
{
    public class AnswerDto
    {
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public string Choice { get; set; }
    }
}
