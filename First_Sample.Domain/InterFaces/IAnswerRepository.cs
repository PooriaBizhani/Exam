using First_Sample.Domain.Entities;

namespace First_Sample.Domain.InterFaces
{
    public interface IAnswerRepository
    {
        Task<IReadOnlyList<Answer>> GetAllAnswer();
        Task<bool> Add(Answer entity);
        Task<bool> Update(Answer entity);
        Task<Answer> GetAnswerByQuestionId(int id);
        Task<int> GetAnsweredQuestionCount(int userId);
        Task<List<string>> GetUserAnswers(int userid);
        Task<string> GetExamResults(List<string> answers);
    }
}
