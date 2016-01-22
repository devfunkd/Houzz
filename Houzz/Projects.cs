using System;
using System.Collections.Generic;
using System.Linq;
using Houzz.Domain;
using HtmlAgilityPack;

namespace Houzz
{
    public class Projects 
    {
        /// <summary>
        /// Retrieves Houzz Projects
        /// </summary>
        /// <param name="username">Houzz Username</param>
        /// <returns></returns>
        public List<HouzzProject> GetAll(string username)
        {
            var url = string.Format("http://www.houzz.com/projects/users/{0}", username);
            HtmlDocument htmlDoc = new HtmlWeb().Load(url);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='sidebar-body']//a");
            var projects = new List<HouzzProject>();

            if (nodes != null && nodes.Any())
            {
                foreach (var node in nodes.Skip(1)) // Skipping the first one ("All Projects")
                {
                    var id = ParseProjectId(node);
                    var name = ParseProjectName(node);

                    projects.Add(new HouzzProject()
                    {
                        Id = id,
                        Name = name,
                        Images = GetImages(id)
                    });
                }

                return projects;
            }

            return null;
        }

        /// <summary>
        /// Returns a list of image urls within a Houzz project
        /// </summary>
        /// <param name="projectId">Houzz Project ID</param>
        /// <returns>List of image urls</returns>
        public IEnumerable<string> GetImages(int projectId)
        {
            var url = string.Format("http://www.houzz.com/projects/{0}", projectId);
            HtmlDocument htmlDoc = new HtmlWeb().Load(url);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='imageArea']//img");

            if (nodes != null && nodes.Any())
            {
                var images = nodes.Select(e => e.GetAttributeValue("src", null))
                                    .Where(s => !string.IsNullOrEmpty(s));

                return images;
            }

            return null;
        }

        private static int ParseProjectId(HtmlNode node)
        {
            var link = node.GetAttributeValue("href", null);
            var splicedUrl = link.Split('/');
            if (splicedUrl.Length >= 4 && splicedUrl[4] != "users")
            {
                return Convert.ToInt32(splicedUrl[4]);
            }

            return 0;
        }

        private static string ParseProjectName(HtmlNode node)
        {
            return node.InnerText;
        }
    }
}
