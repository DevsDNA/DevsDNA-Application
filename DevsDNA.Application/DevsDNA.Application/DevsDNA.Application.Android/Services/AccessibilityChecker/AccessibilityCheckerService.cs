[assembly: Xamarin.Forms.Dependency(typeof(DevsDNA.Application.Droid.Services.AccessibilityCheckerService))]
namespace DevsDNA.Application.Droid.Services
{
    using Android.Content;
    using Android.Views.Accessibility;
    using DevsDNA.Application.Services;

    public class AccessibilityCheckerService : IAccessibilityCheckerService
    {
        public bool IsVoiceAssistantActive()
        {
            using (AccessibilityManager accessibilityManager = (AccessibilityManager)MainActivity.CurrentActivity.GetSystemService(Context.AccessibilityService))
            {
                return accessibilityManager?.IsTouchExplorationEnabled ?? false;
            }   
        }
    }
}
