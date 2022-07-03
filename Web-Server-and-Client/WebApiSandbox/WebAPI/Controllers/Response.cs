using System;

namespace WebAPI.Controllers
{
    [Serializable]
    public class Response
    {
        public bool successful { get; set; } = true;
    }
}
