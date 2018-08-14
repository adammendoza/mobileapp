using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Toggl.Foundation.Extensions
{
    public static class NavigationExtensions
    {
        public static Task ForkNavigate<TDaneelViewModel, TGiskardViewModel>(this IMvxNavigationService navigationService)
            where TDaneelViewModel : IMvxViewModel
            where TGiskardViewModel : IMvxViewModel
        {
            var platformInfo = Mvx.Resolve<PlatformInfo>();

            if (platformInfo.Platform == Platform.Daneel)
            {
                return navigationService.Navigate<TDaneelViewModel>();
            }

            return navigationService.Navigate<TGiskardViewModel>();
        }
    }
}
