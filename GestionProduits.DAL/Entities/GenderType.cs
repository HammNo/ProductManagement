using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Enums;

namespace ProductManagement.DAL.Entities
{
    public enum GenderType
    {
        [Description("Homme")]
        Male,
        [Description("Femme")]
        Female,
        [Description("Autre")]
        Other
    }
}
