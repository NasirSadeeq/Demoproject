using Microsoft.AspNetCore.Mvc;

namespace Demoproject.Models
{
    public interface IadminRepository
    {
        Task<Admin> AddAdmin(Admin admin);

        Task<UserDetails> AddUser(UserDetails userDetails);
        Task<UserDetails> UpdateUser(UserDetails userDetails);
        Task DeleteUser(string Email);
        Task <IEnumerable<UserDetails>> GetAllUsers();
        Task<UserDetails>GetUser(string Email);
        Task<Admin> GetAdmin(string Email);
        Task<IEnumerable<Activity>> GetAllActivities();
        Task<Activity> GetActivity(int id);
        Task<Activity> UpdateActivity(Activity activity,string email);
        Task DeleteActivity(int id);
        Task <Activity> CreateActivity(Activity activity,string email);
       // Task LogOut();

    }
}
