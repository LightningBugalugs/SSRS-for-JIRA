using SSRS_for_JIRA.WebServices.JiraObjects;
using System.Collections.Generic;
using System.Linq;

namespace SSRS_for_JIRA.WebServices.ReportObjects
{
    /// <summary>
    /// <para>The data item for the velocity component report</para>
    /// </summary>
    public class ComponentReport
    {        
        /// <summary>
        /// <para>The name of the component</para>
        /// </summary>
        public string Component { get; set; } 

        /// <summary>
        /// <para>The sprint id for the line item</para>
        /// </summary>
        public int SprintId { get; set; }

        /// <summary>
        /// <para>The sprint name for the line item</para>
        /// </summary>
        public string SprintName { get; set; }

        /// <summary>
        /// <para>The count of stories for that sprint for the component</para>
        /// </summary>
        public int StoriesForComponent { get; set; } 

        /// <summary>
        /// <para>The count of stories completed for that sprint for the component</para>
        /// </summary>
        public int StoriesCompletedForComponent { get; set; } 

        /// <summary>
        /// <para>The sum of story points completed for that sprint for the component</para>
        /// </summary>
        public decimal StoryPointsCompletedForComponent { get; set; }

        /// <summary>
        /// <para>Velocity for the component calculated over the last four sprints</para>
        /// </summary>
        public decimal VelocityForComponentFourSprintCalc { get; set; }

        /// <summary>
        /// <para>Velocity for the component calculated over the last six sprints</para>
        /// </summary>
        public decimal VelocityForComponentSixSprintCalc { get; set; }

        /// <summary>
        /// <para>Default contstructor</para>
        /// </summary>
        public ComponentReport() { }

        /// <summary>
        /// <para>Constructor from sprints for the component report item</para>
        /// </summary>
        /// <param name="component">The name of the component</param>
        /// <param name="sprintId">The id of the sprint</param>
        /// <param name="sprintName">The name of the sprint</param>
        /// <param name="sprints">The sprint data listing</param>
        public ComponentReport(string component, int sprintId, string sprintName, List<Sprint> sprints)
        {
            this.Component = component;
            this.SprintId = sprintId;
            this.SprintName = sprintName;
            this.VelocityForComponentFourSprintCalc = 0.0m;
            this.VelocityForComponentSixSprintCalc = 0.0m;

            // sprint details data
            var sprint = (from s in sprints
                          where s.Id == sprintId
                          select s).First();
            this.StoriesForComponent = (from si in sprint.SprintIssues
                                        where si.IssueFields.Components.Any(c => c.Name.ToLowerInvariant() == component.ToLowerInvariant())
                                        select si).Count();
            this.StoriesCompletedForComponent = sprint.StoriesCompletedForComponent(component);
            this.StoryPointsCompletedForComponent = sprint.StoryPointsCompletedForComponent(component);

            // velocity details data
            var fourWeekSprints = (from s in sprints
                                   where s.Id <= sprintId
                                   orderby s.Id descending
                                   select s).Take(4).ToList();
            foreach (var fourWeekSprint in fourWeekSprints)
            {
                this.VelocityForComponentFourSprintCalc += fourWeekSprint.StoryPointsCompletedForComponent(component);
            }
            this.VelocityForComponentFourSprintCalc = this.VelocityForComponentFourSprintCalc / 4.0m;

            var sixWeekSprints = (from s in sprints
                                  where s.Id <= sprintId
                                  orderby s.Id descending
                                  select s).Take(6).ToList();
            foreach (var sixWeekSprint in sixWeekSprints)
            {
                this.VelocityForComponentSixSprintCalc += sixWeekSprint.StoryPointsCompletedForComponent(component);
            }
            this.VelocityForComponentSixSprintCalc = this.VelocityForComponentSixSprintCalc / 6.0m;
        }
    }
}