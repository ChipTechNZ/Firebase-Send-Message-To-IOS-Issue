using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace Firebase_Date_Message_Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Please replace this path with the provided key file.
            var credential = GoogleCredential.FromFile(@"C:\privatekeyFile.json");
            FirebaseApp.Create(new AppOptions
            {

                Credential = credential
            });

            var registrationToken = "dJn6zh_0MkQOuzjkzNU-lj:APA91bHviayaEEzSF3KT878UHGoGzeUz-MoIOhUgQBaodlokXJs3bEpDzLUAQobyQZUJ6MxslB1OBwGE9OdEa-vXtIickTHx3aGoBw6sSCOdLg8nWJ-2k1iXyLgfczoRdAKsRDdTyXRZ";

            var message = new Message
            {
                /* Please uncomment this section when you want to turn it into notification message.
                Notification = new Notification
                {
                    Body = "Please let me know if you can receive this",
                    Title = "Ted Test"
                },
                */
                Data = new Dictionary<string, string>
                {
                    {"SerialNumber", "70000016"},
                    {"EventId", "9eb553b4-28c9-4334-bc68-fabeb8330762"},
                    {"Type", "DeviceSetupComplete123"}
                },
                Token = registrationToken,
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                },
                Apns = new ApnsConfig
                {
                    Headers = new Dictionary<string, string>
                    {
                        {"apns-priority", "10"}
                    },
                    Aps = new Aps
                    {
                        ContentAvailable = true
                    }
                }
            };


            Parallel.For(0, 10, async i =>
            {
                try
                {
                    FirebaseMessaging.DefaultInstance.SendAsync(message).GetAwaiter().GetResult();
                }
                catch (FirebaseMessagingException e)
                {
                    var responseMessage = await e.HttpResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to send message at {i} time, the detail message is {responseMessage}");
                }

                Console.WriteLine($"Successfully send message at {i} time");
            });

            Console.WriteLine("Finished");

        }
    }
}
