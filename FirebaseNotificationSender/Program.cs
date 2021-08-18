using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Contracts;
using FirebaseProvider;

namespace FirebaseNotificationSender
{
    class Program
    {
        private static IFirebaseNotificationService _firebaseNotificationService;
        static void Main(string[] args)
        {
            _firebaseNotificationService = new FirebaseNotificationService();
            UserAction userAction;
            do
            {
                userAction = GetUserInput();
                switch (userAction)
                {
                    case UserAction.SendNotification:
                        SendNotificationIndividually();
                        break;
                    case UserAction.SendNotificationToGroup:
                        SendNotificationToGroup();
                        break;
                    case UserAction.SendBatchNotification:
                        SendBatchNotification();
                        break;
                }

            } while (userAction != UserAction.Stop);




        }

        static UserAction GetUserInput()
        {
            var list = Enum.GetNames(typeof(UserAction));
            foreach (string name in list)
            {
                Console.WriteLine($"for {name}-> press {list.ToList().IndexOf(name)}");
            }
            return Enum.Parse<UserAction>(Console.ReadLine() ?? UserAction.Stop.ToString());
        }
        static FirebaseNotificationPayload GetNotifcationDetail()
        {
            return new FirebaseNotificationPayload
            {
                Body = Console.ReadLine(),
                Title = Console.ReadLine()
            };
        }
        static void SendNotificationToGroup()
        {
            var d = GetNotifcationDetail();
            Console.WriteLine("enter Group name");
            _firebaseNotificationService.SendNotificationToGroup(d, Console.ReadLine());
        }
        static void SendNotificationIndividually()
        {
            var folder = $"D://fcm-sample/token files";

            var files = Directory.GetFiles(folder);
            var d = GetNotifcationDetail();
            foreach (var file in files)
            {
                FirebaseToken firebaseToken;
                using (StreamReader sr = new StreamReader(file))
                {
                    firebaseToken = System.Text.Json.JsonSerializer.Deserialize<FirebaseToken>(sr.ReadToEnd());
                }
                _firebaseNotificationService.SendNotification(d, firebaseToken.Token);
            }
        }

        static void SendBatchNotification()
        {

            Console.WriteLine("Enter Batch size between 1 to 500");
            int pageSize = Convert.ToInt32(Console.ReadLine());
            var folder = $"D://fcm-sample/token files";

            var files = Directory.GetFiles(folder);
            var d = GetNotifcationDetail();
            for (int page = 1; page <= (int)Math.Ceiling((double)files.Length / pageSize); page++)
            {
                List<string> tokens = new List<string>();
                foreach (var file in files.Skip((page - 1) * pageSize).Take(pageSize))
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        tokens.Add(System.Text.Json.JsonSerializer.Deserialize<FirebaseToken>(sr.ReadToEnd()).Token);
                    }
                }
                _firebaseNotificationService.SendNotification(d, tokens.ToArray());

            }
        }
    }


    internal enum UserAction
    {
        SendNotification,
        SendNotificationToGroup,
        SendBatchNotification,
        Stop
    }
}
