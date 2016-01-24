using System.Xml.Serialization;

namespace SSRS_for_JIRA.WebServices.JiraObjects
{
    /// <summary>
    /// <para>JIRA Status element</para>
    /// </summary>
    public class Status
    {
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// <para>Status Name translation from status name in JIRA to related status name.</para>
        /// </summary>
        public string StatusName
        {
            get
            {
                var statusName = this.Name;
                switch (statusName.ToLowerInvariant())
                {
                    case "pending external resource":
                    case "waiting for customer":
                        statusName = "On Hold";
                        break;
                    case "pending automated testing":
                    case "pending testing deployment":
                    case "in system/uat testing":
                    case "pending code review":
                        statusName = "In Testing";
                        break;
                    default:
                        break;
                }
                return statusName;
            }
        }
    }
}