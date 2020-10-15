namespace DevsDNA.Application.Services
{
	using DevsDNA.Application.Services.Notifications;
    using System.Threading.Tasks;

    public interface INotificationManagerService
	{
		Task ProcessNotificationAsync(PushNotificationsType pushType);
	}
}