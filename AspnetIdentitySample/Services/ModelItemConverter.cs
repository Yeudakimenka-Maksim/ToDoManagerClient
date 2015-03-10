using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToDoManager.localhost;
using ToDoManager.Models;

namespace ToDoManager.Services
{
    public static class ModelItemConverter
    {
        public static ToDoModel ToToDoModel(this ToDoItem item)
        {
            return new ToDoModel()
            {
                Id = item.ToDoId,
                Description = item.Name,
                IsDone = item.IsCompleted
            };
        }

        public static ToDoItem ToToDoItem(this ToDoModel model, int userId)
        {
            return new ToDoItem()
            {
               ToDoId = model.Id,
               Name = model.Description,
               IsCompleted = model.IsDone,
               UserId = userId,
               IsCompletedSpecified=true,
               ToDoIdSpecified=true,
               UserIdSpecified=true
            };
        }
    }
}