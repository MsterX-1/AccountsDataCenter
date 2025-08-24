using Application.DTOs.AccountDto;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountService _accountService;
        public AccountsController(AccountService accountService)
        {
            _accountService = accountService;
        }
        #region Get Methods
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accountDto = await _accountService.GetAllAccountsAsync();
            if (accountDto == null)
            {
                return NotFound("No account found.");
            }
            return Ok(accountDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var accountDto = await _accountService.GetAccountByIdAsync(id);
            if (accountDto == null)
            {
                return NotFound($"No account found with ID: {id}");
            }
            return Ok(accountDto);
        }
        [HttpGet("ByUserId/{userId}")]
        public async Task<IActionResult> GetAccountsByUserId(int userId)
        {
            var accountDto = await _accountService.GetAccountsByUserIdAsync(userId);
            if (accountDto == null)
            {
                return NotFound($"No accounts found for User ID: {userId}");
            }
            return Ok(accountDto);
        }
        [HttpGet("BySellerName/{sellerName}")]
        public async Task<IActionResult> GetAccountsBySellerName(string sellerName)
        {
            var accountDto = await _accountService.GetAccountsBySellerNameAsync(sellerName);
            if (accountDto == null)
            {
                return NotFound($"No accounts found for Seller Name: {sellerName}");
            }
            return Ok(accountDto);
        }
        #endregion
        #region Post Methods
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _accountService.AddAccountAsync(dto);
                var createdAccount = await _accountService.GetAccountsBySellerNameAsync(dto.SellerName);
                return Ok(createdAccount);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region Put Methods
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] UpdateAccountDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               await _accountService.UpdateAccountAsync(id, dto);
                return Ok("Updated Successfuly");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region Delete Methods
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                await _accountService.DeleteAccountAsync(id);
                return Ok("Deleted Successfuly");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        #endregion
    }
}
