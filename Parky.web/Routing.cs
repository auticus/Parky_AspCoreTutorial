
namespace Parky.web
{
    public static class Routing
    {
        //public static string APIRoute = "https://localhost:44312/";
        public static string APIRoute = "https://localhost:5001/"; //this should use whatever config you are using (internal IIS or kestrel)
        public static string NationalParkRoute = APIRoute + "api/v1/nationalparks/";
        public static string TrailsRoute = APIRoute + "api/v1/trails/";
        public static string AccountAPIPath = APIRoute + "api/v1/Users/";
    }
}
