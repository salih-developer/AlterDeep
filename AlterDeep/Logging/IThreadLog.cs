using System.Threading.Tasks;

namespace AlterDeep.Logging
{
    public interface IThreadLog
    {
        Task<bool> Insert(object obj);
    }
}