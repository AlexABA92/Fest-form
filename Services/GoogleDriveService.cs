//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Drive.v3;
//using Google.Apis.Services;
//using Google.Apis.Util.Store;

namespace Fest_form.Services
{
    //public class GoogleDriveService
    //{
    //    private static readonly string[] Scopes = { DriveService.Scope.DriveFile };
    //    private static readonly string AplicationName = "Fest-formFail";

    //    public static DriveService GetDriveService() {
    //        UserCredential userCredential;
    //        using (var stream = new FileStream("wwwroot/client_secret.json", FileMode.Open, FileAccess.Read))
    //        {
    //            string credPath = "token.json";
    //            userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
    //                   GoogleClientSecrets.Load(stream).Secrets,
    //                   Scopes,
    //                   "user",
    //                   CancellationToken.None,
    //                   new FileDataStore(credPath, true)).Result;
                    
    //        }
    //        var service = new DriveService(new BaseClientService.Initializer() {
    //            HttpClientInitializer = userCredential,
    //            ApplicationName = AplicationName
    //        });
    //        return service;
    //    }
    //}
}
