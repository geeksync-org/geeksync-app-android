
using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Service.Notification;
using GeekSyncClient.Client;
using GeekSyncClient;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using GeekSyncClient.Config;
using Newtonsoft.Json;

namespace geeksync_app_android
{
    [Service(Label = "GSNotificationService", Permission = "android.permission.BIND_NOTIFICATION_LISTENER_SERVICE")]
    [IntentFilter(new[] { "android.service.notification.NotificationListenerService" })]
    public class GSNotificationService : NotificationListenerService
    {
        List<SenderClient> senders = new List<SenderClient>();
        ConfigManager confMan;

        public override void OnCreate()
        {
            base.OnCreate();
            confMan = new ConfigManager(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "geeksync.config"));

            //foreach (Peer p in confMan.Config.Peers.Where(x => x.PeerID != confMan.Config.MyID))

            foreach (Peer p in confMan.Config.Peers)
            {
                senders.Add(new SenderClient(confMan, p.ChannelID, "https://gs.jk-ovh.kruza.pl/"));
            }
        }

        public override void OnNotificationPosted(StatusBarNotification sbn)
        {

            foreach (SenderClient c in senders)
            {
                //if (c.IsAvailable) t++; else f++;
                if (sbn.Notification.TickerText!=null)
                
                c.SendMessage(sbn.Notification.TickerText.ToString());
            }

        }





    }

   
}
