using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IFileRepository
    {
        Task<TWFile> SaveFileAsync(TWFile file);
    }
}
