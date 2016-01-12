using System.Xml.Serialization;

namespace SSRS_for_JIRA.WebServices.JiraObjects
{
    /// <summary>
    /// <para>JIRA FixVersion element</para>
    /// </summary>
    public class FixVersion
    {
        [XmlElement("id")]
        public int Id { get; set; }
        [XmlElement("description")]
        public string Description { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("releasedDate")]
        public string ReleaseDate { get; set; }
    }
}