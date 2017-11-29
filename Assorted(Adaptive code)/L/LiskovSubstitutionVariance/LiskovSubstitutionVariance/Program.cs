using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiskovSubstitutionVariance
{
    public interface IEntityRepositiry<TEntity> where TEntity : Entity
    {
        TEntity GetByID(Guid id);
    }
    public interface IEqualityComparer<in TEntity> where TEntity : Entity
    {
        bool Equals(TEntity left, TEntity right);
    }

    public class Entity
    {
        public string Guid { get; private set; }
        public string Name { get; private set; }
    }
    public class User : Entity
    {
        public string Email { get; private set; }
        public DateTime DateOfBirth { get; private set; }
    }
    // variance example
    public class UserRepository : IEntityRepositiry<User>
    {
        public User GetByID(Guid id)
        {
            return new User();
        }
    }
    // contravariance example
    public class EntityEqualityComparer : IEqualityComparer<Entity>
    {
        public bool Equals(Entity left, Entity right)
        {
            return left.Guid == right.Guid;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Liskov rules
            // There must be contravariance of the method arguments in the subtype.
            // There must be covariance of the return types in the subtype.
            // No new exceptions are allowed that are not part of an expected exception class hierarchy

        }
    }
}
