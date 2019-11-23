using LeaveTracker.Interfaces;
using LeaveTracker.Services;
using LeaveTracker.ViewModels;
using LeaveTracker.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LeaveTracker.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LeaveTracker
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LeaveViewerPage, LeaveViewerPageViewModel>();
            containerRegistry.RegisterForNavigation<LeaveDetailPage, LeaveDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<LeaveApplyPage, LeaveApplyPageViewModel>();
            containerRegistry.RegisterForNavigation<AddUserPage, AddUserPageViewModel>();


            containerRegistry.RegisterSingleton<User>();
            containerRegistry.RegisterSingleton<IFirebaseService, FirebaseService>();
        }
    }
}
