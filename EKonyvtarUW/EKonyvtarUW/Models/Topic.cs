using SQLite;

namespace EKonyvtarUW.Models
{
    [Table("topic")]
    public class Topic
    {

        [Column("_id")]
        public string Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("parent_id")]
        public string ParentId { get; set; }

    }
}
