
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.IO;

namespace geeksync_app_android
{
    [Activity(Label = "ConfImpExActivity")]
    public class ConfImpExActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_confimpex);

        //    Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
        //    SetSupportActionBar(toolbar);

            var txt = FindViewById<TextView>(Resource.Id.configText);

            FileInfo fi = new FileInfo(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "geeksync.config"));
            StreamReader sr = fi.OpenText();

            txt.Text = sr.ReadToEnd();
            sr.Close();

            // Create your application here
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_settings, menu);
            return true;
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings_save)
            {
                var txt = FindViewById<TextView>(Resource.Id.configText);

                FileInfo fi = new FileInfo(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "geeksync.config"));
                if (fi.Exists) fi.Delete();
                StreamWriter sw=fi.AppendText();

                sw.WriteLine(txt.Text);
                sw.Close();
                var main = new Intent(this, typeof(MainActivity));
                StartActivity(main);
            }
            if (id == Resource.Id.action_settings_discard)
            {
                var main = new Intent(this, typeof(MainActivity));
                StartActivity(main);
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}
