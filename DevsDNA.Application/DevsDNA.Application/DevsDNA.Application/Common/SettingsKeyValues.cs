namespace DevsDNA.Application.Common
{
	using DevsDNA.Application.Helpers;

	public static class SettingsKeyValues
    {
		public const string PreferenceKeyNotificationToken = "NotificationToken";
		public const string PreferenceKeyFirstTimeKey = "firstTime";


		public const string FacebookEndPoint = "https://graph.facebook.com";
        public const string FacebookPageId = "434033656806892";

        public const string UrlDevsDNAEndPoint = "https://www.devsdna.com";
		public const string UrlDevsDNABlog = UrlDevsDNAEndPoint + "/feed/";

        public const string YouTubeEndPoint = "https://www.youtube.com/";
        public const string YouTubeChannelId = "UCulG_4j8ggcXJKyeXuHcvdA";

		public const string UrlDevsDNAFacebook = "https://www.facebook.com/DevsDNA/";
		public const string UrlDevsDNATwitter = "https://twitter.com/devsdna";
		public const string UrlDevsDNALinkedIn = "https://www.linkedin.com/company/devsdna/";
		public const string UrlDevsDNAInstagram = "https://www.instagram.com/devsdna";
		public const string EmailDevsDNA = "info@devsdna.com";
		public const string PhoneNumberDevsDNA = "+34627273613";
		public const string UrlDevsDNAPlayStore = "https://play.google.com/store/apps/details?id=com.devsdna.application";
		public const string UrlDevsDNAAppStore = "https://apps.apple.com/us/app/id1526657968";

		public static string NotificationHubName = Secrets.NotificationHubName;
		public static string NotificationConnectionString = Secrets.NotificationHubConnectionString;

		public const string ResolutionGroupName = "DevsDNA";

		public static string MessagingCenterOrientationChangedMessage = "OrientationChanged";		
	}
}
