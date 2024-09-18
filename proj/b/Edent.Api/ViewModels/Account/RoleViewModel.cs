using System.Collections.Generic;

namespace Edent.Api.ViewModels.Account
{
    public class RoleViewModel : IViewModel
    {
        public RoleViewModel()
        {
            Permissions = new HashSet<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<string> Permissions { get; set; }
    }
}
