namespace DevsDNA.Application.Droid.Services.Notifications
{
    using System;
    using Android.App;
    using Android.Content;
	using Android.Graphics;
	using Android.Service.Notification;
    using Android.Support.V4.App;
    using Android.Util;
	using DevsDNA.Application.Common;
	using DevsDNA.Application.Services.Notifications;
    using Firebase.Messaging;
	using Xamarin.Essentials;

	[Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseNotificationService : FirebaseMessagingService
    {
        private const string TAG = "FirebaseNotificationService";

        public object NotificationsService { get; private set; }

        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);
            Services.NotificationService.NotificationToken = p0;
			Preferences.Set(SettingsKeyValues.PreferenceKeyNotificationToken, p0);

            Log.Debug(TAG, "FCM token: " + Services.NotificationService.NotificationToken);
        }

        public override void OnMessageReceived(RemoteMessage p0)
        {
            base.OnMessageReceived(p0);

            RemoveFirebaseOriginalNotifications();

			PushNotificationInfo notificationInfo = ReadNotification(p0);
            if (notificationInfo != null)
            {
                CreateNotification(notificationInfo);
            }
        }

        private void RemoveFirebaseOriginalNotifications()
        {
            if (GetSystemService(NotificationService) is NotificationManager notificationManager)
            {
                StatusBarNotification[] activeNotifications = notificationManager.GetActiveNotifications();
                if (activeNotifications == null)
                    return;

                foreach (StatusBarNotification item in activeNotifications)
                {
                    string tag = item.Tag;
                    int id = item.Id;

                    if (tag != null && tag.Contains("FCM-Notification"))
                        notificationManager.Cancel(tag, id);
                }
            }
        }

        //{"data":{"message":"Notification Hub test notification","title":"Test01","pushType":"1"}}
        private PushNotificationInfo ReadNotification(RemoteMessage remoteMessage)
        {
            PushNotificationInfo notificationInfo = new PushNotificationInfo();

			if (remoteMessage.Data.ContainsKey("message"))
			{
                notificationInfo.Message = remoteMessage.Data["message"];
            }

            if (remoteMessage.Data.ContainsKey("title"))
            {
                notificationInfo.Title = remoteMessage.Data["title"];
            }
            else
            {
                notificationInfo.Title = "DevsDNA";
            }

            if (remoteMessage.Data.ContainsKey("pushType"))
            {
                notificationInfo.Type = (PushNotificationsType)Convert.ToInt32(remoteMessage.Data["pushType"]);
            }

            return notificationInfo;
        }

        private void CreateNotification(PushNotificationInfo notificationInfo)
        {
			NotificationManager notificationManager = NotificationManager.FromContext(this);
			Intent uiIntent = new Intent(this, typeof(MainActivity));
            uiIntent.AddFlags(ActivityFlags.SingleTop); //Use existing activity
            uiIntent.PutExtra("pushType", (int)notificationInfo.Type);
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, uiIntent, PendingIntentFlags.OneShot);

            // Configure the notification channel.
            string channelName = "DevsDNA";
            string channelId = "devsdna";
            NotificationChannel notificationChannel = new NotificationChannel(channelId, channelName, NotificationImportance.High) { LockscreenVisibility = NotificationVisibility.Public };
            notificationManager.CreateNotificationChannel(notificationChannel);

			NotificationCompat.Builder builder = new NotificationCompat.Builder(this, notificationChannel.Id)
				.SetSmallIcon(Droid.Resource.Drawable.notificationSmallIcon)
				.SetColor(Application.Context.GetColor(Droid.Resource.Color.colorPrimary))
				.SetLargeIcon(BitmapFactory.DecodeResource(Resources, Droid.Resource.Drawable.ic_notification))
                .SetChannelId(channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(notificationInfo.Title ?? "DevsDNA")
                .SetContentText(notificationInfo.Message)
                .SetVisibility((int)NotificationVisibility.Public)
                .SetAutoCancel(true);

			Notification notification = builder.Build();
            notificationManager.Notify(0, notification);
        }
    }
}