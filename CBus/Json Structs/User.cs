using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBus.Json_Structs {
    
    class User : IEquatable<User> {
        public string username { get; set; }
        public string name { get; set; }
        public bool canEdit { get; set; }

        public override string ToString() {
            return username;
        }

        public bool Equals(User other) {
            return username.Equals(other.username);
        }
    }
}
