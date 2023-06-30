using Arsha.Core.Entities;

namespace Arsha.App.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Service> Services { get; set; }
        public IEnumerable<TeamMembers> TeamMembers { get; set; }
        public IEnumerable<Item> Items { get; set; }

    }
}
