using System.ComponentModel.DataAnnotations;

namespace First_Sample.Domain.Entities
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }

}
