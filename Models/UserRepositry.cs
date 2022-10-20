using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.VisualBasic;
using System.Text;

namespace Demoproject.Models
{
    public class UserRepositry : IuserRepository
    {
        private readonly AppDbContaxt appDbContaxt;

        public UserRepositry( AppDbContaxt appDbContaxt)
        {
            this.appDbContaxt = appDbContaxt;
        }

        public async Task<UserDetails> AddUser(UserDetails userDetails)
        {
            var addUser = new UserDetails()
            {
                FisrtName = userDetails.FisrtName, 
                LastName= userDetails.LastName,
                Email = (userDetails.Email).ToLower(), 
                Address = userDetails.Address,
                MobileNo = userDetails.MobileNo,
                Password = encrtptPassword( userDetails.Password),
                roles= "user",
            };
            var result = await appDbContaxt.User.AddAsync(addUser);
            await appDbContaxt.SaveChangesAsync();
          var id=appDbContaxt.User.Where(x => x.Email == addUser.Email).FirstOrDefault();
            var addlogindetails = new Login()
            {
             
                Userid = id.Id,
                Email = addUser.Email,
                Password = encrtptPassword(userDetails.Password),
                roles = addUser.roles
            };
            var loginresult=await appDbContaxt.login.AddAsync(addlogindetails);
            await appDbContaxt.SaveChangesAsync();
           
            return result.Entity;
        }

        public async Task<Activity> CreateActivity(Activity activity, string email)
        {
            var createActivity = new Activity()
            {
                Description= activity.Description, 
                Time = DateTime.Now, 
                Priorty=activity.Priorty, 
                AssignBy=email.ToLower(), 
                AssignTo=activity.AssignTo,
            };
            var result = await appDbContaxt.Activities.AddAsync(createActivity);
            await appDbContaxt.SaveChangesAsync();
            return result.Entity;
        }

        public async  Task DeleteActivity(int id)
        {
            var result = await appDbContaxt.Activities.FirstOrDefaultAsync(e => e.Id == id);
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

        public async Task<UserDetails> GetUserDetails(string Email)
        {
            return await appDbContaxt.User.FirstOrDefaultAsync(e=>e.Email==Email.ToLower());
        }

        public async Task<Activity> UpdateActivity(Activity activity,string email)
        {
            var result = await appDbContaxt.Activities.FirstOrDefaultAsync(e => e.Id == activity.Id);
            if (result != null)
            {
                activity.Description = result.Description;
                activity.Time = result.Time;
                activity.Priorty = result.Priorty;
                activity.AssignBy = email.ToLower();
                activity.AssignTo=result.AssignTo;
                await appDbContaxt.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<UserDetails> UpdateUserDetails(UserDetails userDetails)
        {
            var result = await appDbContaxt.User.FirstOrDefaultAsync(e => e.Email == userDetails.Email.ToLower());
            if (result != null)
            {
                userDetails.Email = result.Email.ToLower();
                userDetails.FisrtName = result.FisrtName;
                userDetails.LastName = result.LastName;
                userDetails.Address = result.Address;
                userDetails.MobileNo = result.MobileNo;
                userDetails.Password = encrtptPassword( result.Password);
              //  userDetails.roles = "user";
                await appDbContaxt.SaveChangesAsync();
                var result1 = await appDbContaxt.login.FirstOrDefaultAsync(e => e.Email == result.Email.ToLower());
                if(result1 != null)
                {
                    result.Email = result1.Email.ToLower();
                    await appDbContaxt.SaveChangesAsync();
                    
                }
                return result;

            }
            return null;
        }
        public static string encrtptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] storepassword = ASCIIEncoding.ASCII.GetBytes(password);
                string encryptpassword = Convert.ToBase64String(storepassword);
                return encryptpassword;

            }
           
        }
    }
}
