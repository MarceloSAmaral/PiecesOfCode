using System;

namespace WebAPI.Controllers
{
    [Serializable]
    public class FullResponse
    {
        public bool successful { get; set; } = true;
        public DateTimeOffset AnswerDate { get; set; } = System.DateTimeOffset.Now;
    }
}
