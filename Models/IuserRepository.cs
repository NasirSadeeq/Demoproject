namespace Demoproject.Models
{
    public interface IuserRepository
    {
        Task<UserDetails> AddUser(UserDetails userDetails);
        Task<UserDetails> GetUserDetails(string Email);
        Task<UserDetails> UpdateUserDetails(UserDetails userDetails);
        Task<Activity> GetActivity(int id);
        Task<Activity> UpdateActivity(Activity activity,string email);
        Task DeleteActivity(int id);
        Task<Activity> CreateActivity(Activity activity,string email);

    }
}
