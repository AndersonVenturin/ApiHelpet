using ApiAuthentication.Data;
using ApiAuthentication.DTO;
using ApiAuthentication.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _contexto;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(DataContext contexto, UserManager<ApplicationUser> userManager)
        {
            _contexto = contexto;
            _userManager = userManager;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<ApplicationUser>> GetCurrentUser()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userIdClaim);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            var usuario = await _contexto.ApplicationUsers.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userIdClaim);

            if (user == null || user.Id != usuario.Id)
            {
                return Forbid();
            }

            return Ok(usuario);
        }

        [HttpGet("{id}/endereco"), Authorize]
        public async Task<ActionResult<Address>> GetEnderecoUsuario(string id)
        {
            var usuario = await _contexto.ApplicationUsers.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userIdClaim);

            if (user == null || user.Id != usuario.Id)
            {
                return Forbid();
            }

            await _contexto.Entry(usuario)
                .Reference(u => u.Address)
                .LoadAsync();

            return Ok(usuario.Address);
        }

        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult<ApplicationUser>> AddUser(
            [FromBody] UserViewModel model, 
            [FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _contexto.ApplicationUsers.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            try
            {
                usuario.UserName = model.UserName;
                usuario.CPF = model.CPF;
                usuario.PhoneNumber = model.PhoneNumber;
                var enderecoExistente = await _contexto.Addresses.FindAsync(usuario.AddressId);

                if (enderecoExistente != null)
                {
                    enderecoExistente.CEP = model.Address.CEP;
                    enderecoExistente.City = model.Address.City;
                    enderecoExistente.Street = model.Address.Street;
                    enderecoExistente.District = model.Address.District;
                    enderecoExistente.UF = model.Address.UF;
                    enderecoExistente.Complement = model.Address?.Complement;
                    enderecoExistente.Number = model.Address?.Number;

                    _contexto.Addresses.Update(enderecoExistente);
                }
                else
                {
                    var novoEndereco = new Address
                    {
                        CEP = model.Address.CEP,
                        City = model.Address.City,
                        Street = model.Address.Street,
                        District = model.Address.District,
                        UF = model.Address.UF,
                        Complement = model.Address?.Complement,
                        Number = model.Address?.Number
                    };

                    _contexto.Addresses.Add(novoEndereco);
                    usuario.Address = novoEndereco;
                }

                _contexto.ApplicationUsers.Update(usuario);
                await _contexto.SaveChangesAsync();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }
    }
}