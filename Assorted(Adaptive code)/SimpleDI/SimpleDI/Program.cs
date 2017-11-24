using System;

namespace SimpleDI
{
    public interface ISecurityService
    {
        void ChangeUserPassword(string userID, string newPassword);
    }
    public interface IUserRepository
    {
        User GetByID(string userID);
    }

    public class SecurityService : ISecurityService
    {
        // 2.0 Add interface dependency
        private readonly IUserRepository userRepository;
        public SecurityService(IUserRepository userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException("userRepository");
        }
        public void ChangeUserPassword(string userID, string newPassword)
        {
            // 1.0 That code is not here initially
            // 2.0 use DI
            //UserRepository userRepository = new UserRepository();
            User user = userRepository.GetByID(userID);
            user.ChangePassword(newPassword);
        }
    }

    public class User
    {
        internal void ChangePassword(string newPassword)
        {
            // STUB
            throw new NotImplementedException();
        }
    }
    public class UserRepository : IUserRepository
    {
        public User GetByID(string userID)
        {
            // STUB
            throw new NotImplementedException();
        }
    }
    public class AccountController
    {
        //private readonly SecurityService securityService;
        // 2.0 depend on interface
        private readonly ISecurityService securityService;
        public AccountController(ISecurityService securityService)
        {
            // NB. 
            // Dependent on concrete class
            // Dependent on that class dependencies
            // Impossible to test
            // this.securityService = new SecurityService();
            // 2.0 take it from injection
            this.securityService = securityService ?? throw new ArgumentNullException("securityService");
        }
        public void ChangePassword(string userID, string newPassword)
        {
            // NB.
            // Dependent on concrete class
            // Dependent on that class dependencies
            // Impossible to test
            // UserRepository userRepository = new UserRepository();
            // User user = userRepository.GetByID(userID);
            // 1.0 Move implementation to sec.service
            this.securityService.ChangeUserPassword(userID, newPassword);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var controller = new AccountController(new SecurityService(new UserRepository()));
            // TODO
            // who is creating that DI chain in MVVM?
        }
    }
}
