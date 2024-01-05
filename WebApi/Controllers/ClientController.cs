using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleDotnetCleanArchitecture.ApplicationBusiness.Dtos.Clients;
using SampleDotnetCleanArchitecture.ApplicationBusiness.Interfaces;
using SampleDotnetCleanArchitecture.EnterpriseBusiness.Entities;

namespace SampleDotnetCleanArchitecture.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientApplication _clientApplication;
        private readonly ILogger<ClientController> _logger;

        public ClientController(
            IClientApplication clientApplication,
            ILogger<ClientController> logger
            )
        {
            _clientApplication = clientApplication;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = nameof(AccountClaims.CLAIMS_CLIENT_CAN_LIST))]
        public async Task<ActionResult<ICollection<ClientResponseDto>>> ListAsync()
        {
            try
            {
                _logger.LogError("List");

                ICollection<ClientResponseDto> response = await _clientApplication.ListAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("List. Message: {message}. Full: {data}", ex.Message, ex.ToString());
                throw;
            }
        }

        [HttpGet]
        [Route("{id}", Name = nameof(GetByIdAsync))]
        [Authorize(Policy = nameof(AccountClaims.CLAIMS_CLIENT_CAN_LIST))]
        public async Task<ActionResult<ClientResponseDto>> GetByIdAsync(long id)
        {
            try
            {
                _logger.LogError("GetById. {id}", id);

                ClientResponseDto? response = await _clientApplication.GetByIdAsync(id);
                if (null == response)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetById. {id}. Message: {message}. Full: {data}", id, ex.Message, ex.ToString());
                throw;
            }
        }

        [HttpPost]
        [Authorize(Policy = nameof(AccountClaims.CLAIMS_CLIENT_CAN_INSERT))]
        public async Task<ActionResult<ClientCreateResponseDto>> CreateAsync([FromBody] ClientCreateRequestDto request)
        {
            try
            {
                if (null == request)
                    return BadRequest("Invalid client request");

                _logger.LogError("Create: {firstName} {lastName}", request.FirstName, request.LastName);

                var currentAccount = this.User?.Identity?.Name;

                ClientCreateResponseDto response = await _clientApplication.CreateAsync(request, currentAccount);

                return CreatedAtRoute(nameof(GetByIdAsync), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Create: {firstName} {lastName}. Message: {message}, Full: {data}", request.FirstName, request.LastName, ex.Message, ex.ToString());
                throw;
            }
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Policy = nameof(AccountClaims.CLAIMS_CLIENT_CAN_UPDATE))]
        public async Task<ActionResult<ClientCreateResponseDto>> UpdateAsync([FromRoute] long id, [FromBody] ClientUpdateRequestDto request)
        {
            try
            {
                if (null == request)
                    return BadRequest("Invalid client request");

                _logger.LogError("Update: {id} {firstName} {lastName}", id, request.FirstName, request.LastName);

                var currentAccount = this.User?.Identity?.Name;

                await _clientApplication.UpdateAsync(id, request, currentAccount);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Update: {id} {firstName} {lastName}. Message: {message}, Full: {data}", id, request.FirstName, request.LastName, ex.Message, ex.ToString());
                throw;
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = nameof(AccountClaims.CLAIMS_CLIENT_CAN_DELETE))]
        public async Task<IActionResult> DeleteAsync([FromRoute] long id)
        {
            try
            {
                _logger.LogError("Delete. {id}", id);

                await _clientApplication.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete. {id}. Message: {message}. Full: {data}", id, ex.Message, ex.ToString());
                throw;
            }
        }
    }
}
