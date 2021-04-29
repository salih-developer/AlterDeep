using System.ComponentModel.DataAnnotations.Schema;

namespace AlterDeep.DBOperations.Model
{
    [Table("Flow")]
    public class Flow : ModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}