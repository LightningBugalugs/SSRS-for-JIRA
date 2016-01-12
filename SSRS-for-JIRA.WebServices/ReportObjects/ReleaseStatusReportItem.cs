namespace SSRS_for_JIRA.WebServices.ReportObjects
{
    /// <summary>
    /// <para>Line item object for relese status report</para>
    /// </summary>
    public class ReleaseStatusReportItem
    {
        /// <summary>
        /// <para>The name of the release</para>
        /// </summary>
        public string ReleaseName { get; set; }
        
        /// <summary>
        /// <para>The status the issue is currently in</para>
        /// </summary>
        public string IssueStatus { get; set; }

        /// <summary>
        /// <para>The order the status should be displayed in</para>
        /// </summary>
        public int IssueStatusOrder { get; set; }

        /// <summary>
        /// <para>Number of story points for the issue</para>
        /// </summary>
        public string StoryPoints { get; set; }        

        /// <summary>
        /// <para>Calcuated field to generate a status order Id used for ordering the issues</para>
        /// </summary>
        public int IssueStatusOrderId
        {
            get
            {
                var issueStatusOrderId = -1;

                switch (this.IssueStatus.ToLowerInvariant())
                {
                    case "waiting for support":
                        issueStatusOrderId = 1;
                        break;
                    case "proposed story":
                        issueStatusOrderId = 2;
                        break;
                    case "pending external resource":
                        issueStatusOrderId = 3;
                        break;
                    case "open":
                        issueStatusOrderId = 4;
                        break;                    
                    case "in progress":
                        issueStatusOrderId = 5;
                        break;
                    case "in system/uat testing":
                        issueStatusOrderId = 6;
                        break;
                    case "verified":
                        issueStatusOrderId = 7;
                        break;
                    case "resolved":
                        issueStatusOrderId = 8;
                        break;
                    default:
                        break;
                }

                return issueStatusOrderId;
            }
        }
    }
}