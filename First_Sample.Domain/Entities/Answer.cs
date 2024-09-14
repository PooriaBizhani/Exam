using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Domain.Entities
{
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        [ForeignKey("QuestionId")]
        public int QuestionId { get; set; }
        public string Choice { get; set; }
        public Question Questions { get; set; }
        public User Users { get; set; }
    }
}
