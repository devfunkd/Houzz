using System.Collections.Generic;

namespace Houzz.Domain
{
    public class HouzzProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Images { get; set; }
    }
}
