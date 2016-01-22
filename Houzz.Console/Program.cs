using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace Houzz.Console
{
    /// <summary>
    /// This is simply used to debug the Houzz Wrapper
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var houzzService = new Projects();
            var projectImages = houzzService.GetAll("username");
            System.Console.WriteLine(projectImages);
        } 
    }
}
