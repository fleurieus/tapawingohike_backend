using Tapawingo_backend.Data;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly DataContext _context;

        public FileRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<TWFile> SaveFileAsync(TWFile file)
        {
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
            return file;
        }
    }
}
