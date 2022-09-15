using Back.DTO;
using Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Interfaces
{
	public interface IPersonService
	{
		MultipleFriendsWithTypeDTO GetAllFriends(TokenDTO tokenString);
		bool RegisterNewUser(PersonBasicInfoDTO newUser);
		string Login(LoginCredentialsDTO loginCredentials);
		SearchByNameAndSurnameResultsDTO SearchByNameAndSurname(SearchByNameAndSurnameDTO searchParams, TokenDTO token);
		bool AddFriend(AddingFriendDTO info);
		bool RemoveFriend(EmailTokenDTO info);
		bool DeleteAccount(EmailTokenDTO info);
		bool UpdateRole(EmailRoleDTO info);
		bool UpdatePerson(PersonBasicInfoDTO info, TokenDTO token);
		bool UpdatePassword(PasswordTokenDTO info);
		PersonBasicInfoDTO GetPerson(EmailTokenDTO info);
		PeopleDTO GetAllPeople(TokenDTO token);
		PeopleDTO GetNonFriends(TokenDTO token);
		PeopleDTO GetAllRequests();

	}
}
