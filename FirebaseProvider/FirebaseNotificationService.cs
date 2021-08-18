using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace FirebaseProvider
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        FirebaseApp firebaseApp;
        public FirebaseNotificationService()
        {

            firebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
            });
        }
        public List<NotificationResponse> SendNotification(FirebaseNotificationPayload payload, params string[] registrationTokens)
        {

            var message = new MulticastMessage()
            {
                Tokens = registrationTokens,
                Data = payload?.getDictonary(),
            };

            var response = FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).Result;


            if (response.FailureCount > 0)
            {
                var failedTokens = new List<string>();
                for (var i = 0; i < response.Responses.Count; i++)
                {
                    if (!response.Responses[i].IsSuccess)
                    {
                        // The order of responses corresponds to the order of the registration tokens.
                        failedTokens.Add(registrationTokens[i]);
                    }
                }
            }

            return response.Responses.Select(x => new NotificationResponse
            {
                Id = x.MessageId,
                status = x.IsSuccess
            }).ToList();
        }

        public string SendNotification(FirebaseNotificationPayload payload, string registrationToken)
        {

            // See documentation on defining a message payload.
            var message = new Message()
            {
                Data = payload?.getDictonary(),
                Token = registrationToken,
            };

            return FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
        }

        public string SendNotificationToGroup(FirebaseNotificationPayload payload, string topic)
        {
            // See documentation on defining a message payload.
            var message = new Message()
            {
                Data = payload?.getDictonary(),
                Topic = topic,
            };

            // Send a message to the devices subscribed to the provided topic.
            return FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
        }

        public void SubscribeToGroup(string topic, params string[] registrationTokens)
        {


            // Subscribe the devices corresponding to the registration tokens to the
            // topic
            var response = FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(
                registrationTokens, topic).Result;
            // See the TopicManagementResponse reference documentation
            // for the contents of response.
            Console.WriteLine($"{response.SuccessCount} tokens were subscribed successfully");

        }

        public void UnSubscribeToGroup(string topic, params string[] registrationTokens)
        {

            // Unsubscribe the devices corresponding to the registration tokens from the
            // topic
            var response = FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(
                registrationTokens, topic).Result;
            // See the TopicManagementResponse reference documentation
            // for the contents of response.
            Console.WriteLine($"{response.SuccessCount} tokens were unsubscribed successfully");
        }
    }
}