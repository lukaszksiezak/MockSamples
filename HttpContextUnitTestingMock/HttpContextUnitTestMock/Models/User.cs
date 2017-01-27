using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpContextUnitTestMock.Models
    {
    [Serializable]
    public class User
        {
        public int UsersID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BirthDate { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public string AddressCountry { get; set; }
        public string Company { get; set; }
        public string StartedWorking { get; set; }
        }
    }