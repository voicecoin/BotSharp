using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataContexts;

namespace Core.Interfaces
{
    /// <summary>
    /// All table should implement this table inteface
    /// </summary>
    public interface IDbRecord { }

    /// <summary>
    /// Domain model, based on Domain-Driven Design concept.
    /// Business model shold implement this interface.
    /// </summary>
    public interface IDomainModel { }
    /// <summary>
    /// Initialize data for modules
    /// </summary>
    public interface IDbInitializer
    {
        /// <summary>
        /// value smaller is higher priority
        /// </summary>
        int Priority { get; }
        void Load(IHostingEnvironment env, CoreDbContext dc);
    }
}
