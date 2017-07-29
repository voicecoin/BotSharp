using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Core.Interfaces;
using Core.Account;
using Core;
using Core.Bundle;

namespace Apps.Baas
{
    public class Hooks : IHookDbInitializer
    {
        public int Priority => 999;
        public const string DEFAULT_PEOPLE_IMAGE = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAMAAAD04JH5AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAA2ZpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDpBRUQ4OTNDQjJDNzJFNzExQkJGQUQ2QUJCQTJGNTI2NyIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDpFNzFGMzg4ODcyMkMxMUU3QkYxODlBMkZBMTBGNkE2MiIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDpFNzFGMzg4NzcyMkMxMUU3QkYxODlBMkZBMTBGNkE2MiIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgQ1M2IChXaW5kb3dzKSI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOkFFRDg5M0NCMkM3MkU3MTFCQkZBRDZBQkJBMkY1MjY3IiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOkFFRDg5M0NCMkM3MkU3MTFCQkZBRDZBQkJBMkY1MjY3Ii8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+edY8UQAAAGBQTFRFR0dHxMTEs7OzZGRkjIyMFxcX8/PzNzc3y8vLJiYmpKSk7u7u3Nzcc3NzDQ0NvLy8+fn5kpKSWlpaUlJSa2tre3t7ra2t4eHh1dXVm5ub5OTkhISE0dHR/Pz8AAAA////5y4GHwAAACB0Uk5T/////////////////////////////////////////wBcXBvtAAAEEElEQVR42uya55KqUAyAQ5fiAREQy8L7v+WdnV0RToEEc2DvjPkrI5/pReh3FvgAfAA+AB+AD8B/C5AldRnFgRBBHJV1ctkWID/GnSTx0dkKID0pb/+RyE83AEhr0RklqFPbANegm5XgahWghW5RoLUHkHQYEYktgKPubTqPONoBKCVre77b3tL01rq+J3lGaQPgPHl75RTjDwunmjCc+QHGvz84heoD4SlYowNYYf9jqH8mPK7wAyD7fzyTcp1Rjkw4AS4j64ZzD4YjT7kxAkTDt1ZLj3qv0sAHcMW/f0xw5QIIBSm6BiuIkAmgHsLvCwNQDOFY8wC8FIAsMw+KChAAPjnFD/nAZwGISQaYGCHmAHCeP+eErzCD0hwGgKc+RYEHKATaaoC2QNUTpELbYBEge2rzQAEY7HZ5G+BZhgJarxlgSxJgs1BJAyixuWgR4EyPgW85YbkXAZ6F0KUBuNiSCNggoHX7fYsNgyWAMCC1F4Pcnr5bvAvwm1IEcehLBbIe/XmA4l0ThP+7E+4fhrsnot1T8e7FaPdyvHtDsn9L5hB6bKUpPTAAvNpytAp42/J3BpMTC8BrNHtQR7OCBYA6nH4Nw+l9n/F82KainIa4oPAoC4rGxorGw78f99W4p24rllTIHsremg7ZQHAuKtPxQ9iVOeOq1h+vakXODbCwrM4r+ZQSNcwA2nX95WddX2oPOeDwAuAPFq+9ZsgK0CeiI0rssgKgjlayElgBls92cdNIkFG7HiDN1Y4mrWcQAv87Ng5TdxXNWoDvG0ykenLqR4bT7ZAeHlMEfx3Ab2eb6VJurZyPo3qC6gDyjgeLYW8YiW7T8706PCdjU3l0gEGJc43lVxGGoalNup0xVzRYTHtRv1quCB3A/GRDH4uN5flIAbh3xL7GGMjRUixov79ZdQVeLKIuFiDrKE0ovknUjsowM4uxvH9MEOMAPD79y1bwMAAuRwCaunp3GWAY7QnT8OJoFZinRTAboO3Z5GE2ggxwoF1+6TnxsAAQ0f+EQXPEaB7AJR2eKW4gDH4IhhTQ9MzSGJIB6J+y8DfDSP/bQK+AjB8g06tgApCzpmBji5EbAaBbd53AyUVrX9A9YUUBoxx3MQDcLXrAxAvuBoDYXghMTRzrAQ7duusMXlxNPgZ1Hxn01iRQ7yigZorKHoCnFgRQV3GOPYBcjXNQNnH4P8u8YYNEA1DxNoLzVbnSAEQ2GhFl4lecAJSC/bAJ8FDaDZCzgFUXeLW8BwUgsZ0Gp4ZOFIC7/SwwdvW7AuDRj3NrxJcLLsiFwrUL4MqWBtk4eWhVcjkOQa7FwrLIFRnkJLmRBH8PQGwLIP4eAGwLoIZhtqkKRKaW47YUm0nZrj5Y8LfqH4APwAdgb4B/AgwAYglzsiqNp0wAAAAASUVORK5CYII=";
        public void Load(IHostingEnvironment env, CoreDbContext dc)
        {
            var dm = new BundleDomainModel<UserEntity>(dc, new UserEntity
            {
                Id = "8a9fd693-9038-4083-87f7-08e45eff61d2",
                UserName = "info@yaya.ai",
                FirstName = "Yaya",
                LastName = "Bot",
                Email = "info@yaya.ai",
                Password = "Yayabot123",
                Description = "丫丫人工智能聊天机器人",
                Avatar = DEFAULT_PEOPLE_IMAGE
            });
            dm.AddEntity();

            dm = new BundleDomainModel<UserEntity>(dc, new UserEntity
            {
                Id = "265d804d-0073-4a50-bd07-98a28e10f9fb",
                UserName = "yrdrylcyp@163.com",
                FirstName = "灵溪山谷",
                Email = "yrdrylcyp@163.com",
                Password = "Yayabot123",
                Description = "鹰潭东瑞实业有限公司",
                Avatar = DEFAULT_PEOPLE_IMAGE
            });
            dm.AddEntity();
        }
    }
}
