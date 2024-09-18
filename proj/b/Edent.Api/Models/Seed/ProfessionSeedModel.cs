using System.Collections.Generic;

namespace Edent.Api.Models.Seed
{
    public class ProfessionSeedModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<SpecializationSeedModel> Specializations { get; set; }
    }
}
