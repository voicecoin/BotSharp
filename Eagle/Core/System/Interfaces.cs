using Core.Menu;
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
        void Initialize(IConfigurationRoot config, IHostingEnvironment env);
    }

    /// <summary>
    /// All table should implement this table inteface
    /// </summary>
    public interface IDbRecord
    {
        bool IsExist(CoreDbContext dc);
    }
    public interface IDbRecord4Core { }
    public interface IDbRecord4MySql { }

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
        void Load(IHostingEnvironment env, IConfigurationRoot config, CoreDbContext dc);
    }

    public interface IDbTableAmend
    {
        void Amend(CoreDbContext dc);
    }

    public interface IHookMenu
    {
        void UpdateMenu(List<VmMenu> menus, CoreDbContext dc);
    }

    public interface IEntityPermission
    {

    }
}
