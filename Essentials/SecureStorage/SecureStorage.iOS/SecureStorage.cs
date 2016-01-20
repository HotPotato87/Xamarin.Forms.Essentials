using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Foundation;
using Security;
using Xamarin.Forms.Essentials.SecureStorage.Interfaces;

namespace Xamarin.Forms.Essentials.SecureStorage.iOS
{
    /// <summary>
    /// Component taken from Xlabs.Forms
    /// 
    /// Implements <see cref="ISecureStorage"/> for iOS using <see cref="SecKeyChain"/>.
    /// </summary>
    public class SecureStorage : ISecureStorage
    {
        #region ISecureStorage Members

        /// <summary>
        /// Stores data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <param name="dataBytes">Data bytes to store.</param>
        public void Store(string key, byte[] dataBytes)
        {
            var resultCode = SecKeyChain.Add(GetKeyRecord(key, NSData.FromArray(dataBytes)));
            if (resultCode == SecStatusCode.Success) return;

            throw new Exception($"Failed to store data for key {key}. Result code: {resultCode}");
        }

        /// <summary>
        /// Retrieves stored data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <returns>Byte array of stored data.</returns>
        public byte[] Retrieve(string key)
        {
            var existingRecord = GetKeyRecord(key);

            SecStatusCode resultCode;
            var record = SecKeyChain.QueryAsRecord(existingRecord, out resultCode);

            CheckError(resultCode);

            return record.ValueData.ToArray();
        }

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="key">Key for the data to be deleted.</param>
        public void Delete(string key)
        {
            CheckError(SecKeyChain.Remove(GetKeyRecord(key)));
        }

        #endregion

        #region private static methods
        private static void CheckError(SecStatusCode resultCode, [CallerMemberName] string caller = null)
        {
            if (resultCode != SecStatusCode.Success)
            {
                throw new Exception($"Failed to execute {caller}. Result code: {resultCode}");
            }
        }

        private static SecRecord GetKeyRecord(string key, NSData data = null)
        {
            var record = new SecRecord(SecKind.GenericPassword)
            {
                Service = NSBundle.MainBundle.BundleIdentifier,
                Account = key
            };

            if (data != null) record.ValueData = data;

            return record;
        }
        #endregion
    }
}
