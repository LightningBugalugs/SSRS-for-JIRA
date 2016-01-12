using System;
using System.Text;

namespace SSRS_for_JIRA.WebServices.Helpers
{
    /// <summary>
    /// <para>Helper for making connection to JIRA REST API</para>
    /// </summary>
    public class ConnectionHelper
    {
        /// <summary>
        /// <para>Given a username and password create the formatted credentials for Basic Authentication to JIRA REST API</para>
        /// </summary>
        /// <param name="username">Username to use for Basic Authentication to JIRA REST API</param>
        /// <param name="password">Password to use for Basic Authentication to JIRA REST API</param>
        /// <returns>
        /// <para>Returns Base64Encoded Credentails for Basic Authentication to JIRA REST API</para>
        /// </returns>
        public static string GetJiraCredentials(string username, string password)
        {
            var credentials = username + ":" + password;
            var byteArrayCredentials = Encoding.UTF8.GetBytes(credentials);
            var base64Credentials = Convert.ToBase64String(byteArrayCredentials);

            return base64Credentials;
        }
    }
}