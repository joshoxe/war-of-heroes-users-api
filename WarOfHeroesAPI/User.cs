﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WarOfHeroesAPI
{
    public class User
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Email { get; set; }

        public User(string username, string email)
        {
            Username = username;
            Email = email;
        }

        public User()
        {

        }
    }
}
