using First_Sample.Domain.Entities;

namespace First_Sample.Domain.InterFaces
{
    public interface IQuestionRepository
    {
        Task<IReadOnlyList<Question>> GetAllQuestions();
        Task<Question> GetQuestionById(int id);
        Task<int> GetTotalQuestionsCount();
    }
}
