using System;
using System.IO;
using System.Net;

namespace SSRS_for_JIRA.WebServices.Helpers
{
    /// <summary>
    /// <para>Helper for receiving web responses from url requests</para>
    /// </summary>
    public class WebResponseHelper
    {
        /// <summary>
        /// <para>Helper class to get the web response (as a string) from the JIRA REST API call</para>
        /// </summary>
        /// <param name="username">Username to use for Basic Authentication to JIRA REST API</param>
        /// <param name="password">Password to use for Basic Authentication to JIRA REST API</param>
        /// <param name="url">The JIRA REST API call as a url</param>
        /// <returns>
        /// <para>Returns the web response (JSON API response) as a string</para>
        /// </returns>
        public static string GetWebResponse(string username, string password, string url)
        {
            var responseText = String.Empty;
            // add maxResults setting in the url provided
            url = url.Contains("?") ? url.Replace("?", "?maxResults=1250&") : url + "?maxResults=1250";

            var httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            var base64Credentials = ConnectionHelper.GetJiraCredentials(username, password);
            httpWebRequest.Headers.Add("Authorization", "Basic " + base64Credentials);

            var httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                responseText = streamReader.ReadToEnd();
            }

            return responseText;
        }
    }
}