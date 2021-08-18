using FirebaseCompleteSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Contracts;
using FirebaseProvider;

namespace FirebaseCompleteSample.Controllers
{
    public class FirebaseNotificationController : Controller
    {
        private readonly ILogger<FirebaseNotificationController> _logger;
        private readonly IFirebaseNotificationService _firebaseNotificationService;

        public FirebaseNotificationController(ILogger<FirebaseNotificationController> logger, IFirebaseNotificationService firebaseNotificationService)
        {
            _logger = logger;
            _firebaseNotificationService = firebaseNotificationService;
        }


        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public IActionResult SaveToken(FirebaseToken firebaseToken)
        {
            var fileName = $"D://fcm-sample/token files/fcm_{firebaseToken.UserName}.txt";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteAsync(JsonSerializer.Serialize(firebaseToken));
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult SubscribeToGroup(SubscribeRequest subscribeRequest)
        {
            _firebaseNotificationService.SubscribeToGroup(subscribeRequest.GroupName, subscribeRequest.Token);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UnsubscribeToGroup(SubscribeRequest unSubscribeRequest)
        {
            _firebaseNotificationService.UnSubscribeToGroup(unSubscribeRequest.GroupName, unSubscribeRequest.Token);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
