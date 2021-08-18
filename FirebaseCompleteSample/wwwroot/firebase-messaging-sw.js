
importScripts('https://www.gstatic.com/firebasejs/8.7.1/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/8.7.1/firebase-messaging.js');
firebase.initializeApp({
    apiKey: "AIzaSyB6w7pOhyI3z22EiNFwsovxEQg18eo2x-g",
    projectId: "da-test-31c8b",
    messagingSenderId: "65550940234",
    appId: "1:65550940234:web:4049806afca88907feceb3",
    measurementId: "G-TKMVCK5F3B"
});

// Retrieve an instance of Firebase Messaging so that it can handle background
// messages.
const messaging = firebase.messaging();


// If you would like to customize notifications that are received in the
// background (Web app is closed or not in browser focus) then you should
// implement this optional method.
// Keep in mind that FCM will still show notification messages automatically 
// and you should use data messages for custom notifications.
// For more info see: 
// https://firebase.google.com/docs/cloud-messaging/concept-options



messaging.onBackgroundMessage(function (payload) {
    console.log('[firebase-messaging-sw.js] Received background message ', payload);
    // Customize notification here
    const notificationTitle = payload.data.title;
    const notificationOptions = {
        body: payload.data.body,
        icon: payload.data.icon
    };

    self.registration.showNotification(notificationTitle,
        notificationOptions);
});