using First_Sample.Domain.Entities;
using First_Sample.Domain.InterFaces;
using First_Sample.Persistence.Repositories;
using First_Sample.Shared.Dtos.Answer;
using First_Sample.Shared.ViewModels.Answers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Application.Services
{
    public class AnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }
        public async Task<IReadOnlyList<Answer>> GetAllService()
        {
            var Answers = await _answerRepository.GetAllAnswer();
            return Answers;
        }
        public async Task AddService(AnswerDto Answer)
        {
            var user = new Answer
            {
                QuestionId = Answer.QuestionId,
                UserId = 1,
                Choice = Answer.Choice,
            };
            await _answerRepository.Add(user);
        }
        public async Task<Answer> GetAnswerByQuestionIdService(int questionId)
        {
            var user = await _answerRepository.GetAnswerByQuestionId(questionId);
            return user;
        }
        public async Task UpdateService(AnswerDto Answer)
        {
            var user = new Answer
            {
                QuestionId = Answer.QuestionId,
                UserId = 1,
                Choice = Answer.Choice,
            };
            await _answerRepository.Update(user);
        }
        public async Task<int> GetAnsweredQuestionCountService(int userId)
        {
            var user = await _answerRepository.GetAnsweredQuestionCount(userId);
            return user;
        }
        public async Task<List<string>> GetUserAnswersService()
        {
            var result = await _answerRepository.GetUserAnswers(1);
            return result;
        }
        public async Task<string> GetExamResultsService(List<string> answers)
        {
            var result = await _answerRepository.GetExamResults(answers);
            return result;
        }
    }
}