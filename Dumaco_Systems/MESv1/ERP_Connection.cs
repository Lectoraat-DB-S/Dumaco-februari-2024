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
        private bool connected = false;
        private RidderIQSDK application;

        public bool Connect()
        {
            application = new RidderIQSDK();
            ISDKResult loginResult = application.Login(username, password, company);
            if (!loginResult.HasError)
            {
                connected = true;
            }
            return connected;
        }

        // Get values from table. Problems: fieldName and columnName are the same if only one column is selected, otherwise this is a bad implementation
        public object[] GetValues(string tableName, string fieldName, string? columName = "", string? filterName = "", string? sortName = "")
        {
            if (!connected || string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(fieldName))
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
                data[i++] = recordSet.GetField(fieldName).Value;
                recordSet.MoveNext();
            }
            return data;
        }

        // Update first value in table with filter
        public void UpdateValue(string tableName, string fieldName, object value, string filterName)
        {
            if (!connected || string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(fieldName) || value is null)
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
            if (!connected || string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(fieldName))
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
            if (!connected || string.IsNullOrWhiteSpace(tableName) || fieldNames is null || values is null)
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
            if (!connected || string.IsNullOrWhiteSpace(tableName))
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
            if (!connected || string.IsNullOrWhiteSpace(tableName))
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

        private void Disconnect()
        {
            if (connected)
            {
                application.Logout();
                connected = false;
            }
        }
    }
}
