using System.Collections.Generic;
using Contracts;

namespace FirebaseProvider
{
    public interface IFirebaseNotificationService
    {
        List<NotificationResponse> SendNotification(FirebaseNotificationPayload payload, string[] tokens);
        string SendNotification(FirebaseNotificationPayload payload, string tokens);
        string SendNotificationToGroup(FirebaseNotificationPayload payload, string groupName);
        void SubscribeToGroup(string groupName, params string[] token);
        void UnSubscribeToGroup(string groupName, params string[] token);

    }
}