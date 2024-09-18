using System.ComponentModel;

namespace Phone_Shop.Common.Enums
{
    public enum OrderStatus
    {
        Pending,
        Rejected,
        Approved,
        Done,

        [Description("Ship Failed")]
        Ship_Fail
    }
}
