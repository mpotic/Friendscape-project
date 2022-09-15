using Back.DTO;
using Back.Interfaces;
using Back.Miscellaneous;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Controllers
{
	[Route("api/person")]
	[ApiController]
	public class PersonController : ControllerBase
	{
		IPersonService _personService;
		public PersonController(IPersonService personService)
		{
			_personService = personService;
		}

		[HttpGet]
		[Route("all-friends")]
		[Authorize(Roles = "USER,ADMIN")]
		public IActionResult GetAllFriends()
		{
			MultipleFriendsWithTypeDTO retVal = null;
			try
			{
				retVal = _personService.GetAllFriends(new TokenDTO
				{
					Token = Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization]
					.ToString().Substring(7)
				});
			}
			catch (Exception e)
			{
				return NotFound(new ResponseDTO { Message = e.Message });
			}

			if (retVal == null)
			{
				return NotFound(new ResponseDTO { Message = "Error trying to fetch friends!" });
			}

			return Ok(retVal);
		}

		[HttpPost]
		[Route("register")]
		public IActionResult RegisterNewUser([FromBody] PersonBasicInfoDTO userData)
		{
			try
			{
				if (!_personService.RegisterNewUser(userData))
				{
					return BadRequest();
				}
			}
			catch (Exception e)
			{
				return BadRequest(new ExceptionDTO() { Message = e.Message });
			}

			return Ok(new ResponseDTO { Message = "Successfully registered a new user!" });
		}

		[HttpPost]
		[Route("login")]
		public IActionResult Login([FromBody] LoginCredentialsDTO loginCredentials)
		{
			string token = "";
			try
			{
				token = _personService.Login(loginCredentials);
			}
			catch (Exception e)
			{
				return BadRequest(new ExceptionDTO() { Message = e.Message });
			}

			return Ok(new TokenDTO() { Token = token });
		}

		[HttpGet]
		[Route("search")]
		[Authorize(Roles = "USER,ADMIN")]
		public IActionResult SearchByNameAndSurname(string name, string surname)
		{
			if (name == null)
			{
				name = "";
			}
			if (surname == null)
			{
				surname = "";
			}

			SearchByNameAndSurnameResultsDTO retVal = null;

			try
			{
				retVal = _personService.SearchByNameAndSurname(
					new SearchByNameAndSurnameDTO() { Name = name, Surname = surname },
					new TokenDTO
					{
						Token = Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization]
					.ToString().Substring(7)
					});
			}
			catch
			{
				return NotFound(new ResponseDTO() { Message = "No users found!" });
			}

			if (retVal == null)
			{
				return NotFound(new ResponseDTO() { Message = "No users found!" });
			}

			return Ok(retVal);
		}

		[HttpPost]
		[Route("add-friend")]
		[Authorize(Roles = "USER,ADMIN")]
		public IActionResult AddFriend(string email, string type = "FRIEND")
		{
			string token = Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization]
				.ToString().Substring(7);

			try
			{
				if (!_personService.AddFriend(new AddingFriendDTO()
				{
					Email = email,
					Token = token,
					Type = EFriendType.DetermineFriendType(type)
				}))
				{
					return BadRequest(new ResponseDTO() { Message = "Error trying to add a friend!" });
				}
			}
			catch (Exception e)
			{
				return BadRequest(new ResponseDTO() { Message = e.Message });
			}

			return Ok(new ResponseDTO() { Message = "Successfully added a new friend!" });
		}

		[HttpDelete]
		[Route("remove-friend")]
		[Authorize(Roles = "USER,ADMIN")]
		public IActionResult RemoveFriend(string email)
		{
			string token = Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization]
				   .ToString().Substring(7);
			try
			{
				if (!_personService.RemoveFriend(new EmailTokenDTO() { Email = email, Token = token }))
				{
					return BadRequest(new ResponseDTO() { Message = "Failed to remove a friend!" });
				}
			}
			catch (Exception e)
			{
				return BadRequest(new ResponseDTO() { Message = e.Message });
			}

			return Ok(new ResponseDTO() { Message = "Successfully removed a friend!" });
		}

		[HttpDelete]
		[Route("delete-account")]
		[Authorize(Roles = "USER,ADMIN")]
		public IActionResult DeleteAccount(string email = "")
		{
			EmailTokenDTO info = new EmailTokenDTO()
			{
				Email = email,
				Token = Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization]
				   .ToString().Substring(7)
			};

			try
			{
				if (!_personService.DeleteAccount(info))
				{
					return BadRequest(new ResponseDTO() { Message = "Failed to delete account!" });
				}
			}
			catch (Exception e)
			{
				return BadRequest(new ResponseDTO() { Message = e.Message });
			}

			return Ok(new ResponseDTO() { Message = "Successfully deleted the account!" });
		}

		[HttpPut]
		[Route("approve-request")]
		[Authorize(Roles = "ADMIN")]
		public IActionResult UpdateRole(string email, string status)
		{
			try
			{
				if (!_personService.UpdateRole(new EmailRoleDTO() { Email = email, Role = status }))
				{
					return BadRequest(new ResponseDTO() { Message = "Failed to approve request!" });
				}
			}
			catch (Exception e)
			{
				return BadRequest(new ResponseDTO() { Message = e.Message });
			}

			return Ok(new ResponseDTO() { Message = "Successfully changed the user status!" });
		}

		[HttpPut]
		[Route("update-person")]
		[Authorize(Roles = "USER,ADMIN")]
		public IActionResult UpdatePerson(string name, string surname)
		{
			try
			{
				if (!_personService.UpdatePerson(
					new PersonBasicInfoDTO() { Name = name, Surname = surname },
					new TokenDTO()
					{
						Token = Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization]
							.ToString().Substring(7)
					}))
				{
					return BadRequest(new ResponseDTO() { Message = "Error trying to update data!" });
				}
			}
			catch (Exception e)
			{
				return BadRequest(new ResponseDTO() { Message = e.Message });
			}

			return Ok(new ResponseDTO() { Message = "Successfully updated the data!" });
		}

		[HttpPut]
		[Route("update-password")]
		[Authorize(Roles = "USER,ADMIN")]
		public IActionResult UpdatePassword([FromBody] OldNewPasswordDTO passwords)
		{
			PasswordTokenDTO info = new PasswordTokenDTO()
			{
				NewPassword = passwords.NewPassword,
				OldPassword = passwords.OldPassword,
				Token = Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization]
							.ToString().Substring(7)
			};
			try
			{
				if (!_personService.UpdatePassword(info))
				{
					return BadRequest(new ResponseDTO() { Message = "Error trying to change the password!" });
				}
			}
			catch (Exception e)
			{
				return BadRequest(new ResponseDTO() { Message = e.Message });
			}

			return Ok(new ResponseDTO() { Message = "Successfully changed the password!" });
		}

		[HttpGet]
		[Route("get-person-by-email")]
		[Authorize(Roles = "USER,ADMIN")]
		public IActionResult GetPersonByEmail(string email = "")
		{
			PersonBasicInfoDTO info = null;
			try
			{
				info = _personService.GetPerson(new EmailTokenDTO()
				{
					Email = email,
					Token = Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization]
								.ToString().Substring(7)
				});
			}
			catch (Exception e)
			{
				return BadRequest(new ResponseDTO() { Message = e.Message });
			}

			if (info == null)
			{
				return BadRequest(new ResponseDTO() { Message = "Error trying to fetch the data!" });
			}

			return Ok(info);
		}

		[HttpGet]
		[Route("get-all-people")]
		[Authorize(Roles = "USER,ADMIN")]
		public IActionResult GetAllPeople()
		{
			PeopleDTO people = null;
			try
			{
				people = _personService.GetAllPeople(new TokenDTO()
				{
					Token = Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization]
								.ToString().Substring(7)
				});
			}
			catch (Exception e)
			{
				return BadRequest(new ResponseDTO() { Message = e.Message });
			}

			return Ok(people);
		}

		[HttpGet]
		[Route("get-non-friends")]
		[Authorize(Roles = "USER,ADMIN")]
		public IActionResult GetNonFriends()
		{
			PeopleDTO people = null;
			try
			{
				people = _personService.GetNonFriends(new TokenDTO()
				{
					Token = Request.Headers[Microsoft.Net.Http.Headers.HeaderNames.Authorization]
								.ToString().Substring(7)
				});
			}
			catch (Exception e)
			{
				return BadRequest(new ResponseDTO() { Message = e.Message });
			}

			return Ok(people);
		}

		[HttpGet]
		[Route("get-requests")]
		[Authorize(Roles = "ADMIN")]
		public IActionResult GetRequests()
		{
			PeopleDTO people = null;
			try
			{
				people = _personService.GetAllRequests();
			}
			catch (Exception e)
			{
				return BadRequest(new ResponseDTO() { Message = e.Message });
			}

			return Ok(people);
		}
	}
}
