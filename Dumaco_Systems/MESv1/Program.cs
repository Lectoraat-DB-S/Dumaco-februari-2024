using ADODB;
using Ridder.Client.SDK;
using Ridder.Client.SDK.SDKDataAcces;

namespace MESv1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            RidderIQSDK application = new RidderIQSDK();
            ISDKResult loginResult = application.Login("Username", "Password", "Company");
            SDKRecordset recordset = application.CreateRecordset("test", "test", "test", "test");
            recordset.MoveFirst();
            Field field = recordset.GetField("Code");
        }
    }
}
