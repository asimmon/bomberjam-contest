using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

// ReSharper disable MethodHasAsyncOverload
namespace Bomberjam.Website.Database
{
    public partial class DatabaseRepository
    {
        public async Task<ITransaction> CreateTransaction()
        {
            var transaction = await this._dbContext.Database.BeginTransactionAsync().ConfigureAwait(false);
            return new TransactionWrapper(transaction);
        }

        private sealed class TransactionWrapper : ITransaction
        {
            private readonly IDbContextTransaction _transaction;

            public TransactionWrapper(IDbContextTransaction transaction)
            {
                this._transaction = transaction;
            }

            public void Dispose()
            {
                this._transaction.Dispose();
            }

            public Task CommitAsync()
            {
                return this._transaction.CommitAsync();
            }
        }
    }
}