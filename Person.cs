using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook {
    internal class Person {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public DateTime Birthday { get; set; }
        public string HomeAddress { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string ImagePath { get; set; }
        public override string ToString() {
            return Fullname;
        }
    }
    internal class MyPerson : Person {
        public string Password { get; set; }
    }
}
