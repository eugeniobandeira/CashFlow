﻿namespace CashFlow.Domain.Entities
{
    public class UserEntity
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}