using System;

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
        /// <para>Number of story points (total) for the release</para>
        /// </summary>
        public string StoryPointsTotal { get; set; }

        /// <summary>
        /// <para>Number of story points (Completed, aka Verified or Resolved) for the release</para>
        /// </summary>
        public string StoryPointsCompleted { get; set; }

        /// <summary>
        /// <para>Number of story points (Remaining) for the release</para>
        /// </summary>
        public string StoryPointsRemaining { get; set; }

        /// <summary>
        /// <para>Planned/desired completion of the release</para>
        /// </summary>
        public DateTime PlannedCompletion { get; set; }

        /// <summary>
        /// <para>Expected estimated completion of the release</para>
        /// </summary>
        public DateTime EstimatedCompletion { get; set; }

        /// <summary>
        /// <para>The different between planned and estimated in number of sprints (negative = later than expected, positive = earlier than expected)</para>
        /// </summary>
        public int CompletionSprintDifference { get; set; }
    }
}