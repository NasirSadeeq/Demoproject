using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Demoproject.Models
{
   

    public class AdminRepository : IadminRepository
    {
        private readonly AppDbContaxt appDbContaxt;

        public AdminRepository(AppDbContaxt appDbContaxt)
        {
            this.appDbContaxt = appDbContaxt;
        }
        [AllowAnonymous]
        public  async Task<Admin> AddAdmin(Admin admin)
        {
            var adminResult=new Admin()
            {
                Name = admin.Name,
                Password = encrtptPassword( admin.Password), 
                MobileNo = admin.MobileNo, 
                Address = admin.Address,
                Email = admin.Email.ToLower(),
                roles="admin"
            };
            var result = await appDbContaxt.Admin.AddAsync(adminResult);
            await appDbContaxt.SaveChangesAsync();
            var id = appDbContaxt.Admin.Where(x => x.Email == adminResult.Email).FirstOrDefault();

            var addlogindetails = new Login()
            {
                Userid = id.Id,
                Email = adminResult.Email,
                Password = encrtptPassword(admin.Password),
                roles = adminResult.roles
            };
            var loginresult = await appDbContaxt.login.AddAsync(addlogindetails);
            await appDbContaxt.SaveChangesAsync();
           
            return result.Entity;

        }

        public async Task<UserDetails> AddUser(UserDetails userDetails)
        {
            var userDetailsResult = new UserDetails()
            {
                FisrtName = userDetails.FisrtName,
                LastName = userDetails.LastName,
                Email = userDetails.Email.ToLower(), 
                Address = userDetails.Address, 
                MobileNo = userDetails.MobileNo,
                Password = encrtptPassword( userDetails.Password),
                roles="user"

            };
            var result = await appDbContaxt.User.AddAsync(userDetailsResult);
            await appDbContaxt.SaveChangesAsync();
            var id = appDbContaxt.User.Where(x => x.Email == userDetailsResult.Email).FirstOrDefault();
            var loginresult = new Login()
            {
                Userid = id.Id,
                Email = userDetailsResult.Email,
                Password = userDetailsResult.Password,
                roles = userDetailsResult.roles
            };
            var svaelogin=await appDbContaxt.login.AddAsync(loginresult);
            await appDbContaxt.SaveChangesAsync();
           
            return result.Entity;
        }

        public async Task<Activity> CreateActivity(Activity activity, string email)
        {
            var prct = new Activity()
            {
                AssignBy = email.ToLower(),
                AssignTo= activity.AssignTo,
                Description=activity.Description,
                Time=activity.Time,
                Priorty=activity.Priorty

            };
            var result = await appDbContaxt.Activities.AddAsync(prct);
            await appDbContaxt.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteActivity(int id)
        {
            var result = await appDbContaxt.Activities.FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                appDbContaxt.Activities.Remove(result);
                await appDbContaxt.SaveChangesAsync();
            }
        }

        public async Task DeleteUser(string Email)
        {
            var result = await appDbContaxt.User.FirstOrDefaultAsync(e => e.Email.ToLower() == Email.ToLower());
            if (result != null)
            {
                var result1 = await appDbContaxt.login.FirstOrDefaultAsync(e => e.Email.ToLower() == result.Email.ToLower());
                appDbContaxt.login.Remove(result1);
                await appDbContaxt.SaveChangesAsync();
                appDbContaxt.User.Remove(result);
                await  appDbContaxt.SaveChangesAsync();
            }
        }

        public async Task<Activity> GetActivity(int id)
        {
            return await appDbContaxt.Activities.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Admin> GetAdmin(string Email)
        {
            return await appDbContaxt.Admin.FirstOrDefaultAsync(e => e.Email.ToLower() == Email.ToLower());
        }

        public async Task<IEnumerable<Activity>> GetAllActivities()
        {
            return await appDbContaxt.Activities.ToListAsync();
        }

        public async Task<IEnumerable<UserDetails>> GetAllUsers()
        {
            return await appDbContaxt.User.ToListAsync();
        }

        public async Task<UserDetails> GetUser(string Email)
        {
            return await appDbContaxt.User.FirstOrDefaultAsync(e => e.Email.ToLower() == Email.ToLower());
            
        }

        public async Task<Activity> UpdateActivity(Activity activity,string email)
        {
            var result = await appDbContaxt.Activities.FirstOrDefaultAsync(e => e.Id == activity.Id);
            if (result != null)
            {
                activity.Id = result.Id;
                activity.Description=result.Description;
                activity.Time=result.Time;
                activity.Priorty=result.Priorty;
                activity.AssignBy = email.ToLower();
                activity.AssignTo = result.AssignTo;
                await appDbContaxt.SaveChangesAsync();
                return result;

            }
            return null;
        }

        public async  Task<UserDetails> UpdateUser(UserDetails userDetails)
        {
            var result=await appDbContaxt.User.FirstOrDefaultAsync(e=>e.Email==userDetails.Email.ToLower());
            if(result != null)
            {
                userDetails.Email = result.Email.ToLower();
                userDetails.FisrtName=result.FisrtName;
                userDetails.LastName=result.LastName;
                userDetails.Address=result.Address;
                userDetails.MobileNo=result.MobileNo;
               // userDetails.Password=result.Password;
                 await appDbContaxt.SaveChangesAsync();
                var result1 = await appDbContaxt.login.FirstOrDefaultAsync(e => e.Email == result.Email.ToLower());
                if( result1 != null)
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
            if(string.IsNullOrEmpty(password)){
                return null;

            }
            else
            {
                byte[] storepassword = ASCIIEncoding.ASCII.GetBytes(password);
                string encryptpassword = Convert.ToBase64String(storepassword);
                return encryptpassword;

            }
            
        }

        //public async Task LogOut()
        //{
        //   awaait 
        //}
    }
}
