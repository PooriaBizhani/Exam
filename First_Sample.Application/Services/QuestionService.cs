using First_Sample.Domain.Entities;
using First_Sample.Domain.InterFaces;
using First_Sample.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Application.Services
{
    public class QuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }
        public async Task<IReadOnlyList<Question>> GetAllService()
        {
            var Questions = await _questionRepository.GetAllQuestions();
            return Questions;
        }
        public async Task<Question> GetByIdService(int id)
        {
            var Questions = await _questionRepository.GetQuestionById(id);
            return Questions;
        }
        public async Task<int> GetTotelQuestionService()
        {
            return await _questionRepository.GetTotalQuestionsCount();
        }
    }
}
