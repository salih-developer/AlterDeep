using System.ComponentModel.DataAnnotations.Schema;

namespace AlterDeep.DBOperations.Model
{
    [Table("TransactionPageContents")]
    public class TransactionPageContents
    {
        public int TransactionPageId { get; set; }
        public TransactionPage TransactionPage { get; set; }
        public int ContentId { get; set; }
        public Content Content { get; set; }
    }
}