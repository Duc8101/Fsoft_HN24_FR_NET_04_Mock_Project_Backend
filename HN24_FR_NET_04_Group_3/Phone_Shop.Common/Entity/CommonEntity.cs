using System.ComponentModel.DataAnnotations.Schema;

namespace Phone_Shop.Common.Entity
{
    public class CommonEntity
    {
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("update_at")]
        public DateTime UpdateAt { get; set; }
    }
}
