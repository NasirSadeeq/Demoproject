using Microsoft.EntityFrameworkCore;

namespace Demoproject.Models
{
    public class ActivityRepository : IactivityRepository
    {
        private readonly AppDbContaxt appDbContaxt;

        public ActivityRepository(AppDbContaxt appDbContaxt)
        {
            this.appDbContaxt = appDbContaxt;
        }
        public async Task<Activity> CreateActivity(Activity activity)
        {
            var result = await appDbContaxt.Activities.AddAsync(activity);
            await appDbContaxt.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteActivity(int id)
        {
            var result = await appDbContaxt.Activities.FirstOrDefaultAsync(e => e.Id ==id );
            if (result != null)
            {
                appDbContaxt.Activities.Remove(result);
                await appDbContaxt.SaveChangesAsync();
            }
        }

        public async Task<Activity> GetActivity(int id)
        {
            return await appDbContaxt.Activities.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Activity> UpdateActivity(Activity activity)
        {
            var result = await appDbContaxt.Activities.FirstOrDefaultAsync(e => e.Id == activity.Id);
            if (result != null)
            {
                activity.Priorty = result.Priorty;
                activity.Description=result.Description;
                activity.Time=result.Time;
                activity.AssignBy=result.AssignBy;
                activity.AssignTo=result.AssignTo;
                await appDbContaxt.SaveChangesAsync();
                return result;

            }
            return null;
        }
    }
}
