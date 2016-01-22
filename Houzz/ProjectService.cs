using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Houzz
{
    public class ProjectService 
    {
        /// <summary>
        /// Retrieves list of Houzz project IDs
        /// </summary>
        /// <param name="username">Houzz Username</param>
        /// <returns>List of project IDs</returns>
        public IEnumerable<int> GetProjectIds(string username)
        {
            var url = string.Format("http://www.houzz.com/projects/users/{0}", username);
            HtmlDocument htmlDoc = new HtmlWeb().Load(url);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='sidebar-body']//a");

            if (nodes != null && nodes.Any())
            {
                var links = nodes.Skip(1) // Skip "All Projects"
                                .Select(e => e.GetAttributeValue("href", null).ToString());

                var projects = new List<int>();

                foreach (var link in links)
                {
                    var splicedUrl = link.Split('/');
                    if (splicedUrl.Length >= 4)
                    {
                        projects.Add(Convert.ToInt32(splicedUrl[4]));
                    }
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
        public IEnumerable<string> GetProjectImages(int projectId)
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

        /// <summary>
        /// Retrieves all images from all projects by username
        /// </summary>
        /// <param name="username">Houzz Username</param>
        /// <returns>List of image urls</returns>
        public IEnumerable<string> GetAllProjectImages(string username)
        {
            var projectIds = GetProjectIds(username);
            var projectImages = new List<string>();

            foreach (var projectId in projectIds)
            {
                var images = GetProjectImages(projectId);
                if (images != null) { projectImages.AddRange(images); }
            }

            return projectImages;
        }
    }
}
