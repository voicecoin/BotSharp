using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IInitializationLoader
    {
        int Priority { get; }
        void Initialize(IConfiguration config, IHostingEnvironment env);
    }

    /// <summary>
    /// Domain model, based on Domain-Driven Design concept.
    /// Business model shold implement this interface.
    /// </summary>
    public interface IDomainModel
    {
    }

    public interface IDomainModel<T> where T : IDbRecord
    {
        Boolean AddEntity();
        Boolean RemoveEntity();
        T LoadEntity();
    }

    /// <summary>
    /// Initialize data for modules
    /// </summary>
    public interface IHookDbInitializer
    {
        /// <summary>
        /// value smaller is higher priority
        /// </summary>
        int Priority { get; }
        void Load(IHostingEnvironment env, IConfiguration config, Database dc);
    }

    public interface IDbTableAmend
    {
        void Amend(Database dc);
    }

    public interface IEntityPermission
    {

    }
}
