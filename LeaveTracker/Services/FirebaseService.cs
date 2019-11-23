using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using LeaveTracker.Interfaces;
using LeaveTracker.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveTracker.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseClient firebase = new FirebaseClient("private", new FirebaseOptions());
        private readonly string _firebaseAPIKey = "private";

        public async Task<string> LoginAsync(Models.User user)
        {
            FirebaseAuthProvider authProvider = null;
            try
            {
                authProvider = new FirebaseAuthProvider(new FirebaseConfig(_firebaseAPIKey));
                return (await authProvider.SignInWithEmailAndPasswordAsync(string.Join("@", user.UserID, "gmail.com"), user.Password)).FirebaseToken;
            }
            catch (FirebaseAuthException firebaseEx)
            {
                return string.Empty;
            }
            finally
            {
                if (authProvider != null)
                    authProvider.Dispose();
            }
        }

        private async Task<List<LeaveDetail>> GetLeavesAsync(int day, string month, string year)
        {
            var path = string.Concat(new string[] { year, "/", month, "/", day.ToString() });
            var response = (await firebase.Child(path).OnceAsync<LeaveDetail>()).Select(element => new LeaveDetail
            {
                Name = element.Object.Name,
                Id = element.Object.Id,
                LeaveType = element.Object.LeaveType
            }).ToList();

            return response;
        }

        //Todo -instead of getting data for each date of the month, get the data for the entire month in one go and iterate locally.
        //This will drastically reduce the time taken to display the data.
        public async Task<List<CalendarItem>> GetAll(string month, string year)
        {
            List<CalendarItem> ItemList = new List<CalendarItem>();

            int iterationLimit = DateTime.DaysInMonth(int.Parse(year), (DateTime.ParseExact(month.Substring(0, 3), "MMM", CultureInfo.InvariantCulture)).Month);

            for (int i = 1; i <= iterationLimit; i++)
            {
                CalendarItem item = new CalendarItem();

                DateTime dateTime = new DateTime(int.Parse(year), DateTime.ParseExact(month.Substring(0, 3), "MMM", CultureInfo.InvariantCulture).Month, i);
                var res = await GetLeavesAsync(i, month, year);

                item.Date = dateTime.Day.ToString();
                item.Day = dateTime.DayOfWeek.ToString();
                item.LeaveCount = res.Count;
                item.Leaves = res;

                if (res.Count > 0)
                    item.HighlightColor = res.Count < 5 ? System.Drawing.Color.Yellow : System.Drawing.Color.Tomato;

                ItemList.Add(item);
            }

            return ItemList;
        }

        public async Task<bool> UpdateLeaveAsync(int day, string month, string year, LeaveDetail leaveDetail)
        {
            try
            {
                await firebase.Child(string.Join("/", year, month, day, leaveDetail.Id.Replace('.', '-'))).PutAsync(leaveDetail);
                return true;
            }
            catch (FirebaseException firebaseEx)
            {
                return false;
            }
        }

        public async Task<string> GetUserNameAsync(string id)
        {
            try
            {
                var result = await firebase.Child("Users/" + id.Replace('.', '-').ToLower() + "/").OnceAsync<string>();
                return result.ElementAt(0).Object;
            }
            catch (FirebaseException firebaseEx)
            {
                return null;
            }
        }

        public async Task<bool> AddUserAsync(string userName, string userId, string password)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(_firebaseAPIKey));

                await authProvider.CreateUserWithEmailAndPasswordAsync(string.Join("@", userId, "gmail.com"), password, userName);
                await firebase.Child(string.Join("/", "Users", userId.Replace('.', '-').ToLower(), "/Name/")).PutAsync<string>(userId);

                return true;
            }
            catch (FirebaseException firebaseEx)
            {
                return false;
            }
        }
    }
}
