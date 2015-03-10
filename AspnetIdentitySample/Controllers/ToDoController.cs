using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToDoManager.Models;
using ToDoManager.Services;
using System.Linq;

namespace ToDoManager.Controllers
{
	/// <summary>
	/// The to do controller.
	/// </summary>
	public class ToDoController : Controller
	{
		private static IAuthenticationService _authenticationService;
		private static IToDoItemService _todoService;

		#region Constructors and Destructors

		static ToDoController()
		{
			var userService = new UserService();

			_authenticationService = userService;
			_todoService = new ToDoItemService(userService);

            _authenticationService.Login(null);
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// The all.
		/// </summary>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		public async Task<ActionResult> All()
		{
            //IEnumerable<ToDoModel> models;
            //using (ToDoModelContext context = new ToDoModelContext())
            //{
            //    models = context.Set<ToDoModel>();
            //}
            //return View(models);
			return View(await _todoService.GetAll());
		}

		/// <summary>
		/// The create.
		/// </summary>
		/// <returns>
		/// The <see cref="ActionResult"/>.
		/// </returns>
		public ActionResult Create()
		{
			
			return this.View();
		}

		/// <summary>
		/// The create.
		/// </summary>
		/// <param name="todo">
		/// The todo.
		/// </param>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		[HttpPost]
		public async Task<ActionResult> Create([Bind(Include = "Id,Description,IsDone")] ToDoModel todo)
		{
			
			if (this.ModelState.IsValid)
			{
                Thread t = new Thread(() => CreateToDoItemAsync(todo));
                t.Start();
                using (var context = new ToDoModelEntity())
                {
                    context.ToDoModels.Add(todo);
                    context.SaveChanges();
                }
				return this.RedirectToAction("Index");
			}
			return this.View(todo);
		}
        private void CreateToDoItemAsync(ToDoModel model){
            _todoService.Create(model);
        }
		/// <summary>
		/// The delete.
		/// </summary>
		/// <param name="id">
		/// The id.
		/// </param>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			ToDoModel todo;
            using (ToDoModelEntity context = new ToDoModelEntity())
            {
                todo = context.ToDoModels.FirstOrDefault(t => t.Id == id);
            }
            // = await _todoService.GetById(id.Value);
			if (todo == null)
			{
				return this.HttpNotFound();
			}

			return this.View(todo);
		}

        
		/// <summary>
		/// The delete confirmed.
		/// </summary>
		/// <param name="id">
		/// The id.
		/// </param>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		[HttpPost]
		[ActionName("Delete")]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
            ToDoModel todo;// = await _todoService.GetById(id);
            using (ToDoModelEntity context = new ToDoModelEntity())
            {
                todo = context.ToDoModels.FirstOrDefault(t => t.Id == id);
            }
			if (todo == null)
			{
				return this.HttpNotFound();
			}

            //Thread t = new Thread(() => DeleteAsync(id));
            //t.Start();
            using (var context = new ToDoModelEntity())
            {
                context.ToDoModels.Remove(context.ToDoModels.FirstOrDefault(m => m.Id == id));
                context.SaveChanges();
            }
			return this.RedirectToAction("Index");
		}

        private void DeleteAsync(int id)
        {
            _todoService.RemoveById(id);
        }
		/// <summary>
		/// The details.
		/// </summary>
		/// <param name="id">
		/// The id.
		/// </param>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

            ToDoModel todo; //await _todoService.GetById(id.Value);
            using (ToDoModelEntity context = new ToDoModelEntity())
            {
                todo = context.ToDoModels.FirstOrDefault(t => t.Id == id);
            }
            if (todo == null)
            {
                return this.HttpNotFound();
            }

			return this.View(todo);
		}

		/// <summary>
		/// The edit.
		/// </summary>
		/// <param name="id">
		/// The id.
		/// </param>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
            ToDoModel todo; //await _todoService.GetById(id.Value);
            using (ToDoModelEntity context = new ToDoModelEntity())
            {
                todo = context.ToDoModels.FirstOrDefault(t => t.Id == id);
            }
			if (todo == null)
			{
				return this.HttpNotFound();
			}

			return this.View(todo);
		}

		/// <summary>
		/// The edit.
		/// </summary>
		/// <param name="todo">
		/// The todo.
		/// </param>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		[HttpPost]
		public async Task<ActionResult> Edit([Bind(Include = "Id,Description,IsDone")] ToDoModel todo)
		{
			if (this.ModelState.IsValid)
			{
				//await _todoService.Update(todo);
                using (ToDoModelEntity context = new ToDoModelEntity())
                {
                    context.ToDoModels.Attach(todo);
                    context.Entry(todo).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
				return this.RedirectToAction("Index");
			}

			return this.View(todo);
		}

		/// <summary>
		/// The index.
		/// </summary>
		/// <returns>
		/// The <see cref="ActionResult"/>.
		/// </returns>
		public async Task<ActionResult> Index()
		{
            List<ToDoModel> result = null;
            using (ToDoModelEntity context = new ToDoModelEntity())
            {
                result = context.ToDoModels.ToList();
            }
			return View(result);
		}

        private void Synchronize()//resolve problem with id on server and client db
        {

        }
		#endregion
	}
}