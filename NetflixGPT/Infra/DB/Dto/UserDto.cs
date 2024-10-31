using System;
using MongoDB.Bson.Serialization.Attributes;

namespace NetflixGPT.Infra.DB.Dto
{
    public class UserDto
    {
      
        public string Username { get; set; }

      
        public string Email { get; set; }

       
        public string Password { get; set; }
    }
}

