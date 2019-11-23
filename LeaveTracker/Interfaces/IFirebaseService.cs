using LeaveTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveTracker.Interfaces
{
    public interface IFirebaseService
    {
        Task<string> LoginAsync(User user);
        Task<List<CalendarItem>> GetAll(string month, string year);
        Task<bool> UpdateLeaveAsync(int day, string month, string year, LeaveDetail leaveDetail);
        Task<string> GetUserNameAsync(string id);
        Task<bool> AddUserAsync(string userName, string userId, string password);
    }
}
