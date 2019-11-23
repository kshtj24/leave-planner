using LeaveTracker.Interfaces;
using LeaveTracker.Models;
using Prism;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Globalization;

namespace LeaveTracker.ViewModels
{
    public class LeaveApplyPageViewModel : ViewModelBase
    {
        #region Fields
        private string _leaveType;
        private DateTime _leaveDate;
        private User _user;
        #endregion

        #region Properties
        public DateTime LeaveDate
        {
            get { return _leaveDate; }
            set { SetProperty(ref _leaveDate, value); }
        }
        public string LeaveType
        {
            get { return _leaveType; }
            set { SetProperty(ref _leaveType, value); }
        }

        public DelegateCommand ApplyLeaveCommand { get; private set; }
        public string UserName { get; set; }
        #endregion

        public LeaveApplyPageViewModel(INavigationService navigationService, IFirebaseService firebaseService) : base(navigationService, firebaseService)
        {
            ApplyLeaveCommand = new DelegateCommand(ApplyLeave, CanApplyLeave);
            _user = PrismApplicationBase.Current.Container.Resolve(typeof(User)) as User;
            UserName = _user.UserName;
            LeaveDate = DateTime.Now;
        }

        #region Methods
        private async void ApplyLeave()
        {
            try
            {
                ActivityIndicatorTitle = "Updating leave. Please wait.";
                IsBusy = true;

                LeaveDetail leaveDetail = PrismApplicationBase.Current.Container.Resolve(typeof(LeaveDetail)) as LeaveDetail;

                leaveDetail.Name = _user.UserName;
                leaveDetail.Id = _user.UserID;
                leaveDetail.LeaveType = _leaveType;

                //Todo-- Add popup to confirm if the data has been updated successfully.
                await FirebaseService.UpdateLeaveAsync(_leaveDate.Day, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_leaveDate.Month), _leaveDate.Year.ToString(), leaveDetail);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanApplyLeave()
        {
            return true;
        }
        #endregion
    }
}
