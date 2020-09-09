using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rtl.Assignment.Domain.Entities;

namespace Rtl.Assignment.Domain.Repositories
{
    public interface IShowWithCastRepository
    {
        Task<IEnumerable<ShowWithCastEntity>> GetAllAsync(int page, CancellationToken token);
        
        Task UpdateShowBulkAsync(IEnumerable<ShowWithCastEntity> replacements, CancellationToken token);
       
        Task UpdateCastAsync(int id, ShowWithCastEntity replacement, CancellationToken token);
    }
}