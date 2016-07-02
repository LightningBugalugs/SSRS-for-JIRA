using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SSRS_for_JIRA.WebServices.JiraObjects
{
    /// <summary>
    /// <para>JIRA Release element</para>
    /// </summary>
    public class Release
    {
        [XmlElement("id")]
        public int Id { get; set; }
        [XmlElement("self")]
        public string Url { get; set; }
        [XmlElement("state")]
        public string State { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("description")]
        public string Description { get; set; }
        [XmlElement("releaseDate")]
        public DateTime TargetReleaseDate { get; set; }

        public string DescriptionDisplayText
        {
            get
            {
                var splitCharacters = new char[] { ',', ';'};
                var descriptionDisplayText = new StringBuilder();
                foreach (var splitDescription in this.Description.Split(splitCharacters, StringSplitOptions.RemoveEmptyEntries))
                {
                    descriptionDisplayText.AppendLine(splitDescription);
                }
                return descriptionDisplayText.ToString();
            }
        }

        /// <summary>
        /// <para>Issues related to the release.</para>
        /// </summary>
        /// <remarks>
        /// <para>Must be populated seperately as the JSON returned does not include full issue listing</para>
        /// </remarks>
        public List<Issue> ReleaseIssues { get; set; }

        /// <summary>
        /// <para>Deserialize XML to Release object</para>
        /// </summary>
        /// <param name="xml">XML to deserialize</param>
        /// <returns>
        /// <para>Returns an Release object representing the XML data</para>
        /// </returns>
        public static Release DeserializeFromXml(string xml)
        {
            Release release;

            var serializer = new XmlSerializer(typeof(Release));
            using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(xml)))
            {
                release = serializer.Deserialize(memoryStream) as Release;
            }

            return release;
        }
    }
}