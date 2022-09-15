using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Back.Miscellaneous
{
	public static class VerifyNewUser
	{
		public static bool VerifyNewUserInformation(string name, string surname, string email, string password)
		{
			if (string.IsNullOrWhiteSpace(name)
				|| string.IsNullOrWhiteSpace(surname)
				|| string.IsNullOrWhiteSpace(email)
				|| string.IsNullOrWhiteSpace(password))
			{
				throw new Exception("All fields must be filled!");
			}
			else if (!Regex.IsMatch(name, @"^[a-zA-Z]+$") || !Regex.IsMatch(surname, @"^[a-zA-Z]+$"))
			{
				throw new Exception("Name and password can only be composed of letters!");
			}

			try
			{
				if (email != "admin")
				{
					MailAddress m = new MailAddress(email);
				}
			}
			catch (FormatException)
			{
				throw new Exception("Email is in incorrect format!");
			}

			if (password.Length < 7)
			{
				throw new Exception("Password must be at least 7 characters long!");
			}

			return true;
		}
	}
}
