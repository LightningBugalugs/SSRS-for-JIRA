using System.Xml;

namespace SSRS_for_JIRA.WebServices.Helpers
{
    /// <summary>
    /// <para>Helper for JIRA REST API calls</para>
    /// </summary>
    public class JiraRestApiHelper
    {
        /// <summary>
        /// <para>Gets the result of a requested JIRA REST API call and formats it as XML</para>
        /// </summary>
        /// <param name="username">Username to use for Basic Authentication to JIRA REST API</param>
        /// <param name="password">Password to use for Basic Authentication to JIRA REST API</param>
        /// <param name="url">JIRA REST API call (as url)</param>
        /// <returns>
        /// <para>Returns an XmlDocumentation representation of the JSON returned from the JIRA REST API call</para>
        /// </returns>
        public static XmlDocument GetXmlJiraApiResponse(string username, string password, string url)
        {
            var jiraApiResponse = WebResponseHelper.GetWebResponse(username, password, url);
            var xmlJiraApiResponse = JsonToXmlHelper.JsonToXml(jiraApiResponse);
            return xmlJiraApiResponse;
        }
    }
}