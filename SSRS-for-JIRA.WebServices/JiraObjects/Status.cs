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
    }
}