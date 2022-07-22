using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Enums;

namespace ProductManagement.DAL.Entities
{
    public enum OrderStatus
    {
        [Description("En cours")]
        InProgress,
        [Description("En attente")]
        Penging,
        [Description("Cloturée")]
        Closed,
        [Description("Annulée")]
        Cancel
    }
}
