namespace Edent.Api.Infrastructure.Entities
{
    public class DentalServiceReceptInventorySetting : Entity
    {
        public int DentalServiceId { get; set; }
        public virtual DentalService DentalService { get; set; }
        public int ReceptInventorySettingId { get; set; }
        public virtual ReceptInventorySetting ReceptInventorySetting { get; set; }
    }
}
