using System;
using System.Threading.Tasks;

namespace Bomberjam.Website.Database
{
    public interface ITransaction : IDisposable
    {
        public Task CommitAsync();
    }
}