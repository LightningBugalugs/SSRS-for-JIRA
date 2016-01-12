using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SSRS_for_JIRA.WebServices.JiraObjects
{
    /// <summary>
    /// <para>JIRA IssueFields element</para>
    /// </summary>
    public class IssueFields
    {
        [XmlElement("fixVersions")]
        public FixVersion FixVersion { get; set; }
        [XmlElement("created")]
        public string CreatedOn { get; set; }
        [XmlElement("resolutiondate")]
        public string ResolvedOn { get; set; }
        [XmlElement("customfield_10005")]
        public string StoryPoints { get; set; }
        [XmlElement("summary")]
        public string Summary { get; set; }
        [XmlElement("components")]
        public List<Component> Components { get; set; }
        [XmlElement("status")]
        public Status Status { get; set; }

        /// <summary>
        /// <para>Deserialize XML to IssueFields object</para>
        /// </summary>
        /// <param name="xml">XML to deserialize</param>
        /// <returns>
        /// <para>Returns an IssueFields object representing the XML data</para>
        /// </returns>
        public static IssueFields DeserializeFromXml(string xml)
        {
            IssueFields issueFields;

            var serializer = new XmlSerializer(typeof(IssueFields));
            using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(xml)))
            {
                issueFields = serializer.Deserialize(memoryStream) as IssueFields;
            }

            return issueFields;
        }
    }
}