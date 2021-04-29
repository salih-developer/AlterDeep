using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlterDeep.DBOperations.Model
{
    [Table("Content")]
    public class Content : ModelBase
    {
        public int Id { get; set; }
        public string ContentText { get; set; }
        public IList<TransactionPageContents> TransactionPageContents { get; set; }
    }
}