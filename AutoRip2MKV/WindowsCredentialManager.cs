using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using AutoRip2MKV.Properties;

namespace AutoRip2MKV
{
    public class WindowsCredentialManager : ICredentialManager
    {
        private readonly ILogger _logger;
        private const string CREDENTIAL_PREFIX = "AutoRip2MKV_";

        public WindowsCredentialManager(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void StoreCredential(string key, string username, SecureString password)
        {
            try
            {
                _logger.Debug("Storing credential for key: {0}", key);
                
                var credentialName = CREDENTIAL_PREFIX + key;
                var passwordPtr = IntPtr.Zero;
                
                try
                {
                    passwordPtr = Marshal.SecureStringToGlobalAllocUnicode(password);
                    var passwordBytes = Encoding.Unicode.GetBytes(Marshal.PtrToStringUni(passwordPtr));
                    
                    var credential = new CREDENTIAL
                    {
                        AttributeCount = 0,
                        Attributes = IntPtr.Zero,
                        Comment = null,
                        TargetAlias = null,
                        Type = CRED_TYPE.GENERIC,
                        Persist = CRED_PERSIST.LOCAL_MACHINE,
                        CredentialBlobSize = (uint)passwordBytes.Length,
                        TargetName = credentialName,
                        CredentialBlob = Marshal.AllocHGlobal(passwordBytes.Length),
                        UserName = username
                    };

                    Marshal.Copy(passwordBytes, 0, credential.CredentialBlob, passwordBytes.Length);

                    if (!CredWrite(ref credential, 0))
                    {
                        var error = Marshal.GetLastWin32Error();
                        throw new CredentialException($"Failed to store credential for key '{key}': Win32 Error {error}", key, new Win32Exception(error));
                    }

                    Marshal.FreeHGlobal(credential.CredentialBlob);
                    _logger.Info("Successfully stored credential for key: {0}", key);
                }
                finally
                {
                    if (passwordPtr != IntPtr.Zero)
                    {
                        Marshal.ZeroFreeGlobalAllocUnicode(passwordPtr);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to store credential for key: {0}", key);
                throw;
            }
        }

        public void StoreCredential(string key, string username, string password)
        {
            var securePassword = new SecureString();
            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }
            securePassword.MakeReadOnly();
            
            try
            {
                StoreCredential(key, username, securePassword);
            }
            finally
            {
                securePassword.Dispose();
            }
        }

        public Credential GetCredential(string key)
        {
            try
            {
                _logger.Debug("Retrieving credential for key: {0}", key);
                
                var credentialName = CREDENTIAL_PREFIX + key;
                IntPtr credPtr = IntPtr.Zero;

                if (!CredRead(credentialName, CRED_TYPE.GENERIC, 0, out credPtr))
                {
                    var error = Marshal.GetLastWin32Error();
                    if (error == ERROR_NOT_FOUND)
                    {
                        _logger.Debug("Credential not found for key: {0}", key);
                        return null;
                    }
                    throw new Win32Exception(error, $"Failed to retrieve credential: {error}");
                }

                try
                {
                    var cred = Marshal.PtrToStructure<CREDENTIAL>(credPtr);
                    var passwordBytes = new byte[cred.CredentialBlobSize];
                    Marshal.Copy(cred.CredentialBlob, passwordBytes, 0, (int)cred.CredentialBlobSize);
                    
                    var passwordString = Encoding.Unicode.GetString(passwordBytes);
                    var securePassword = new SecureString();
                    foreach (char c in passwordString)
                    {
                        securePassword.AppendChar(c);
                    }
                    securePassword.MakeReadOnly();

                    _logger.Debug("Successfully retrieved credential for key: {0}", key);
                    return new Credential
                    {
                        Username = cred.UserName,
                        Password = securePassword
                    };
                }
                finally
                {
                    CredFree(credPtr);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to retrieve credential for key: {0}", key);
                throw;
            }
        }

        public bool HasCredential(string key)
        {
            try
            {
                var credential = GetCredential(key);
                var exists = credential != null;
                credential?.Dispose();
                return exists;
            }
            catch
            {
                return false;
            }
        }

        public void DeleteCredential(string key)
        {
            try
            {
                _logger.Debug("Deleting credential for key: {0}", key);
                
                var credentialName = CREDENTIAL_PREFIX + key;
                
                if (!CredDelete(credentialName, CRED_TYPE.GENERIC, 0))
                {
                    var error = Marshal.GetLastWin32Error();
                    if (error != ERROR_NOT_FOUND)
                    {
                        throw new Win32Exception(error, $"Failed to delete credential: {error}");
                    }
                }
                
                _logger.Info("Successfully deleted credential for key: {0}", key);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to delete credential for key: {0}", key);
                throw;
            }
        }

        public void MigrateFromPlainText()
        {
            try
            {
                _logger.Info("Starting migration from plain text credentials");
                
                // Migrate SMTP credentials
                var smtpUser = Settings.Default.SMTPUser;
                var smtpPass = Settings.Default.SMTPPass;
                
                if (!string.IsNullOrWhiteSpace(smtpUser) && !string.IsNullOrWhiteSpace(smtpPass))
                {
                    if (!HasCredential("SMTP"))
                    {
                        StoreCredential("SMTP", smtpUser, smtpPass);
                        _logger.Info("Migrated SMTP credentials to secure storage");
                        
                        // Clear plain text storage
                        Settings.Default.SMTPUser = string.Empty;
                        Settings.Default.SMTPPass = string.Empty;
                        Settings.Default.Save();
                    }
                }
                
                _logger.Info("Completed migration from plain text credentials");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to migrate from plain text credentials");
                throw;
            }
        }

        #region Windows API Declarations

        private const int ERROR_NOT_FOUND = 1168;

        [DllImport("Advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredRead(string target, CRED_TYPE type, int reservedFlag, out IntPtr credentialPtr);

        [DllImport("Advapi32.dll", EntryPoint = "CredWriteW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredWrite([In] ref CREDENTIAL userCredential, [In] uint flags);

        [DllImport("Advapi32.dll", EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CredDelete(string target, CRED_TYPE type, int reservedFlag);

        [DllImport("Advapi32.dll", EntryPoint = "CredFree", SetLastError = true)]
        private static extern bool CredFree([In] IntPtr cred);

        private enum CRED_TYPE : uint
        {
            GENERIC = 1,
            DOMAIN_PASSWORD = 2,
            DOMAIN_CERTIFICATE = 3,
            DOMAIN_VISIBLE_PASSWORD = 4,
            GENERIC_CERTIFICATE = 5,
            DOMAIN_EXTENDED = 6,
            MAXIMUM = 7,
            MAXIMUM_EX = MAXIMUM + 1000,
        }

        private enum CRED_PERSIST : uint
        {
            SESSION = 1,
            LOCAL_MACHINE = 2,
            ENTERPRISE = 3,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CREDENTIAL
        {
            public uint Flags;
            public CRED_TYPE Type;
            public IntPtr TargetName;
            public IntPtr Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public uint CredentialBlobSize;
            public IntPtr CredentialBlob;
            public CRED_PERSIST Persist;
            public uint AttributeCount;
            public IntPtr Attributes;
            public IntPtr TargetAlias;
            public IntPtr UserName;
        }

        #endregion
    }
}
