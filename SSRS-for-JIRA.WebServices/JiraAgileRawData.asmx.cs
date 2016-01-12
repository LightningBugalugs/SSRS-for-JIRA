using SSRS_for_JIRA.WebServices.Helpers;
using SSRS_for_JIRA.WebServices.JiraObjects;
using System.Collections.Generic;
using System.Web.Services;
using System.Xml;

namespace SSRS_for_JIRA.WebServices
{
    /// <summary>
    /// <para>Collection of XML Data from JIRA REST API that can be used in reporting services or to expand the report objects.</para>
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class JiraAgileRawData : System.Web.Services.WebService
    {
        /// <summary>
        /// <para>The full XML representation of the JSON response for an issue</para>
        /// </summary>
        /// <param name="issueKey">The identifier for the issue</param>
        /// <returns>
        /// <para>Returns the raw XmlDocument representing the JSON object of the issue.</para>
        /// </returns>
        [WebMethod]
        public XmlDocument GetRawXmlIssue(string issueKey)
        {
            var getRawXmlApiUrl = Properties.Settings.Default.JiraRestApiUrl + "agile/1.0/issue/" + issueKey;
            var xmlJiraApiResponse = JiraRestApiHelper.GetXmlJiraApiResponse(Properties.Settings.Default.JiraUsername, Properties.Settings.Default.JiraPassword, getRawXmlApiUrl);

            return xmlJiraApiResponse;
        }

        /// <summary>
        /// <para>Provides a list of issues given a sprintId</para>
        /// </summary>
        /// <param name="sprintId">The sprint Id to get the issues for</param>
        /// <returns>
        /// <para>Returns the listing of issues for the requested sprint</para>
        /// </returns>
        [WebMethod]
        public List<Issue> GetSprintIssues(int sprintId)
        {
            var getSprintsJiraRestApiUrl = Properties.Settings.Default.JiraRestApiUrl + "agile/1.0/board/" + Properties.Settings.Default.JiraBoardId + "/sprint/" + sprintId + "/issue";
            var xmlJiraApiResponse = JiraRestApiHelper.GetXmlJiraApiResponse(Properties.Settings.Default.JiraUsername, Properties.Settings.Default.JiraPassword, getSprintsJiraRestApiUrl);

            var issues = new List<Issue>();
            foreach (XmlNode xmlNode in xmlJiraApiResponse.GetElementsByTagName("issues"))
            {
                var issueXml = "<Issue>" + xmlNode.InnerXml + "</Issue>";
                var issue = Issue.DeserializeFromXML(issueXml);
                issues.Add(issue);
            }

            return issues;
        }

        /// <summary>
        /// <para>Provides a list of issues given a fixVersion (aka Release name)</para>
        /// </summary>
        /// <param name="fixVersion">The fixVersion (aka Release name) to get the issues for</param>
        /// <returns>
        /// <para>Returns the listing of issues for the requested release</para>
        /// </returns>
        [WebMethod]
        public List<Issue> GetReleaseIssues(string fixVersion)
        {
            var getReleasesJiraRestApiUrl = Properties.Settings.Default.JiraRestApiUrl + "api/2/search?jql=fixVersion = \"" + fixVersion + "\"";
            var xmlJiraApiResponse = JiraRestApiHelper.GetXmlJiraApiResponse(Properties.Settings.Default.JiraUsername, Properties.Settings.Default.JiraPassword, getReleasesJiraRestApiUrl);

            var issues = new List<Issue>();
            foreach (XmlNode xmlNode in xmlJiraApiResponse.GetElementsByTagName("issues"))
            {
                var issueXml = "<Issue>" + xmlNode.InnerXml + "</Issue>";
                var issue = Issue.DeserializeFromXML(issueXml);
                issues.Add(issue);
            }

            return issues;
        }

        /// <summary>
        /// <para>Provides a list of issues given a component name</para>
        /// </summary>
        /// <param name="fixVersion">The component name to get the issues for</param>
        /// <returns>
        /// <para>Returns the listing of issues for the requested component</para>
        /// </returns>
        [WebMethod]
        public List<Issue> GetComponentIssues(string component)
        {
            var getReleasesJiraRestApiUrl = Properties.Settings.Default.JiraRestApiUrl + "api/2/search?jql=component = \"" + component + "\"";
            var xmlJiraApiResponse = JiraRestApiHelper.GetXmlJiraApiResponse(Properties.Settings.Default.JiraUsername, Properties.Settings.Default.JiraPassword, getReleasesJiraRestApiUrl);

            var issues = new List<Issue>();
            foreach (XmlNode xmlNode in xmlJiraApiResponse.GetElementsByTagName("issues"))
            {
                var issueXml = "<Issue>" + xmlNode.InnerXml + "</Issue>";
                var issue = Issue.DeserializeFromXML(issueXml);
                issues.Add(issue);
            }

            return issues;
        }

        /// <summary>
        /// <para>Get the listing of sprints from JIRA</para>
        /// </summary>
        /// <returns>
        /// <para>Returns a list of sprints from JIRA</para>
        /// </returns>
        [WebMethod]
        public List<Sprint> GetSprints()
        {
            var getSprintsJiraRestApiUrl = Properties.Settings.Default.JiraRestApiUrl + "agile/1.0/board/" + Properties.Settings.Default.JiraBoardId + "/sprint";
            var jiraApiResponse = WebResponseHelper.GetWebResponse(Properties.Settings.Default.JiraUsername,
                                                                   Properties.Settings.Default.JiraPassword,
                                                                   getSprintsJiraRestApiUrl);
            var xmlJiraApiResponse = JsonToXmlHelper.JsonToXml(jiraApiResponse);

            var sprints = new List<Sprint>();
            foreach (XmlNode xmlNode in xmlJiraApiResponse.GetElementsByTagName("values"))
            {
                var sprintXml = "<Sprint>" + xmlNode.InnerXml + "</Sprint>";
                var sprint = Sprint.DeserializeFromXml(sprintXml);

                var sprintIssues = this.GetSprintIssues(sprint.Id);
                sprint.SprintIssues = sprintIssues;

                sprints.Add(sprint);
            }

            return sprints;
        }        
    }
}
