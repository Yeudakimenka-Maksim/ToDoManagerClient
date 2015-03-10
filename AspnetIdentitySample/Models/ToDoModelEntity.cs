namespace ToDoManager.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ToDoModelEntity : DbContext
    {
        public ToDoModelEntity()
            : base("name=ToDoModelEntity")
        {
        }

        public virtual DbSet<ToDoModel> ToDoModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
