using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlterDeep.DBOperations.Model
{
    [Table("TransactionPage")]
    public class TransactionPage : ModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public IList<TransactionPageContents> TransactionPageContents { get; set; }
    }
}