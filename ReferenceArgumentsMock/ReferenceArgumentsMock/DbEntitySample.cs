using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceArgumentsMock
{
    public abstract class DbEntityFactory<T> : IEntityContract<T> where T: class, new()
    {
        public void CreateEntity(out T entityToCreate)
        {
            entityToCreate = new T();
        }

        public abstract void ModifyEntity(ref T entityToModify, object attribute);
    }
    public class DbEntitySample 
    {
        public DbEntitySample()
        {
            this.Guid = new Guid();
        }
        public virtual Guid Guid { get; set; }      

        public string Name { get; set; }
    }

    interface IEntityContract<T> where T: class, new()
    {
        void CreateEntity(out T entityToName);
    }

    public class WestCoastCustoms : DbEntityFactory<DbEntitySample>
    {
        public override void ModifyEntity(ref DbEntitySample entityToModify, object attribute)
        {
            entityToModify.Name = (string)attribute;
        }
    }
}
