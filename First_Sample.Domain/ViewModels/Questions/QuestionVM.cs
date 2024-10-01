using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Domain.ViewModels.Questions
{
    public class QuestionVM
    {
        public int Id { get; set; }
        public string Questions { get; set; }
        public string PreviousChoice { get; set; }
    }
}
