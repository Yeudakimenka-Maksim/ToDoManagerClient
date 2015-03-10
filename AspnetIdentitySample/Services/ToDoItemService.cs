using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoManager.Models;
using ToDoManager.localhost;


namespace ToDoManager.Services
{
	public class ToDoItemService : IToDoItemService
	{
		// this is fake solution! !NEVER TRY TO ACCESS DB FROM SERVICES WITHOUT A REASON!
        ToDoManager.localhost.ToDoManager _tm = new localhost.ToDoManager();
		private IUserRetrieverService _userService;

		public ToDoItemService(IUserRetrieverService userService)
		{
			_userService = userService;
		}

		public Task<IEnumerable<ToDoModel>> GetAll()
		{
			// Filter by user here
            return Task.FromResult(_tm.GetTodoList(_userService.GetCurrentUser().UserId,true).Select(x=>x.ToToDoModel()));
		}

		public Task<ToDoModel> Create(ToDoModel todo)
		{
			todo.Id = Guid.NewGuid().GetHashCode(); // almost random
            _tm.CreateToDoItem(todo.ToToDoItem(_userService.GetCurrentUser().UserId));
			return Task.FromResult(todo);
		}

		public Task<ToDoModel> GetById(int id)
		{
            var user = _userService.GetCurrentUser();
			return Task.FromResult(_tm.GetTodoList(user.UserId,true).FirstOrDefault(t=>t.ToDoId==id).ToToDoModel());
		}

		public Task<bool> RemoveById(int id)
		{
            _tm.DeleteToDoItem(id, true);
			return Task.FromResult(true);
		}

		public Task<ToDoModel> Update(ToDoModel todo)
		{
            _tm.UpdateToDoItem(todo.ToToDoItem(_userService.GetCurrentUser().UserId));
			return Task.FromResult(todo);
		}
	}
}