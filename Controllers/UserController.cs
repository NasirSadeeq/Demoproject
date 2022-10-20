using Demoproject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demoproject.Controllers
{
    [Authorize(Roles ="user")]
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IuserRepository iuserRepository;
        private readonly IwjtTokenManager iwjtTokenManager;

        public UserController(IuserRepository iuserRepository, IwjtTokenManager iwjtTokenManager)
        {
            this.iuserRepository = iuserRepository;
            this.iwjtTokenManager = iwjtTokenManager;
        }

        [HttpPost("SignUp")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDetails>> AddUser(UserDetails userDetails)
        {
            try
            {
                if (userDetails == null)
                    return BadRequest();

                var emp = await iuserRepository.GetUserDetails(userDetails.Email.ToLower());
                if (emp != null)
                {
                    ModelState.AddModelError(emp.Email, "User with same Email is already exist");
                    return BadRequest(ModelState);
                }

                var result = await iuserRepository.AddUser(userDetails);
                // return CreatedAtAction(nameof(GetUser),new {email=result.Email},result);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        //[AllowAnonymous]
        //[HttpPost("UserLogin")]
        //public IActionResult UserLogin([FromBody] UserLogin userLogin)
        //{
        //    var Token = iwjtTokenManager.Authenticate(userLogin.Email.ToLower(), userLogin.Password);
        //    if (string.IsNullOrEmpty(Token))
        //        return Unauthorized();

        //    return Ok(Token);
        //}

        [HttpGet("GetUserDetails/{Email}")]
        public async Task<ActionResult<UserDetails>> GetUserDetails(string Email)
        {
            try
            {
                var result = await iuserRepository.GetUserDetails(Email.ToLower());
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

        [HttpPut("UpdateUserDetails/{Email}")]
        public async Task<ActionResult<UserDetails>> UpdateUserDetails(string Email, UserDetails userDetails)
        {
            try
            {
                if (Email != userDetails.Email.ToLower())
                    return BadRequest("Employee Email Mismatch");

                var emp = await iuserRepository.GetUserDetails(Email.ToLower());
                if (emp == null)
                {
                    return BadRequest($"Employee with Email {Email} not found");
                }

                return await iuserRepository.UpdateUserDetails(userDetails);
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


                var result = await iuserRepository.CreateActivity(activity,email);
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
                var result = await iuserRepository.GetActivity(id);
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

        [HttpPut("UpdateActivity/{id}")]
        public async Task<ActionResult<Activity>> UpdateActivity(int id, Activity activity,string email)
        {
            try
            {
                if (id != activity.Id)
                    return BadRequest("Activity Id Mismatch");

                var emp = await iuserRepository.GetActivity(id);
                if (emp == null)
                {
                    return BadRequest($"Activity with this  {id} not found");
                }

                return await iuserRepository.UpdateActivity(activity,email);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete("DeleteActivity/{id}")]
        public async Task<ActionResult> DeleteActivity(int id)
        {
            try
            {
                var emp = await iuserRepository.GetActivity(id);
                if (emp == null)
                {
                    return BadRequest($"Activity with this id=  {id} not found");
                }

                await iuserRepository.DeleteActivity(id);
                return Ok($"Activity with this id= {id} Deleted");
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

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
