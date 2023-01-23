using CommunityToolkit.Mvvm.ComponentModel;

namespace TalaGrid.ViewModels
{
    public partial class AppShellViewModel : ObservableObject
    {
        public AppShellViewModel()
        {
#if WINDOWS
            FlyoutBehaviourStr = "Locked";
#elif ANDROID
            FlyoutBehaviourStr = "Flyout";
#endif
        }

        [ObservableProperty]
        string flyoutBehaviourStr;

    }
}
