using First_Sample.Domain.Entities;
using First_Sample.Domain.InterFaces;
using First_Sample.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace First_Sample.Persistence.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly First_Sample_Context _context;
        public AnswerRepository(First_Sample_Context context)
        {
            _context = context;
        }

        public async Task<bool> Add(Answer entity)
        {
            try
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;// اگر رکوردی اضافه شد، true برگردانید.
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message;
                Console.WriteLine($"Inner Exception: {innerException}");
                return false;
            }
        }

        public async Task<IReadOnlyList<Answer>> GetAllAnswer()
        {
            var Answers = await _context.Answers.ToListAsync();
            return Answers;
        }

        public async Task<Answer> GetAnswerByQuestionId(int questionId)
        {
            var Answer = await _context.Answers.FirstOrDefaultAsync(o => o.QuestionId == questionId);
            return Answer;
        }

        public async Task<int> GetAnsweredQuestionCount(int userId)
        {
            return await _context.Answers
                     .Where(a => a.QuestionId == userId)
                     .CountAsync();
        }

        public async Task<string> GetExamResults(List<string> answers)
        {
            string Daron = "Daron gara hastid";
            string Boron = "Boron gara hastid";

            // Count the number of "yes" and "no" answers
            int yesCount = answers.Count(answer => answer.Equals("yes", StringComparison.OrdinalIgnoreCase));
            int noCount = answers.Count(answer => answer.Equals("no", StringComparison.OrdinalIgnoreCase));

            // Determine the result based on the counts
            if (yesCount > noCount)
            {
                return Daron;  // Introvert
            }
            else
            {
                return Boron;  // Extrovert
            }
        }

        public async Task<List<string>> GetUserAnswers(int userid)
        {
            var userAnswers = await _context.Answers
     .Where(answer => answer.UserId == userid)  // Filter by user ID
     .Select(answer => answer.Choice)  // Select the answer choice
     .ToListAsync();
            return userAnswers;
        }

        public async Task<bool> Update(Answer entity)
        {
            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return true;// اگر رکوردی اضافه شد، true برگردانید.
            }
            catch (Exception)
            {
                // در صورت بروز خطا، false برگردانید.
                return false;
            }
        }
    }
}
