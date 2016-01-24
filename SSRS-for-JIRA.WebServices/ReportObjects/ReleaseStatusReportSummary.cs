using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSRS_for_JIRA.WebServices.ReportObjects
{
    /// <summary>
    /// <para>Summary of components for the release report by release version</para>
    /// </summary>
    public class ReleaseStatusReportSummary
    {
        /// <summary>
        /// <para>The name of the release</para>
        /// </summary>
        public string ReleaseName { get; set; }

        /// <summary>
        /// <para>Number of story points for the release</para>
        /// </summary>
        public string StoryPoints { get; set; }

        /// <summary>
        /// <para>Expected estimated completion of the release</para>
        /// </summary>
        public DateTime EstimatedCompletion { get; set; }
    }
}