[assembly: Xamarin.Forms.Dependency(typeof(DevsDNA.Application.iOS.Services.AccessibilityCheckerService))]
namespace DevsDNA.Application.iOS.Services
{
    using DevsDNA.Application.Services;
    using System;
    using UIKit;

    public class AccessibilityCheckerService : IAccessibilityCheckerService
    {
        public bool IsVoiceAssistantActive()
        {
            return UIAccessibility.IsVoiceOverRunning;
        }
    }
}
