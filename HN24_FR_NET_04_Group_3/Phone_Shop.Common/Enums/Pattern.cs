using System.ComponentModel;

namespace Phone_Shop.Common.Enums
{
    public enum Pattern
    {
        [Description(@"^\d{10}$")]
        Phone,

        [Description(@"^[a-zA-Z][\w-]+@([\w]+.[\w]+|[\w]+.[\w]{2,}.[\w]{2,})")]
        Email,

        [Description(@"^[a-zA-Z][a-zA-Z0-9]{5,49}$")]
        Username
    }
}
