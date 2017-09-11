using Core.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.View
{
    public static class DmViewService
    {
        public static bool Add(this DomainModel<ViewEntity> viewModel)
        {
            if (!viewModel.AddEntity()) return false;

            if(viewModel.Entity.DataContainer == DataContainer.Empty)
            {
                viewModel.Entity.DataContainer = DataContainer.SeflHost;
            }
            
            if (viewModel.Entity.Columns != null)
            {
                viewModel.Entity.Columns.ForEach(column =>
                {
                    var dm = new DomainModel<ViewColumEntity>(viewModel.Dc, column);
                    dm.Entity.ViewId = viewModel.Entity.Id;
                    dm.AddEntity();
                });
            }

            if (viewModel.Entity.Actions != null)
            {
                viewModel.Entity.Actions.ForEach(action =>
                {
                    var dm = new DomainModel<ViewActionEntity>(viewModel.Dc, action);
                    dm.Entity.ViewId = viewModel.Entity.Id;
                    dm.AddEntity();
                });
            }

            return true;
        }

        public static void Load(this DomainModel<ViewEntity> viewModel)
        {
            CoreDbContext dc = viewModel.Dc;
            viewModel.LoadEntity();
            viewModel.Entity.Result = new DmPageResult<object>() { Size = 20, Page = 1, Items = new List<object>() };
            viewModel.Entity.Columns = dc.Table<ViewColumEntity>().Where(x => x.ViewId == viewModel.Entity.Id).OrderBy(x => x.DisplayName).ToList();
            viewModel.Entity.Actions = dc.Table<ViewActionEntity>().Where(x => x.ViewId == viewModel.Entity.Id).OrderBy(x => x.Name).ToList();
        }

        public static void LoadDefinition(this DomainModel<ViewEntity> viewModel)
        {
            CoreDbContext dc = viewModel.Dc;
            viewModel.LoadEntity();
            viewModel.Entity.Columns = dc.Table<ViewColumEntity>().Where(x => x.ViewId == viewModel.Entity.Id).OrderBy(x => x.DisplayName).ToList();
            viewModel.Entity.Actions = dc.Table<ViewActionEntity>().Where(x => x.ViewId == viewModel.Entity.Id).OrderBy(x => x.Name).ToList();
        }
    }
}
