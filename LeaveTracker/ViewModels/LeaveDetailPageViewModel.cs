using LeaveTracker.Models;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;

namespace LeaveTracker.ViewModels
{
    public class LeaveDetailPageViewModel : ViewModelBase
    {
        #region Fields
        private List<LeaveDetail> _leaveDetails;
        #endregion

        #region Properties        
        public List<LeaveDetail> LeaveDetails
        {
            get { return _leaveDetails; }
            set { SetProperty(ref _leaveDetails, value); }
        }
        #endregion

        public LeaveDetailPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            Title = "Leave Details";
        }

        #region Methods        
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            CalendarItem calendarItem = parameters.GetValue<CalendarItem>("details");
            LeaveDetails = calendarItem.Leaves;
        }
        #endregion
    }
}
