using Ridder.Client.SDK;

namespace MESv1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            RidderIQSDK application = new RidderIQSDK();
            ISDKResult loginResult = application.Login("Username", "Password", "Company");
        }
    }
}
