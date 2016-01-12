using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SSRS_for_JIRA.WebServices.JiraObjects
{
    /// <summary>
    /// <para>JIRA Issue element</para>
    /// </summary>
    public class Issue
    {
        [XmlElement("id")]
        public int Id { get; set; }
        [XmlElement("self")]
        public string Url { get; set; }
        [XmlElement("key")]
        public string Key { get; set; }
        [XmlElement("fields")]
        public IssueFields IssueFields { get; set; }

        /// <summary>
        /// <para>Deserialize XML to Issue object</para>
        /// </summary>
        /// <param name="xml">XML to deserialize</param>
        /// <returns>
        /// <para>Returns an Issue object representing the XML data</para>
        /// </returns>
        public static Issue DeserializeFromXML(string xml)
        {
            Issue issue;

            var serializer = new XmlSerializer(typeof(Issue));
            using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(xml)))
            {
                issue = serializer.Deserialize(memoryStream) as Issue;
            }

            return issue;
        }
    }
}