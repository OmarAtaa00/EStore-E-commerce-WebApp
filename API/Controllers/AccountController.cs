using System;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<User> userManager,
         SignInManager<User> signInManager,

        ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            // var email = User.FindFirstValue(ClaimTypes.Email);//Clean version to get email
            //old and harder version 
            //var email = HttpContext.User?.Claims?.firstOrDefault(x => x.Type == claimTypes.Email)?.Value;

            var user = await _userManager.FindByEmailFormClaims(User);
            return new UserDto
            {
                Email = user.Email,
                Name = user.DisplayName,
                Token = _tokenService.CreateToken(user),

            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //In order to login we need combination form userManger to get the user from database
            //and SignInManger to check the user password to our one in the database
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            //If user not found
            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false); //->lockout
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            //otherwise 
            return new UserDto
            {
                Email = user.Email,
                Name = user.DisplayName,
                Token = _tokenService.CreateToken(user),

            };

        }

        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result) //.Value does not exist
            {

                return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Email address is in use " } });
            }
            var user = new User
            {
                Email = registerDto.Email,
                DisplayName = registerDto.Name,
                UserName = registerDto.Email, //the email is unique we can use it as username



            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                Name = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user),

            };
        }

        //check if email is already in use 
        //this is a helper method to the client so they can do email Asynchronous  validation on the client side
        //this is mostly for the client side to utilize this 
        [HttpGet("emailexists")]
        public async Task<bool> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
        // get the user address
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            // var email = User.FindFirstValue(ClaimTypes.Email);//Clean version to get email
            //old and harder version 
            //var email = HttpContext.User?.Claims?.firstOrDefault(x => x.Type == claimTypes.Email)?.Value;

            var user = await _userManager.FindUserByClaimWithAddressAsync(User);

            return _mapper.Map<Address, AddressDto>(user.Address);
            // ISSUE returns 201 no content in the address method 
            //Sol. in Authentication configuration --> ValidAudience  = false;

            //the problem now that it's trying to return address that have User type in it that have address
            //in it as well so it trying to return that too 

        }
        [Authorize]
        [HttpPut]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto address)
        {
            // get user address 
            var user = await _userManager.FindUserByClaimWithAddressAsync(User);
            //Utilize auto mapper  to update the properties in the UserAddress
            user.Address = _mapper.Map<AddressDto, Address>(address);

            // update the user using userManager
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));

            return BadRequest("Unexpected error while updating the address");
        }

    }
}

// One of your jobs as a software developer is not problem solving but information gathering 
