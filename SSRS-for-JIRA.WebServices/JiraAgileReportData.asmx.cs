using System.Collections.Generic;
using System.Web.Services;
using System.Xml;
using SSRS_for_JIRA.WebServices.Helpers;
using SSRS_for_JIRA.WebServices.JiraObjects;
using SSRS_for_JIRA.WebServices.ReportObjects;
using System.Linq;
using System;

namespace SSRS_for_JIRA.WebServices
{
    /// <summary>
    /// <para>Collection of Customised Reporting Objects from JIRA Agile for use with Sql Server Reporting Services (SSRS)</para>
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class JiraAgileReportData : System.Web.Services.WebService
    {
        // JIRA REST API Documentation:         https://docs.atlassian.com/jira/REST/latest/
        // JIRA Agile REST API Documentation:   https://docs.atlassian.com/greenhopper/REST/cloud/
        
        /// <summary>
        /// <para>Get Report Data by Component for use in Velocity Reporting Charts (basically one line per sprint per component)</para>
        /// </summary>
        /// <returns>
        /// <para>Returns a list of Component Report line items (basically one line per sprint per component)</para>
        /// </returns>
        [WebMethod]
        public List<ComponentReport> GetComponentReportData()
        {
            var jiraAgileRawData = new JiraAgileRawData();
            var componentReports = new List<ComponentReport>();

            // get all sprint data
            var sprints = jiraAgileRawData.GetSprints();

            // get components to process
            var components = Properties.Settings.Default.JiraComponentList.Split(';');

            // create a component report for each component/sprint
            foreach (var component in components)
            {
                // for each sprint get listing of issues
                foreach (var sprint in sprints)
                {
                    var componentReport = new ComponentReport(component, sprint.Id, sprint.Name, sprints);
                    componentReports.Add(componentReport);
                }
            }

            return componentReports;
        }

        /// <summary>
        /// <para>Get a summary of velocities and data for all components</para>
        /// </summary>
        /// <returns>
        /// <para>List of component summaries (one per component)</para>
        /// </returns>
        [WebMethod]
        public List<ComponentReportSummary> GetComponentReportSummaries()
        {
            var componentReportSummaries = new List<ComponentReportSummary>();

            var components = Properties.Settings.Default.JiraComponentList.Split(';');
            var componentReportData = this.GetComponentReportData();

            foreach (var component in components)
            {
                var jiraAgileRawData = new JiraAgileRawData();
                var totalStories = jiraAgileRawData.GetComponentIssues(component);
                var componentReportSummary = new ComponentReportSummary(component, componentReportData, totalStories);
                componentReportSummaries.Add(componentReportSummary);
            }

            return componentReportSummaries;
        }

        /// <summary>
        /// <para>Get lising of release progress (one item per release per progress step)</para>
        /// </summary>
        /// <returns>
        /// <para>Returns a list of release status report line items (one item per release per progress step)</para>
        /// </returns>
        [WebMethod]
        public List<ReleaseStatusReportItem> GetReleaseProgressReportData()
        {
            var getReleasesJiraRestApiUrl = Properties.Settings.Default.JiraRestApiUrl + "agile/1.0/board/" + Properties.Settings.Default.JiraBoardId + "/version";
            var xmlJiraApiResponse = JiraRestApiHelper.GetXmlJiraApiResponse(Properties.Settings.Default.JiraUsername, Properties.Settings.Default.JiraPassword, getReleasesJiraRestApiUrl);
            var jiraAgileRawData = new JiraAgileRawData();

            var releaseStatusReport = new List<ReleaseStatusReportItem>();
            foreach (XmlNode xmlNode in xmlJiraApiResponse.GetElementsByTagName("values"))
            {
                var releaseXml = "<Release>" + xmlNode.InnerXml + "</Release>";
                var release = Release.DeserializeFromXml(releaseXml);

                var releaseIssues = jiraAgileRawData.GetReleaseIssues(release.Name);
                release.ReleaseIssues = releaseIssues;
                var averageStoryPointAmount = 1.0m;
                var averageStoryPointAmountCalc = (from ri in releaseIssues
                                                   where ri.IssueFields.StoryPoints != String.Empty
                                                   select Convert.ToDecimal(ri.IssueFields.StoryPoints)).ToList();
                if (averageStoryPointAmountCalc.Count > 0)
                {
                    averageStoryPointAmount = (from ri in averageStoryPointAmountCalc select ri).Average();
                    averageStoryPointAmount = Math.Round(averageStoryPointAmount, 3);
                }

                foreach (var releaseIssue in releaseIssues)
                {
                    var releaseStatusReportItem = new ReleaseStatusReportItem() { IssueStatus = releaseIssue.IssueFields.Status.StatusName, 
                                                                                  ReleaseName = release.Name, 
                                                                                  ReleaseDate = release.TargetReleaseDate,
                                                                                  StoryPoints = releaseIssue.IssueFields.StoryPoints };
                    if (releaseStatusReportItem.IssueStatus.ToLowerInvariant() == "proposed story")
                    {
                        releaseStatusReportItem.StoryPoints = averageStoryPointAmount.ToString();
                    }
                    releaseStatusReportItem.IssueStatusOrder = releaseStatusReportItem.IssueStatusOrderId;
                    releaseStatusReport.Add(releaseStatusReportItem);
                }
            }

            releaseStatusReport = releaseStatusReport.OrderByDescending(rsr => rsr.ReleaseName).ToList();

            return releaseStatusReport;
        }

        /// <summary>
        /// <para>Get listing of release status report (one item per release)</para>
        /// </summary>
        /// <returns>
        /// <para>Returns a list of releas status report items (one item per release)</para>
        /// </returns>
        [WebMethod]
        public List<ReleaseStatusReportSummary> GetReleaseStatusReportSummaries()
        {
            var jiraAgileRawData = new JiraAgileRawData();

            // get component data
            var componentReportSummaries = new List<ComponentReportSummary>();
            var components = Properties.Settings.Default.JiraComponentList.Split(';');
            var componentReportData = this.GetComponentReportData();
            foreach (var component in components)
            {
                jiraAgileRawData = new JiraAgileRawData();
                var totalStories = jiraAgileRawData.GetComponentIssues(component);
                var componentReportSummary = new ComponentReportSummary(component, componentReportData, totalStories);
                componentReportSummaries.Add(componentReportSummary);
            }

            // get release data
            var getReleasesJiraRestApiUrl = Properties.Settings.Default.JiraRestApiUrl + "agile/1.0/board/" + Properties.Settings.Default.JiraBoardId + "/version";
            var xmlJiraApiResponse = JiraRestApiHelper.GetXmlJiraApiResponse(Properties.Settings.Default.JiraUsername, Properties.Settings.Default.JiraPassword, getReleasesJiraRestApiUrl);
            jiraAgileRawData = new JiraAgileRawData();
            var releaseStatusReport = new List<ReleaseStatusReportItem>();
            foreach (XmlNode xmlNode in xmlJiraApiResponse.GetElementsByTagName("values"))
            {
                var releaseXml = "<Release>" + xmlNode.InnerXml + "</Release>";
                var release = Release.DeserializeFromXml(releaseXml);

                var releaseIssues = jiraAgileRawData.GetReleaseIssues(release.Name);
                release.ReleaseIssues = releaseIssues;
                var averageStoryPointAmount = 1.0m;
                var averageStoryPointAmountCalc = (from ri in releaseIssues
                                                   where ri.IssueFields.StoryPoints != String.Empty
                                                   select Convert.ToDecimal(ri.IssueFields.StoryPoints)).ToList();
                if (averageStoryPointAmountCalc.Count > 0)
                {
                    averageStoryPointAmount = (from ri in averageStoryPointAmountCalc select ri).Average();
                    averageStoryPointAmount = Math.Round(averageStoryPointAmount, 3);
                }
                
                foreach (var releaseIssue in releaseIssues)
                {
                    var releaseStatusReportItem = new ReleaseStatusReportItem()
                    {
                        IssueStatus = releaseIssue.IssueFields.Status.StatusName,
                        ReleaseName = release.Name,
                        ReleaseDate = release.TargetReleaseDate,
                        StoryPoints = releaseIssue.IssueFields.StoryPoints
                    };
                    if (releaseStatusReportItem.IssueStatus.ToLowerInvariant() == "proposed story")
                    {
                        releaseStatusReportItem.StoryPoints = averageStoryPointAmount.ToString();
                    }
                    releaseStatusReportItem.IssueStatusOrder = releaseStatusReportItem.IssueStatusOrderId;
                    releaseStatusReport.Add(releaseStatusReportItem);
                }
            }
            releaseStatusReport = releaseStatusReport.OrderByDescending(rsr => rsr.ReleaseName).ToList();

            // combine component and release data for release status report summary
            var overallVelocity = (from crs in componentReportSummaries
                                   select crs.VelocityForComponentFourSprintCalc).Sum() / components.Count();
            var releaseStatusReportSummaries = new List<ReleaseStatusReportSummary>();
            var releases = (from rsri in releaseStatusReport
                            select rsri.ReleaseName).Distinct().ToList();
            var remainingStatuses = new List<string>() { "waiting for support", "on hold", "proposed story", "open", "in progress", "in testing" };
            var completedStatuses = new List<string>() { "verified", "resolved" };
            foreach (var release in releases)
            {
                var releaseStatusReportSummary = new ReleaseStatusReportSummary();
                releaseStatusReportSummary.ReleaseName = release;
                releaseStatusReportSummary.StoryPointsTotal = (from rsri in releaseStatusReport
                                                               where rsri.ReleaseName == release
                                                                     && !String.IsNullOrEmpty(rsri.StoryPoints)
                                                               select Convert.ToDecimal(rsri.StoryPoints)).Sum().ToString();
                releaseStatusReportSummary.StoryPointsRemaining = (from rsri in releaseStatusReport
                                                                   where rsri.ReleaseName == release
                                                                         && !String.IsNullOrEmpty(rsri.StoryPoints)
                                                                         && remainingStatuses.Contains(rsri.IssueStatus.ToLowerInvariant())
                                                                   select Convert.ToDecimal(rsri.StoryPoints)).Sum().ToString();
                releaseStatusReportSummary.StoryPointsCompleted = (from rsri in releaseStatusReport
                                                                   where rsri.ReleaseName == release
                                                                         && !String.IsNullOrEmpty(rsri.StoryPoints)
                                                                         && completedStatuses.Contains(rsri.IssueStatus.ToLowerInvariant())
                                                                   select Convert.ToDecimal(rsri.StoryPoints)).Sum().ToString();
                releaseStatusReportSummary.PlannedCompletion = (from rsri in releaseStatusReport
                                                                where rsri.ReleaseName == release
                                                                select rsri.ReleaseDate).FirstOrDefault();

                var remainingSprints = Convert.ToDecimal(releaseStatusReportSummary.StoryPointsRemaining) / overallVelocity;
                var remainingDays = Convert.ToInt32(remainingSprints * 14.0m);
                releaseStatusReportSummary.EstimatedCompletion = DateTime.Now.AddDays(remainingDays);
                
                var completionSprintDifferenceInDays = (releaseStatusReportSummary.PlannedCompletion - releaseStatusReportSummary.EstimatedCompletion).TotalDays;
                var completionSprintDifferenceInSprints = completionSprintDifferenceInDays/14.0d;
                releaseStatusReportSummary.CompletionSprintDifference = Convert.ToInt32(completionSprintDifferenceInSprints);
                if (Convert.ToDecimal(releaseStatusReportSummary.StoryPointsRemaining) == 0m) { releaseStatusReportSummary.CompletionSprintDifference = 0; }

                releaseStatusReportSummaries.Add(releaseStatusReportSummary);
            }

            return releaseStatusReportSummaries;
        }
    }
}
