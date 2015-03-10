using System.Security.Authentication;

using ToDoManager.Models;

namespace ToDoManager.Services
{
	public class UserService : IAuthenticationService, IUserRetrieverService
	{
		private static readonly object _syncObj = new object();
		private volatile static UserModel _currentUser;

		public UserModel Login(UserModel user)
		{
			if (_currentUser == null)
			{
				lock (_syncObj)
				{
					if (_currentUser == null)
					{
                        _currentUser = user == null ? new UserModel { UserId = 1, UserName = "Kaval                                                                                                                           ", Password = "Test" } : user;
					}
				}
			}

			return _currentUser;
		}

		public UserModel GetCurrentUser()
		{
			if (_currentUser == null)
			{
				throw new AuthenticationException("There is no user here.");
			}

			return _currentUser;
		}
	}
}