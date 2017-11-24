using System;

namespace NullObject
{
    public interface IUser
    {
        void IncrementSessionTicket();
    }
    public interface IUserRepository
    {
        IUser GetByID(Guid guid);
    }

    public class User : IUser
    {
        public void IncrementSessionTicket()
        {
            // STUB
        }
    }
    public class NullUser : IUser
    {
        public void IncrementSessionTicket()
        {
            // DO nothing
        }
    }

    class UserRepository : IUserRepository
    {
        public IUser GetByID(Guid guid)
        {
            //return null;
            // 3.0
            return new NullUser();
        }
    }
    class Program
    {
        static IUserRepository userRepository = new UserRepository();

        static void Main(string[] args)
        {
            var user = userRepository.GetByID(Guid.NewGuid());
            // 1.0
            try
            {
                // Without the Null Object pattern, this line could throw an exception
                user.IncrementSessionTicket();
            }
            catch (Exception)
            {
                Console.WriteLine("Yap, exception");
            }
            // 2.0
            // client code should not check for null in any form
            if (user != null)
            {
                user.IncrementSessionTicket();
            }
            // 2.1
            user?.IncrementSessionTicket();
            // 3.0 nothing is thrown as we expect NullUser object
            user.IncrementSessionTicket();
            // NB IsNull method is antipattern
            // TODO See C#8.0 nullable reference type update
        }
    }
}
