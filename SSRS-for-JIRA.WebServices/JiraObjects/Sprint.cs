using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Linq;

namespace SSRS_for_JIRA.WebServices.JiraObjects
{
    /// <summary>
    /// <para>JIRA Sprint element</para>
    /// </summary>
    public class Sprint
    {
        [XmlElement("id")]
        public int Id { get; set; }
        [XmlElement("self")]
        public string Url { get; set; }
        [XmlElement("state")]
        public string State { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("startDate")]
        public DateTime StartDate { get; set; }
        [XmlElement("endDate")]
        public DateTime EndDate { get; set; }
        [XmlElement("completeDate")]
        public DateTime CompleteDate { get; set; }
        [XmlElement("originBoardId")]
        public int ScrumBoardId { get; set; }

        /// <summary>
        /// <para>Issues related to the sprint.</para>
        /// </summary>
        /// <remarks>
        /// <para>Must be populated seperately as the JSON returned does not include full issue listing</para>
        /// </remarks>
        public List<Issue> SprintIssues { get; set; }

        /// <summary>
        /// <para>The number of story points attached to the sprint</para>
        /// </summary>
        public decimal StoryPointsTotal
        {
            get
            {
                var storyPointsTotal = (from si in this.SprintIssues
                                         select String.IsNullOrEmpty(si.IssueFields.StoryPoints) ? 0 : Convert.ToDecimal(si.IssueFields.StoryPoints)).Sum();
                return storyPointsTotal;
            }
        }

        /// <summary>
        /// <para>The number of story points completed in the sprint</para>
        /// </summary>
        public decimal StoryPointsCompleted
        {
            get
            {
                var storyPointsCompleted = (from si in this.SprintIssues
                                            where !String.IsNullOrWhiteSpace(si.IssueFields.ResolvedOn)
                                                  && Convert.ToDateTime(si.IssueFields.ResolvedOn) > this.StartDate
                                                  && Convert.ToDateTime(si.IssueFields.ResolvedOn) < this.EndDate
                                            select String.IsNullOrEmpty(si.IssueFields.StoryPoints) ? 0 : Convert.ToDecimal(si.IssueFields.StoryPoints)).Sum();
                return storyPointsCompleted;
            }
        }

        /// <summary>
        /// <para>Stories completed for the specific component</para>
        /// </summary>
        /// <param name="component">Component to limit the calculation to</param>
        /// <returns>
        /// <para>The number of stories completed for the component selected</para>
        /// </returns>
        public int StoriesCompletedForComponent(string component)
        {
            var possibleStoriesCompletedForComponent = (from si in this.SprintIssues
                                                        where si.IssueFields.Components.Any(c => c.Name.ToLowerInvariant() == component)
                                                        select si).ToList();
            var storiesCompletedForComponent = (from si in possibleStoriesCompletedForComponent
                                                where !String.IsNullOrWhiteSpace(si.IssueFields.ResolvedOn)
                                                      && Convert.ToDateTime(si.IssueFields.ResolvedOn) > this.StartDate
                                                      && Convert.ToDateTime(si.IssueFields.ResolvedOn) < this.EndDate
                                                select si).Count();

            return storiesCompletedForComponent;
        }

        /// <summary>
        /// <para>Story points completed for the specific component</para>
        /// </summary>
        /// <param name="component">Component to limit the calculation to</param>
        /// <returns>
        /// <para>The number of story points completed for the component selected</para>
        /// </returns>
        public decimal StoryPointsCompletedForComponent(string component)
        {
            var possibleStoryPointsCompletedForComponent = (from si in this.SprintIssues
                                                            where si.IssueFields.Components.Any(c => c.Name.ToLowerInvariant() == component)
                                                            select si).ToList();
            var storyPointsCompletedForComponent = (from si in possibleStoryPointsCompletedForComponent
                                                    where !String.IsNullOrWhiteSpace(si.IssueFields.ResolvedOn)
                                                          && Convert.ToDateTime(si.IssueFields.ResolvedOn) > this.StartDate
                                                          && Convert.ToDateTime(si.IssueFields.ResolvedOn) < this.EndDate
                                                    select String.IsNullOrEmpty(si.IssueFields.StoryPoints) ? 0 : Convert.ToDecimal(si.IssueFields.StoryPoints)).Sum();
            storyPointsCompletedForComponent = Math.Round(storyPointsCompletedForComponent, 3);

            return storyPointsCompletedForComponent;
        }

        /// <summary>
        /// <para>Deserialize XML to Sprint object</para>
        /// </summary>
        /// <param name="xml">XML to deserialize</param>
        /// <returns>
        /// <para>Returns an Sprint object representing the XML data</para>
        /// </returns>
        public static Sprint DeserializeFromXml(string xml)
        {
            Sprint sprint;

            var serializer = new XmlSerializer(typeof(Sprint));
            using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(xml)))
            {
                sprint = serializer.Deserialize(memoryStream) as Sprint;
            }

            return sprint;
        }
    }
}