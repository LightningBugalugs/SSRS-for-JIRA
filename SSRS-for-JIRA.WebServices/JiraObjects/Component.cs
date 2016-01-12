using System.Xml.Serialization;

namespace SSRS_for_JIRA.WebServices.JiraObjects
{
    /// <summary>
    /// <para>JIRA Component Element</para>
    /// </summary>
    public class Component
    {
        [XmlElement("id")]
        public int Id { get; set; }
        [XmlElement("self")]
        public string Url { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("description")]
        public string Description { get; set; }
    }
}