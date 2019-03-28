using System;

namespace WebApplication.DTO
{
    public class MessagesDTO
    {
        public Guid Sender { get; set; }
        public string Contents { get; set; }
        public DateTime Date { get; set; }
    }
}
