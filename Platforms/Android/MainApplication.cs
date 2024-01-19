using Android.App;
using Android.Runtime;

namespace blitz_chat
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
            Android.Webkit.CookieManager.Instance.RemoveAllCookies(null);
            Android.Webkit.CookieManager.Instance.RemoveSessionCookies(null);
            Android.Webkit.WebStorage.Instance.DeleteAllData();
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}