using SSRS_for_JIRA.WebServices.JiraObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SSRS_for_JIRA.WebServices.ReportObjects
{
    /// <summary>
    /// <para>Summary of components for the velocity report by component</para>
    /// </summary>
    public class ComponentReportSummary
    {
        /// <summary>
        /// <para>The name of th ecomponent</para>
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// <para>The number of sprints for the component</para>
        /// </summary>
        public int SprintCount { get; set; }

        /// <summary>
        /// <para>The number of stories for the component</para>
        /// </summary>
        public int StoriesForComponent { get; set; }

        /// <summary>
        /// <para>The number of completed stories for the component</para>
        /// </summary>
        public int StoriesCompletedForComponent { get; set; }

        /// <summary>
        /// <para>The number of remaining stories for the component</para>
        /// </summary>
        public int StoriesRemainingForComponent { get; set; }

        /// <summary>
        /// <para>The number of completed story points for the component</para>
        /// </summary>
        public decimal StoryPointsCompletedForComponent { get; set; }

        /// <summary>
        /// <para>The number of remaining story points for the component</para>
        /// </summary>
        public decimal StoryPointsRemainingForComponent { get; set; }

        /// <summary>
        /// <para>Velocity for the component calculated over the last four sprints</para>
        /// </summary>
        public decimal VelocityForComponentFourSprintCalc { get; set; }

        /// <summary>
        /// <para>Velocity for the component calculated over the last six sprints</para>
        /// </summary>
        public decimal VelocityForComponentSixSprintCalc { get; set; }

        /// <summary>
        /// <para>Default constructor</para>
        /// </summary>
        public ComponentReportSummary() { }

        /// <summary>
        /// <para>Constructor from component reports to create the summary report</para>
        /// </summary>
        /// <param name="component">The component name for the summary</param>
        /// <param name="componentReports">The component report items to make up the summary</param>
        /// <param name="totalStories">The issue items to make up the summary</param>
        public ComponentReportSummary(string component, List<ComponentReport> componentReports, List<Issue> totalStories) 
        {
            this.Component = component;
            this.SprintCount = (from cr in componentReports
                                where cr.Component == component
                                select cr).Count();
            var lastComponentReport = (from cr in componentReports
                                       where cr.Component == component
                                       orderby cr.SprintId descending
                                       select cr).First();
            this.VelocityForComponentFourSprintCalc = lastComponentReport.VelocityForComponentFourSprintCalc;
            this.VelocityForComponentFourSprintCalc = Math.Round(this.VelocityForComponentFourSprintCalc, 3);
            this.VelocityForComponentSixSprintCalc = lastComponentReport.VelocityForComponentSixSprintCalc;
            this.VelocityForComponentSixSprintCalc = Math.Round(this.VelocityForComponentSixSprintCalc, 3);
            this.StoriesCompletedForComponent = (from cr in componentReports
                                                 where cr.Component == component
                                                 select cr.StoriesCompletedForComponent).Sum();
            this.StoryPointsCompletedForComponent = (from cr in componentReports
                                                     where cr.Component == component
                                                     select cr.StoryPointsCompletedForComponent).Sum();

            this.StoriesForComponent = (from s in totalStories
                                        select s).Count();
            this.StoriesRemainingForComponent = (from s in totalStories
                                                 where s.IssueFields.ResolvedOn == null
                                                 select s).Count();
            this.StoryPointsRemainingForComponent = (from s in totalStories
                                                     where s.IssueFields.ResolvedOn == null
                                                     select String.IsNullOrEmpty(s.IssueFields.StoryPoints) ? 0 : Convert.ToDecimal(s.IssueFields.StoryPoints)).Sum();
        }
    }
}