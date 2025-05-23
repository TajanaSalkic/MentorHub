﻿namespace Backend.Models
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public string Role { get; set; }

        public bool Approved { get; set; }
    }
}
