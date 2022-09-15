using AutoMapper;
using Back.DTO;
using Back.Interfaces;
using Back.Interfaces.Strategy;
using Back.LogStrategy;
using Back.Miscellaneous;
using Back.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Back.Services
{
	public class PersonService : IPersonService
	{
		IMapper _mapper;
		IUnitOfWork _unitOfWork;
		IFriendshipService _friendshipService;
		IConfiguration _configuration;
		IStrategyLogger _logger;

		public PersonService(IMapper mapper,
			IUnitOfWork unitOfWork,
			IFriendshipService friendshipService,
			IConfiguration configuration,
			IStrategyLogger strategyLogger)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_friendshipService = friendshipService;
			_configuration = configuration;
			_logger = strategyLogger;
		}

		public MultipleFriendsWithTypeDTO GetAllFriends(TokenDTO tokenString)
		{
			JWTTokenHelper _tokenHelper = JWTTokenHelper.GetInstance();

			var person = _unitOfWork.PersonRepo.GetPersonWithFriends(
				_tokenHelper.GetEmail(tokenString.Token));
			if (person == null)
			{
				_logger.SetStrategy("WARNING");
				_logger.DoLog("No friends were found!");

				return null;
			}

			MultipleFriendsWithTypeDTO friends = new MultipleFriendsWithTypeDTO()
			{
				Friends = new List<FriendWithTypeDTO>()
			};

			foreach (Friendship friendship in person.Friendships)
			{
				friendship.Friend.Password = null;
				friendship.Friend.Friends = null;

				Person friend = new Person()
				{
					Name = friendship.Friend.Name,
					Surname = friendship.Friend.Surname,
					Email = friendship.Friend.Email
				};

				friends.Friends.Add(new FriendWithTypeDTO()
				{
					Friend = _mapper.Map<PersonBasicInfoDTO>(friend),
					Type = ((FriendType)friendship.FriendType).ToString()
				});
			}

			_logger.SetStrategy("INFO");
			_logger.DoLog("Friends have been found!");

			return friends;
		}

		public bool RegisterNewUser(PersonBasicInfoDTO userData)
		{
			VerifyNewUser.VerifyNewUserInformation(userData.Name,
				userData.Surname, userData.Email, userData.Password);

			Person newUser = new Person()
			{
				Name = userData.Name,
				Surname = userData.Surname,
				Email = userData.Email,
				Password = BCrypt.Net.BCrypt.HashPassword(userData.Password),
				Role = "REQUEST",
				Friendships = new List<Friendship>()
			};

			_unitOfWork.PersonRepo.Add(newUser);
			_unitOfWork.Complete();

			_logger.SetStrategy("INFO");
			_logger.DoLog("New user registered successfully!");

			return true;
		}

		public string Login(LoginCredentialsDTO loginCredentials)
		{
			JWTTokenHelper _tokenHelper = JWTTokenHelper.GetInstance();

			Person person = _unitOfWork.PersonRepo.GetPersonByEmail(loginCredentials.Email);
			if (person == null)
			{
				_logger.SetStrategy("Error");
				_logger.DoLog("Person with the provided email is not found!");

				throw new Exception("Person with the provided email is not found!");
			}

			if (!BCrypt.Net.BCrypt.Verify(loginCredentials.Password, person.Password))
			{
				_logger.SetStrategy("Error");
				_logger.DoLog("Invalid password!");

				throw new Exception("Invalid password!");
			}

			if (person.Role != "ADMIN" && person.Role != "USER")
			{
				_logger.SetStrategy("WARNING");
				_logger.DoLog("Unable to log in, user approval is pending!");

				throw new Exception("Unable to log in, user approval is pending!");
			}

			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Role, person.Role),
				new Claim("email", loginCredentials.Email)
			};

			string token = _tokenHelper.GetJWTToken(person,
				_configuration.GetSection("SecretKey").Value, claims);

			_logger.SetStrategy("INFO");
			_logger.DoLog("User logged in successfully!");

			return token;
		}

		public SearchByNameAndSurnameResultsDTO SearchByNameAndSurname(SearchByNameAndSurnameDTO searchParams, TokenDTO token)
		{
			JWTTokenHelper helper = new JWTTokenHelper();
			List<Person> people = _unitOfWork.PersonRepo.
				SearchByNameAndSurname(searchParams.Name ?? "",
				searchParams.Surname ?? "");

			var retVal = new SearchByNameAndSurnameResultsDTO() { People = people };

			if (retVal.People.Count > 0)
			{
				_logger.SetStrategy("INFO");
				_logger.DoLog("User found!");
			}
			else
			{
				_logger.SetStrategy("WARNING");
				_logger.DoLog("User not found!");
			}

			var friends = this.GetAllFriends(token);
			foreach (var p in friends.Friends)
			{
				if(!retVal.People.Where(x => x.Email == p.Friend.Email).Any())
				{
					continue;
				}
				retVal.People.Remove(retVal.People.Where(x => x.Email == p.Friend.Email).First());
			}

			if (retVal.People.Where(x => x.Email == helper.GetEmail(token.Token)).Any())
			{
				retVal.People.Remove(retVal.People.Where(x => x.Email == helper.GetEmail(token.Token)).First());
			}

			return retVal;
		}

		public bool AddFriend(AddingFriendDTO info)
		{
			JWTTokenHelper _tokenHelper = JWTTokenHelper.GetInstance();

			Person friend = _unitOfWork.PersonRepo.GetPersonByEmail(info.Email);
			Person user = _unitOfWork.PersonRepo.GetPersonWithFriends(_tokenHelper.GetEmail(info.Token));

			if (friend == null || user == null || (friend.Role != "USER" && friend.Role != "ADMIN"))
			{
				_logger.SetStrategy("ERROR");
				_logger.DoLog("A user with a given email address doesn't exist!");

				throw new Exception("A user with a given email address doesn't exist!");
			}
			else if (user.Friendships == null)
			{
				user.Friendships = new List<Friendship>();
			}
			else if (user.Friendships.Where(x => x.Friend.Email == info.Email).Any())
			{
				_logger.SetStrategy("WARNING");
				_logger.DoLog("User is already a friend!");

				throw new Exception("User is already a friend!");
			}

			user.Friendships.Add(new Friendship()
			{
				User = user,
				Friend = friend,
				FriendType = ((int)info.Type)
			});

			_logger.SetStrategy("INFO");
			_logger.DoLog("Friend has been successfully added!");

			_unitOfWork.Complete();

			return true;
		}

		public bool RemoveFriend(EmailTokenDTO info)
		{
			JWTTokenHelper _tokenHelper = JWTTokenHelper.GetInstance();

			Person user = _unitOfWork.PersonRepo.GetPersonWithFriends(_tokenHelper.GetEmail(info.Token));
			if (user == null)
			{
				_logger.SetStrategy("ERROR");
				_logger.DoLog("A user with a given email address doesn't exist!");

				throw new Exception("A user with a given email address doesn't exist!");
			}
			else if (user.Friendships == null || user.Friendships.Count < 1)
			{
				_logger.SetStrategy("ERROR");
				_logger.DoLog("User has no friends!");

				throw new Exception("User has no friends!");
			}
			else if (!user.Friendships.Where(x => x.Friend.Email == info.Email).Any())
			{
				_logger.SetStrategy("ERROR");
				_logger.DoLog("The user isn't in the friend list!");

				throw new Exception("The user isn't in the friend list!");
			}

			user.Friendships.Remove(user.Friendships.Where(x => x.Friend.Email == info.Email).First());

			_unitOfWork.Complete();

			_logger.SetStrategy("INFO");
			_logger.DoLog("Friend has been removed successfully!");

			return true;
		}

		public bool DeleteAccount(EmailTokenDTO info)
		{
			JWTTokenHelper _tokenHelper = JWTTokenHelper.GetInstance();

			string email = "";
			if (string.IsNullOrWhiteSpace(info.Email))
			{
				email = _tokenHelper.GetEmail(info.Token);
			}
			else
			{
				email = info.Email;
			}

			Person person = _unitOfWork.PersonRepo.GetPersonByEmail(email);
			if (person == null)
			{
				_logger.SetStrategy("ERROR");
				_logger.DoLog("Account removal failed!");

				throw new Exception("Account removal failed!");
			}

			_unitOfWork.FriendshipRepo.DeleteWhereUserId(person.Id);
			_unitOfWork.FriendshipRepo.DeleteWhereFriendId(person.Id);
			bool retVal = _unitOfWork.PersonRepo.RemovePersonByEmail(email);
			if (!retVal)
			{
				_logger.SetStrategy("ERROR");
				_logger.DoLog("Account removal failed!");

				throw new Exception("Account removal failed!");
			}

			_unitOfWork.Complete();

			_logger.SetStrategy("INFO");
			_logger.DoLog("Account has been removed successfully!");

			return retVal;
		}

		public bool UpdateRole(EmailRoleDTO info)
		{
			JWTTokenHelper _tokenHelper = JWTTokenHelper.GetInstance();

			if (info.Role != "USER" && info.Role != "ADMIN")
			{
				_logger.SetStrategy("WARNING");
				_logger.DoLog("Can't use the given role!");

				throw new Exception("Can't use the given role!");
			}

			if (!_unitOfWork.PersonRepo.ChangePersonRole(info.Email, info.Role))
			{
				_logger.SetStrategy("ERROR");
				_logger.DoLog("Failed to change users role!");

				throw new Exception("Failed to change users role!");
			}

			_unitOfWork.Complete();

			_logger.SetStrategy("INFO");
			_logger.DoLog("Role has been updated!");

			return true;
		}

		public bool UpdatePerson(PersonBasicInfoDTO info, TokenDTO token)
		{
			JWTTokenHelper _tokenHelper = JWTTokenHelper.GetInstance();

			string email = _tokenHelper.GetEmail(token.Token);
			Person person = _unitOfWork.PersonRepo.GetPersonByEmail(email);

			if (person == null)
			{
				_logger.SetStrategy("ERROR");
				_logger.DoLog("A user with a given email address doesn't exist!");

				throw new Exception("A user with a given email address doesn't exist!");
			}
			if (!VerifyNewUser.VerifyNewUserInformation(info.Name, info.Surname, email, "password"))
			{
				_logger.SetStrategy("ERROR");
				_logger.DoLog("User information is invalid!");

				throw new Exception("User information is invalid!");
			}

			person.Name = info.Name;
			person.Surname = info.Surname;

			_unitOfWork.Complete();

			_logger.SetStrategy("INFO");
			_logger.DoLog("User information has been updated!");

			return true;
		}

		public bool UpdatePassword(PasswordTokenDTO info)
		{
			JWTTokenHelper _tokenHelper = JWTTokenHelper.GetInstance();

			string email = _tokenHelper.GetEmail(info.Token);
			Person person = _unitOfWork.PersonRepo.GetPersonByEmail(email);

			if (person == null)
			{
				_logger.SetStrategy("ERROR");
				_logger.DoLog("A user with a given email address doesn't exist!");

				throw new Exception("A user with a given email address doesn't exist!");
			}
			else if (!BCrypt.Net.BCrypt.Verify(info.OldPassword, person.Password))
			{
				_logger.SetStrategy("ERROR");
				_logger.DoLog("Password confirmation failed! Invalid password!");

				throw new Exception("Password confirmation failed! Invalid password!");
			}

			person.Password = BCrypt.Net.BCrypt.HashPassword(info.NewPassword);

			_unitOfWork.Complete();

			_logger.SetStrategy("INFO");
			_logger.DoLog("User information has been updated!");

			return true;
		}

		/// <summary>
		/// If email is provided use it as such, if not, use the email from the token claims.
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>

		public PersonBasicInfoDTO GetPerson(EmailTokenDTO info)
		{
			JWTTokenHelper _tokenHelper = JWTTokenHelper.GetInstance();
			string email = "";

			if (!string.IsNullOrWhiteSpace(info.Email))
			{
				email = info.Email;
			}
			else
			{
				email = _tokenHelper.GetEmail(info.Token);
			}

			Person person = _unitOfWork.PersonRepo.GetPersonByEmail(email);

			if (person == null || (person.Role != "ADMIN" && person.Role != "USER"))
			{
				_logger.SetStrategy("ERROR");
				_logger.DoLog("A user with a given email address doesn't exist!");

				throw new Exception("A user with a given email address doesn't exist!");
			}

			person.Password = "";

			_logger.SetStrategy("INFO");
			_logger.DoLog("Returning person info!");

			return _mapper.Map<PersonBasicInfoDTO>(person);
		}

		public PeopleDTO GetAllPeople(TokenDTO token)
		{
			PeopleDTO retVal = new PeopleDTO()
			{
				People = _unitOfWork.PersonRepo.GetAll().
					Where(x => (x.Role == "USER" || x.Role == "ADMIN")).ToList()

			};

			JWTTokenHelper _tokenHelper = JWTTokenHelper.GetInstance();
			string email = _tokenHelper.GetEmail(token.Token);
			retVal.People.Remove(retVal.People.Where(x => x.Email == email).First());

			foreach (Person p in retVal.People)
			{
				p.Password = "";
			}

			_logger.SetStrategy("INFO");
			_logger.DoLog("Returning poeple info!");

			return retVal;
		}

		public PeopleDTO GetNonFriends(TokenDTO token)
		{
			PeopleDTO retVal = new PeopleDTO()
			{
				People = _unitOfWork.PersonRepo.GetAll().
					Where(x => (x.Role == "USER" || x.Role == "ADMIN")).ToList()
			};

			JWTTokenHelper _tokenHelper = JWTTokenHelper.GetInstance();
			string email = _tokenHelper.GetEmail(token.Token);
			retVal.People.Remove(retVal.People.Where(x => x.Email == email).First());

			var friends = this.GetAllFriends(token);
			foreach (var p in friends.Friends)
			{
				if (!retVal.People.Where(x => x.Email == p.Friend.Email).Any())
				{
					continue;
				}
				retVal.People.Remove(retVal.People.Where(x => x.Email == p.Friend.Email).First());
			}

			foreach (Person p in retVal.People)
			{
				p.Password = "";
			}

			_logger.SetStrategy("INFO");
			_logger.DoLog("Returning poeple info!");

			return retVal;
		}

		public PeopleDTO GetAllRequests()
		{
			PeopleDTO retVal = new PeopleDTO()
			{
				People = _unitOfWork.PersonRepo.GetAll().
					Where(x => x.Role == "REQUEST").ToList()
			};

			foreach (Person p in retVal.People)
			{
				p.Password = "";
			}

			_logger.SetStrategy("INFO");
			_logger.DoLog("Returning poeple info!");

			return retVal;
		}
	}
}
