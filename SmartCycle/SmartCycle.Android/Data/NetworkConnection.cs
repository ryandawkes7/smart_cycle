using Android.App;
using Android.Content;
using Android.Net;
using SmartCycle.Data;
using SmartCycle.Droid.Data;


[assembly: Xamarin.Forms.Dependency(typeof(NetworkConnection))]

namespace SmartCycle.Droid.Data
{
    public class NetworkConnection : INetworkConnection
    {
        public bool IsConnected { get; set; }

        public void CheckInternetConnection()
        {
            var ConnectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            var ActiveNetworkInfo = ConnectivityManager.ActiveNetworkInfo;
            if(ActiveNetworkInfo == null && ActiveNetworkInfo.IsConnectedOrConnecting)
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
            }
        }
    }
}