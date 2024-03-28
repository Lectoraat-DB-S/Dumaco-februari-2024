using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADODB;
using Ridder.Client.SDK;
using Ridder.Client.SDK.SDKDataAcces;

namespace MESv1
{
    public class ERP_Connection
    {
        private const string username = "Administrator";
        private const string password = "perron_ad";
        private const string company = "Perron038";
        private readonly RidderIQSDK application = new RidderIQSDK();

        public bool Connect()
        {
            if (application.LoggedinAndConnected)
            {
                return true;
            }
            ISDKResult loginResult = application.Login(username, password, company);
            if (!loginResult.HasError)
            {
                //connected = true;
            }
            return application.LoggedinAndConnected;
        }
        private void Disconnect()
        {
            if (application.LoggedinAndConnected)
            {
                application.Logout();
            }
        }

        // Get values from table froms specified colum.
        public object[] GetValues(string tableName, string columName, string? filterName = "", string? sortName = "")
        {
            if (!application.LoggedinAndConnected || string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(columName))
            {
                return new object[0];
            }

            SDKRecordset recordSet = application.CreateRecordset(tableName, columName, filterName, sortName);
            int amountOfRecords = recordSet.RecordCount;
            object[] data = new object[amountOfRecords];
            recordSet.MoveFirst();
            int i = 0;
            SDKRowData rowData = recordSet.GetCurrentRow();
            while (!recordSet.EOF && i < amountOfRecords)
            {
                data[i++] = recordSet.GetField(columName).Value;
                recordSet.MoveNext();
            }
            return data;
        }

        // Update first value in table with filter
        public void UpdateValue(string tableName, string fieldName, object value, string filterName)
        {
            if (!application.LoggedinAndConnected || string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(fieldName) || value is null)
            {
                return;
            }

            SDKRecordset recordset = application.CreateRecordset(tableName, "", filterName, "");
            recordset.MoveFirst();
            recordset.SetFieldValue(fieldName, value);
            ISDKResult result = recordset.Update();
            if (result.HasError)
            {
                throw new Exception("Data update failed");
            }
        }

        // Update values in table with the same filter
        public void UpdateValues(string tableName, string fieldName, object value, string filterName)
        {
            if (!application.LoggedinAndConnected || string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(fieldName))
            {
                return;
            }
            SDKRecordset recordset = application.CreateRecordset(tableName, "", filterName, "");
            recordset.MoveFirst();
            while (!recordset.EOF)
            {
                recordset.SetFieldValue(fieldName, value);
                recordset.Update();
                recordset.MoveNext();
            }
        }

        // Add values to table
        public void AddValues(string tableName, string[] fieldNames, object[] values)
        {
            if (!application.LoggedinAndConnected || string.IsNullOrWhiteSpace(tableName) || fieldNames is null || values is null)
            {
                return;
            }

            if (fieldNames.Length == values.Length)
            {
                return;
            }

            SDKRecordset recordset = application.CreateRecordset(tableName, "", "", "");
            recordset.AddNew();
            for (int i = 0; i < fieldNames.Length; i++)
            {
                recordset.SetFieldValue(fieldNames[i], values[i]);
            }
            ISDKResult result = recordset.Update();
            if (result.HasError)
            {
                throw new Exception("Data added failed");
            }
        }

        // Delete first value in table with filter
        public void DeleteValue(string tableName, string filterName)
        {
            if (!application.LoggedinAndConnected || string.IsNullOrWhiteSpace(tableName))
            {
                return;
            }

            SDKRecordset recordset = application.CreateRecordset(tableName, "", filterName, "");
            recordset.MoveFirst();
            recordset.Delete();
        }

        // Delete values in table with the same filter
        public void DeleteValues(string tableName, string filterName)
        {
            if (!application.LoggedinAndConnected || string.IsNullOrWhiteSpace(tableName))
            {
                return;
            }

            SDKRecordset recordset = application.CreateRecordset(tableName, "", filterName, "");
            recordset.MoveFirst();
            while (!recordset.EOF)
            {
                recordset.Delete();
                recordset.MoveNext();
            }
        }
    }
}
