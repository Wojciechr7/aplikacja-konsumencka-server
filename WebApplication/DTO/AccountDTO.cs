using System;

namespace WebApplication.DTO
{
    public class AccountDTO
    {
        public string Token { get; set; }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
