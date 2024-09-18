using System.Collections.Generic;

namespace Edent.Api.ViewModels
{
    public class ReceptInventorySettingViewModel : IViewModel
    {
        public ReceptInventorySettingViewModel()
        {
            ReceptInventorySettingItems = new HashSet<ReceptInventorySettingItemViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public virtual ICollection<ReceptInventorySettingItemViewModel> ReceptInventorySettingItems { get; set; }
    }
}
