using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class TestController : ControllerBase
    {
        public object Demo()
        {
            return new {Name = "test", Age = 12};
        }
    }
}
