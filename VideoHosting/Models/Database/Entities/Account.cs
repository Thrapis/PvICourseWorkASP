using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Database.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public DateTime CreationDate { get; set; }
        public Account() { CreationDate = DateTime.Now; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        public static Account FromJson(string jsonAccount)
        {
            return JsonConvert.DeserializeObject<Account>(jsonAccount);
        }
    }
}