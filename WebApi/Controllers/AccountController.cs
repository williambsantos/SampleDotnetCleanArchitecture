using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleDotnetCleanArchitecture.ApplicationBusiness.Dtos.Accounts;
using SampleDotnetCleanArchitecture.ApplicationBusiness.Interfaces;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountApplication _application;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAccountApplication application,
            ILogger<AccountController> logger
            )
        {
            _application = application;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AccountLoginResponseDto>> LoginAsync([FromBody] AccountLoginRequestDto request)
        {
            try
            {
                if (null == request)
                    return BadRequest("Invalid client request");

                _logger.LogError("Login: {userName}", request.Username);

                AccountLoginResponseDto? response = await _application.LoginAsync(request);
                if (response == null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Login. Message: {message}. Full: {data}", ex.Message, ex.ToString());
                throw;
            }
        }

        [HttpGet]
#if DEBUG
        [AllowAnonymous]
#else

        [Authorize(Policy = nameof(AccountClaims.CLAIMS_USER_CAN_LIST))]
#endif
        public async Task<ActionResult<ICollection<AccountResponseDto>>> ListAsync()
        {
            try
            {
                _logger.LogError("List");

                ICollection<AccountResponseDto> response = await _application.ListAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("List. Message: {message}. Full: {data}", ex.Message, ex.ToString());
                throw;
            }
        }

        [HttpGet]
        [Route("{userName}", Name = "GetByUserNameAsync")]
#if DEBUG
        [AllowAnonymous]
#else

        [Authorize(Policy = nameof(AccountClaims.CLAIMS_USER_CAN_LIST))]
#endif
        public async Task<ActionResult<AccountResponseDto>> GetByUserNameAsync([FromRoute] string userName)
        {
            try
            {
                _logger.LogError("GetByUserName. {userName}", userName);

                AccountResponseDto? response = await _application.GetByUserNameAsync(userName);
                if (null == response)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetByUserName. {userName}. Message: {message}. Full: {data}", userName, ex.Message, ex.ToString());
                throw;
            }
        }

        [HttpPost]
#if DEBUG
        [AllowAnonymous]
#else

        [Authorize(Policy = nameof(AccountClaims.CLAIMS_USER_CAN_MODIFY))]
#endif
        public async Task<ActionResult<AccountCreateResponseDto>> CreateAsync([FromBody] AccountCreateRequestDto request)
        {
            try
            {
                if (null == request)
                    return BadRequest("Invalid client request");

                _logger.LogError("Register: {userName}", request.Username);

                AccountCreateResponseDto response = await _application.CreateAsync(request);
                return CreatedAtRoute(nameof(GetByUserNameAsync), new { userName = response.UserName }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Register. Message: {message}. Full: {data}", ex.Message, ex.ToString());
                throw;
            }
        }

        [HttpDelete]
        [Route("{userName}")]
#if DEBUG
        [AllowAnonymous]
#else

        [Authorize(Policy = nameof(AccountClaims.CLAIMS_USER_CAN_MODIFY))]
#endif
        public async Task<IActionResult> DeleteAsync([FromRoute] string userName)
        {
            try
            {
                _logger.LogError("Delete. {userName}", userName);

                await _application.DeleteAsync(userName);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete. {userName}. Message: {message}. Full: {data}", userName, ex.Message, ex.ToString());
                throw;
            }
        }

        [HttpPost]
        [Route("{userName}/claims/{claimName}")]
#if DEBUG
        [AllowAnonymous]
#else

        [Authorize(Policy = nameof(AccountClaims.CLAIMS_USER_CAN_MODIFY))]
#endif
        public async Task<ActionResult<string>> AddClaimToUserAsync([FromRoute] string userName, AccountClaims claimName)
        {
            try
            {
                _logger.LogError("AddClaimToUser. {userName}, {claimName}", userName, claimName);

                await _application.AddClaimToUserAsync(userName, claimName);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("AddClaimToUser. {userName}, {claimName}. Message: {message}. Full: {data}", userName, claimName, ex.Message, ex.ToString());
                throw;
            }
        }

        [HttpDelete]
        [Route("{userName}/claims/{claimName}")]
#if DEBUG
        [AllowAnonymous]
#else

        [Authorize(Policy = nameof(AccountClaims.CLAIMS_USER_CAN_MODIFY))]
#endif
        public async Task<ActionResult<string>> DeleteClaimFromUserAsync([FromRoute] string userName, AccountClaims claimName)
        {
            try
            {
                _logger.LogError("DeleteClaimFromUser. {userName}, {claimName}", userName, claimName);

                await _application.DeleteClaimFromUserAsync(userName, claimName);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteClaimFromUser. {userName}, {claimName}. Message: {message}. Full: {data}", userName, claimName, ex.Message, ex.ToString());
                throw;
            }
        }
    }

}
