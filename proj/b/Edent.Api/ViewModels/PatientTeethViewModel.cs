using Edent.Api.Infrastructure.Enums;
using System.Collections.Generic;

namespace Edent.Api.ViewModels
{
    public class PatientTeethViewModel
    {
        public PatientTeethViewModel()
        {
            BottomLeft = new HashSet<PatientToothViewModel>();
            BottomRight = new HashSet<PatientToothViewModel>();
            TopLeft = new HashSet<PatientToothViewModel>();
            TopRight = new HashSet<PatientToothViewModel>();
        }

        public int PatientId { get; set; }
        public PatientAgeType PatientAgeType { get; set; }
        public ICollection<PatientToothViewModel> BottomLeft { get; set; }
        public ICollection<PatientToothViewModel> BottomRight { get; set; }
        public ICollection<PatientToothViewModel> TopLeft { get; set; }
        public ICollection<PatientToothViewModel> TopRight { get; set; }
    }
}
