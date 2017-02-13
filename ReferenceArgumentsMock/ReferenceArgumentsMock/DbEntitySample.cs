using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceArgumentsMock
{
    public class DbEntityFactory<T> : IEntityContract<T> where T: class, new()
    {
        public void CreateEntity(out T entityToName)
        {
            entityToName = new T();
        }
    }
    class DbEntitySample 
    {
        protected DbEntitySample()
        {
            this.Guid = new Guid();
        }
        protected virtual Guid Guid { get; set; }      
    }
    interface IEntityContract<T> where T: class, new()
    {
        void CreateEntity(out T entityToName);
    }
}
