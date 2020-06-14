using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GeekSyncClient.Client;
using GeekSyncClient;
using System.Linq;
using System.Collections.Generic;
using GeekSyncClient.Config;
using System.IO;
using Android.Content;

namespace geeksync_app_android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        List<SenderClient> senders = new List<SenderClient>();
        ConfigManager confMan;
        //ReceiverClient rc;

        int cnt = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

           // StartService(new Intent(this, typeof(GSNotificationService)));

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            confMan = new ConfigManager(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "geeksync.config"));

            //foreach(Peer p in confMan.Config.Peers.Where(x=>x.PeerID!=confMan.Config.MyID))
            
            foreach (Peer p in confMan.Config.Peers)
            {
                senders.Add(new SenderClient(confMan,p.ChannelID, "https://gs.jk-ovh.kruza.pl/"));
            }

         //   rc = new ReceiverClient(confMan, confMan.Config.Peers.Single(x=>x.PeerID==confMan.Config.MyID).ChannelID, "https://gs.jk-ovh.kruza.pl/");
         //   rc.MessageReceived = HandleReceivedMessage;
         //   rc.Connect();

            TextView txt = FindViewById<TextView>(Resource.Id.txt);

            FileInfo fi = new FileInfo(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "geeksync.config"));
            StreamReader sr = fi.OpenText();

            txt.Text = sr.ReadToEnd();
            sr.Close();
        }
        /*
        void HandleReceivedMessage(string msg)
        {
            TextView txt = FindViewById<TextView>(Resource.Id.txt);
            txt.Text = msg;
        }
        */
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                var impex = new Intent(this, typeof(ConfImpExActivity));
                StartActivity(impex);
            }
            if (id == Resource.Id.action_home)
            {
                var main = new Intent(this, typeof(MainActivity));
                StartActivity(main);
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
          //  int f = 0;
          //  int t = 0;
            string msg = "Sending status:" + System.Environment.NewLine;
            foreach(SenderClient c in senders)
            {
                //if (c.IsAvailable) t++; else f++;
                msg += c.ChannelID.ToString() + ": " + c.IsAvailable.ToString() + System.Environment.NewLine;
                c.SendMessage("Hello from Android - #"+cnt.ToString());                
            }
            msg += "END";
            cnt++;
            TextView txt = FindViewById<TextView>(Resource.Id.txt);
            txt.Text = msg;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
