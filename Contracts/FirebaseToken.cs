using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public class FirebaseToken
    {
        public string Token { get; set; }
        public string UserName { get; set; }
    }

    public class SubscribeRequest
    {
        public string GroupName { get; set; }
        public string Token { get; set; }
    }

    public class NotificationResponse
    {
        public bool status { get; set; }
        public string Id { get; set; }
    }
    public class FirebaseNotificationPayload
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public string Icon { get; set; } =
            "https://www.gstatic.com/mobilesdk/160503_mobilesdk/logo/2x/firebase_28dp.png";

        public Dictionary<string, string> getDictonary()
        {
            return new Dictionary<string, string>()
            {
                {"icon", Icon},
                {"title", Title},
                {"body", Body},
            };
        }
    }
}
