namespace Demoproject.Models
{
    public interface IactivityRepository
    {
        Task<Activity> CreateActivity(Activity activity);
        Task<Activity> UpdateActivity(Activity activity);
        Task DeleteActivity(int id);
        Task <Activity> GetActivity(int id);
    }
}
