using DataService;
using System.Collections.Generic;
namespace BatchCoreService
{
    public class Group
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public int UpdateRate { get; set; }
        public int DeadBand { get; set; }
        public bool Active { get; set; }
        public int DriverId { get; set; }
        public ICollection<TagMetaData> TagMetas { get; set; } = new List<TagMetaData>();
    }
}
