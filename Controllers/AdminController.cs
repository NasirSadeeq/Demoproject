using Demoproject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demoproject.Controllers
{
    [Authorize(Roles ="admin")]
    [Route("api/[controller]")]
    [ApiController]

    public class AdminController : ControllerBase
    {
        private readonly IadminRepository iadminRepository;
       // private readonly IadminTokenManager iadminTokenManager;

        public AdminController(IadminRepository iadminRepository)
        {
            this.iadminRepository = iadminRepository;
            //this.iadminTokenManager = iadminTokenManager;
        }
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                return Ok(await iadminRepository.GetAllUsers());
            }
            catch (Exception ex)
            {

               return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }

        [HttpGet("GetUser/{Email}")]
        public async Task<ActionResult<UserDetails>> GetUser(string Email)
        {
            try
            {
                var result=await iadminRepository.GetUser(Email.ToLower());
                if(result== null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<UserDetails>> AddUser(UserDetails userDetails)
        {
            try
            {
                if (userDetails == null)
                    return BadRequest();

                var emp = await iadminRepository.GetUser(userDetails.Email.ToLower());
                if(emp != null)
                {
                    ModelState.AddModelError(emp.Email, "User with same Email is already exist");
                    return BadRequest(ModelState);
                }

                var result = await iadminRepository.AddUser(userDetails);
               // return CreatedAtAction(nameof(GetUser),new {email=result.Email},result);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [AllowAnonymous]
        [HttpPost("AddAdmin")]
        public async Task<ActionResult<Admin>> AddAdmin(Admin admin)
        {
            try
            {
                if (admin == null)
                    return BadRequest();

                var emp = await iadminRepository.GetAdmin(admin.Email.ToLower());
                if (emp != null)
                {
                    ModelState.AddModelError(emp.Email, "Admin with same Email is already exist");
                    return BadRequest(ModelState);
                }

                var result = await iadminRepository.AddAdmin(admin);
                // return CreatedAtAction(nameof(GetUser),new {email=result.Email},result);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
       
        [HttpPut("UpdateUser/{Email}")]
        public async Task<ActionResult<UserDetails>> UpdateUser(string Email, UserDetails userDetails)
        {
            try
            {
                if (Email !=userDetails.Email.ToLower())
                    return BadRequest("Employee Email Mismatch");

                var emp = await iadminRepository.GetUser(Email.ToLower());
                if (emp == null)
                {
                    return BadRequest($"Employee with Email {Email} not found");
                }

                return await iadminRepository.UpdateUser(userDetails);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [HttpDelete("DeleteUser/{Email}")]
        public async Task<ActionResult> DeleteUser(string Email)
        {
            try
            {
                var emp = await iadminRepository.GetUser(Email.ToLower());
                if (emp == null)
                {
                    return BadRequest($"User with Email {Email} not found");
                }

                 await iadminRepository.DeleteUser(Email.ToLower());
                return Ok($"User with Email {Email} Deleted");
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("GetAllActivities")]
        public async Task<ActionResult> GetAllActivities()
        {
            try
            {
                return Ok(await iadminRepository.GetAllActivities());
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost("CreateActivity")]
        public async Task<ActionResult<Activity>> CreateActivity(Activity activity,string email)
        {
            try
            {
                if (activity == null)
                    return BadRequest();


                var result = await iadminRepository.CreateActivity(activity,email);
                // return CreatedAtAction(nameof(GetUser),new {email=result.Email},result);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("GetActivity/{id}")]
        public async Task<ActionResult<Activity>> GetActivity(int id)
        {
            try
            {
                var result = await iadminRepository.GetActivity(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut("UpdateActivity{id}")]
        public async Task<ActionResult<Activity>> UpdateActivity(int id, Activity activity,string email)
        {
            try
            {
                if (id != activity.Id)
                    return BadRequest("Activity Id Mismatch");

                var emp = await iadminRepository.GetActivity(id);
                if (emp == null)
                {
                    return BadRequest($"Activity with this  {id} not found");
                }
                var admin = new Admin();
                return await iadminRepository.UpdateActivity(activity,email);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete(" DeleteActivity/{id}")]
        public async Task<ActionResult> DeleteActivity(int id)
        {
            try
            {
                var emp = await iadminRepository.GetActivity(id);
                if (emp == null)
                {
                    return BadRequest($"Activity with this id=  {id} not found");
                }

                await iadminRepository.DeleteActivity(id);
                return Ok($"Activity with this id= {id} Deleted");
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        //[AllowAnonymous]
        //[HttpPost("AdminLogin")]
        //public IActionResult AdminLogin([FromBody] UserLogin userLogin)
        //{
        //    var Token = iadminTokenManager.Authenticate(userLogin.Email.ToLower(), userLogin.Password);
        //    if (string.IsNullOrEmpty(Token))
        //        return Unauthorized();

        //    return Ok(Token);
        //}
        [HttpPost]
        [Route("LogOut")]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var StoreCookies = Request.Cookies.Keys;
            foreach (var cookie in StoreCookies)
            {
                Response.Cookies.Delete(cookie);
            }
            return Ok("Logout Successfully");
        }
    }
}
