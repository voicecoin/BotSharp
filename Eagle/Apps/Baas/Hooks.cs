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
    public class Hooks : IDbInitializer
    {
        public int Priority => 999;
        public const string DEFAULTIMAGE = "data:image/jpeg;base64,/9j/4AAQSkZJRgABAgAAZABkAAD/7AARRHVja3kAAQAEAAAAPAAA/+4ADkFkb2JlAGTAAAAAAf/bAIQABgQEBAUEBgUFBgkGBQYJCwgGBggLDAoKCwoKDBAMDAwMDAwQDA4PEA8ODBMTFBQTExwbGxscHx8fHx8fHx8fHwEHBwcNDA0YEBAYGhURFRofHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8f/8AAEQgALwAxAwERAAIRAQMRAf/EAI8AAAEEAwEAAAAAAAAAAAAAAAAFBgcIAgMEAQEAAgIDAQAAAAAAAAAAAAAAAAQFBgECAwcQAAIBAgQEBQMEAwAAAAAAAAECAwQFABESBiExEwdBUXEiFGEyQoGRoRVSYhYRAAEEAAIGCQUBAAAAAAAAAAEAEQIDIQQxQWESEwVRcYGhseEiMkLwkcEjFGL/2gAMAwEAAhEDEQA/ALO4EIJABJOQHEk4EJvVvcPZNFKYp7xT9ReDCNjLkfI9MPh2vluYmHED4eKXlm6o6ZBdtp3Tt27tpttxgqpBziRx1MvPQcm/jHK7KW1++JC3rvhP2kFKmF11RgQjAhYyyxQxPNKwSKNS8jscgqqMySfoMZAJLBYJbFV27gdyrluOrkpaSR6eyIxWKBSVMoH5y5c8/BeQ9eOLvy7lcKIgyxs8OpV3N5yVhYYRWmos+1NuLFTX+OquN6kjWWpoqaVYIqYSAMqO5WRmk0kEgDIY3jddc5raMNRIcnyWprrrwm5l0dC47zZqCC30+4tuVM7W9pujLHMQtTSVAGpVZkyDKwGaOMv3x1pukZGq0DeZ9kh9aQtLKwAJwOHeCpN7T9yqm7yCxXmTqV6oTR1R+6VVGbI/m4HHPxHPjzr/ADjlYrHEr9usdHkpTIZwz9EtKlDFfUojAhMnvFcpaHY1UsR0tWSR0xYc9LHUw/VUIxLclqE8wH+OKR5hPdqO3BV5pJlgq4ZnXWsUiuyeYUgkYusw4IVdiWLpydyqCpg3bW1jhnpLo/zaGoIOmSGcB10k/wCOrScJctsBpEdccCNoTOciRYTqliFsoqaeg7cXSpqlKRXirpYberDLWabqSSyDP8Rq05+eNZyE81ED4Rk/azBZiDGkk/IhuxN203Ga23SkuEBIlpZUmXL/AEYHL9eWHbqxZAxOghkvXMxkCNStspDKGHIjMY82VuRgQmb3ctM1x2PWCFS0tIyVQUeKxn3/ALIzHEpye4QzAf5YfXaks/WZVFtWKhXtxTU0+8aEVESzxxLPUCFxmrPBTySoCPH3oMWvmUiKJMWdh9yAoTKAGwPt8FmncveXUmeorhWLM/UMNXFHURq3gY1kVhHl5LkMYPLKGDR3W6CR920rP9lmsv14pX2tuq/bgrLna71VNXUFZQVTvBIB04nghaSJ4kACxlWQfaAML5rK10xjOsbsoyHa5Yv0rrRfOwmMi4IKZ1itU12vNFbYVLPVTJHw8FJ9zeirmTiSzFwrrMzqCUqrM5CI1q2YAAAHADljzdW1GBC8ZVZSrAMrDJlPEEHwOAFChDdnb2/bUvibi2ujTUcMnWjjjBd6c/krLzaPLMZ+XA+ZtuT5lXmK+FdhI4dfmoPMZSdUt+vR4Jtz7p2fUytPU7SiFRIdUxgrJ4Yyx5lYwGC+gxIRyt8QwtLbYg96VN1ZxMO8rZT7lSaCe1bS26tDWXJDBUTRyTVdQ0LfdGhf7Fb8shyxrLLMRO6zejHHQIh/ysi5wY1xYntKk3td21bbym63QK13mXTHEDqECNzGY4Fz4kegxX+bc043oh7B3+SlMjk+H6pe7wUh4hFIowIRgQjAhNy8Dt0alheP6j5Wfu+WaYS5/XX7sPU/1N+viNs3mS1nBf1br7WShY/+a6Lf0Xwujw1fB6Wn6Z9LhjjfxX/ZvP8A6f8AK6VbjehuxKeF11RgQjAhf//Z";
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
                Avatar = DEFAULTIMAGE
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
                Avatar = DEFAULTIMAGE
            });
            dm.AddEntity();
        }
    }
}
