using System;
using System.Security;

namespace AutoRip2MKV
{
    public interface ICredentialManager
    {
        void StoreCredential(string key, string username, SecureString password);
        void StoreCredential(string key, string username, string password);
        Credential GetCredential(string key);
        bool HasCredential(string key);
        void DeleteCredential(string key);
        void MigrateFromPlainText();
    }

    public class Credential
    {
        public string Username { get; set; }
        public SecureString Password { get; set; }
        public string PasswordPlainText 
        { 
            get 
            {
                if (Password == null) return string.Empty;
                
                IntPtr valuePtr = IntPtr.Zero;
                try
                {
                    valuePtr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(Password);
                    return System.Runtime.InteropServices.Marshal.PtrToStringUni(valuePtr);
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
                }
            }
        }

        public void Dispose()
        {
            Password?.Dispose();
        }
    }
}
