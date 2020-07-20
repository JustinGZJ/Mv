using System.Collections.Generic;
namespace BatchCoreService
{
    public class Driver
    {
        public short Id { get; set; }
        public string Name { get; set; } = "";
        public string Assembly { get; set; }
        public string ClassName { get; set; }
        public string Server { get; set; }
        public string Description { get; set; }
        public int Timeout { get; set; }
        public ICollection<Group> Groups { get; set; } = new List<Group>();
        public ICollection<DriverArgument> Arguments { get; set; } = new List<DriverArgument>();
        public bool Active { get; set; }
    }
}
