using eUseControl.Domain.Entities.User;

namespace eUseControl.BusinessLogic.Interface
{
    public interface IUserServices
    {
        bool RegisterUser(User user);
        User LoginUser(User user);
        bool RemoveUser(string name, string email);
        User GetUserById(int id);
        UserMembership GetUserMembershipById(int id);
        
        int? SaveUserMembership(UserMembership userMembership);
        bool UpdateUserMembership(int? userMembershipId, User userId);
     }
}
