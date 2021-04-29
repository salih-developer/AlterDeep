using System;

namespace AlterDeep.DBOperations.Model
{
    public abstract class ModelBase
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}