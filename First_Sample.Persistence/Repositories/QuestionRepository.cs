using First_Sample.Domain.Entities;
using First_Sample.Domain.InterFaces;
using First_Sample.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace First_Sample.Persistence.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly First_Sample_Context _context;
        public QuestionRepository(First_Sample_Context context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<Question>> GetAllQuestions()
        {
            var Questions = await _context.Questions.ToListAsync();
            return Questions;
        }

        public async Task<Question> GetQuestionById(int id)
        {
            var Questions = await _context.Questions.FirstOrDefaultAsync(o => o.QuestionId == id);
            return Questions;
        }

        public async Task<int> GetTotalQuestionsCount()
        {
            return await _context.Questions.CountAsync();
        }
    }
}
