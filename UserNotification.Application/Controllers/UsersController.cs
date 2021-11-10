using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserNotification.Application.Authentication.Interfaces;
using UserNotification.Domain.Commands;
using UserNotification.Domain.Entities;
using UserNotification.Domain.Interfaces.Repositories;
using UserNotification.Domain.Interfaces.Services;
using UserNotification.Shared.Commands;

namespace UserNotification.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiVersion("2.0")]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IUserServices _userServices;
        private readonly ITokenServices _tokenServices;

        public UsersController(IUsersRepository usersRepository, IUserServices userServices, ITokenServices tokenService)
        {
            _usersRepository = usersRepository;
            _userServices = userServices;
            _tokenServices = tokenService;
        }

        /// <summary>
        /// Login do Usuário
        /// </summary>
        /// <returns>Se login efetuado com sucesso.</returns>
        /// <response code="200">Login efetuado com sucesso. ObjectResult populado com o objeto de Users. Token com o Token gerado para o Usuário</response>
        /// <response code="400">Usuário ou Senha inválidos.</response>
        [HttpPost("DoLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> DoLogin([FromBody] LoginCommand loginCommand)
        {
            CommandResultObject<Users> commandResult = (CommandResultObject<Users>)await _userServices.DoLogin(loginCommand);
            
            if (commandResult.StatusCode == 200)
                return Ok(new { commandResult, token = _tokenServices.CreateToken(loginCommand) });
            else
                return Ok(new { commandResult });
        }

        /// <summary>
        /// Inclusão do Usuário
        /// </summary>
        /// <returns>Se usuário cadastrado com sucesso.</returns>
        /// <response code="200">Usuário cadastrado com sucesso.</response>
        [HttpPost("Insert")]
        [Authorize]
        public async Task<IActionResult> Insert([FromBody] CreateUsersCommand createUserCommand) => Ok(await _userServices.Insert(createUserCommand));

        /// <summary>
        /// Atualização dos dados do Usuário
        /// </summary>
        /// <returns>Se o Usuário foi cadastrado.</returns>
        /// <response code="200">Usuário alterado com sucesso.</response>
        /// <response code="400">Usuário não existe.</response>
        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateUsersCommand updateUserCommand) => Ok(await _userServices.Update(updateUserCommand));

        /// <summary>
        /// Exclusão do Usuário
        /// </summary>
        /// <returns>Se o Usuário excluído.</returns>
        /// <response code="200">Usuário excluído com sucesso.</response>
        /// <response code="400">Usuário não existe.</response>
        [HttpPost("Delete")]
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] IdCommand removeUserCommand) => Ok(await _userServices.Delete(removeUserCommand));

        /// <summary>
        /// Consulta de Usuários
        /// </summary>
        /// <returns>Lista de Usuários cadastrados.</returns>
        /// <response code="200">Lista de Users</response>
        [HttpGet("Select")]
        [Authorize]
        public async Task<IActionResult> Select() => Ok(await _userServices.Select());

        /// <summary>
        /// Consulta de Usuário específico
        /// </summary>
        /// <returns>Usuário cadastrado.</returns>
        /// <response code="200">Objeto de Users</response>
        [HttpGet("Select/{id}")]
        [Authorize]
        public async Task<IActionResult> Select(int id) => Ok(await _userServices.Select(id));

        /// <summary>
        /// Envio de Email para lembrar o Usuário
        /// </summary>
        /// <returns>Se o Email enviado com sucesso.</returns>
        /// <response code="200">Email enviado com sucesso.</response>
        /// <response code="400">Email não enviado.</response>
        [HttpPost("SendEmail")]
        [Authorize]
        public async Task<IActionResult> SendEmail([FromBody] ICollection<EmailUsersCommand> listEmailUsersCommand) => Ok(await _userServices.SendEmail(listEmailUsersCommand));
    }
}
