using System.Xml;
using Newtonsoft.Json;

namespace SSRS_for_JIRA.WebServices.Helpers
{
    /// <summary>
    /// <para>Helper for conversion of JSON to XML</para>
    /// </summary>
    public class JsonToXmlHelper
    {
        /// <summary>
        /// <para>Take a JSON string (like those returned from JIRA REST API calls) and formats as an XmlDocument</para>
        /// </summary>
        /// <param name="json">The JSON string to conver to XML</param>
        /// <returns>
        /// <para>Returns the XmlDocument representation of the JSON string</para>
        /// </returns>
        public static XmlDocument JsonToXml(string json)
        {
            var xmlDocument = JsonConvert.DeserializeXmlNode(json, "root");
            return xmlDocument;
        }
    }
}