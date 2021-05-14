using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

namespace PlushkinForms.Droid
{
    [Activity(Label = "PlushkinForms", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    [IntentFilter(new[] { "android.intent.action.SEND" }, Categories = new[] { "android.intent.category.DEFAULT" }, DataMimeTypes = new[] { "text/plain" })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Plugin.InputKit.Platforms.Droid.Config.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());

            if (Intent.Action == "android.intent.action.SEND")
            {
                var url = Intent.Extras.GetString("android.intent.extra.TEXT");
                Xamarin.Forms.MessagingCenter.Send<object, string[]>(this, "AddItem", new string[] { url, url });
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}