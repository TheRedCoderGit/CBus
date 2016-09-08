using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBus.Json_Structs {
    
    class Location : IEquatable<Location> {
        public string name { get; set; }
        public string address { get; set; }

        public override string ToString() {
            return name;
        }

        public bool Equals(Location other) {
            return name.Equals(other.name);
        }
    }
}
