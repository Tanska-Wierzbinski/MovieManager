using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMovie Movie { get; }
        IActor Actor { get; }
        ICategory Category { get; }

        IReview Review { get; }

        Task Save();
    }
}
