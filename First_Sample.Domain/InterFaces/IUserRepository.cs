using First_Sample.Domain.Entities;

namespace First_Sample.Domain.InterFaces
{
    public interface IUserRepository
    {
        Task<IReadOnlyList<User>> GetAllUser();
        Task<bool> Add(User entity);
        Task<User> GetUserById(int id);
        Task<bool> Update(User entity);
        Task<bool> Remove(User entity);
    }
}
