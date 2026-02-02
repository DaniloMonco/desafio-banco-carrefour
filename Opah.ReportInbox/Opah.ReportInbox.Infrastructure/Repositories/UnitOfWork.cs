using Opah.ReportInbox.Domain.Repositories;

namespace Opah.ReportInbox.Infrastructure.Repositories
{
    public sealed class UnitOfWork(DapperContext context) : IUnitOfWork
    {
        private readonly DapperContext _context = context;
        
        public void BeginTransaction()
            => _context.BeginTransaction();


        public void Commit()
            => _context.Commit();

        public void Rollback()
            => _context.Rollback();

    }
}
