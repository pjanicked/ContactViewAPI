namespace ContactViewAPI.App.Helpers.Common
{
    using Microsoft.AspNetCore.Http;

    public static class CommonHelper
    {
        public static void AddAppError(this HttpResponse res, string message)
        {
            res.Headers.Add("Application-Error", message);
            res.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            res.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
