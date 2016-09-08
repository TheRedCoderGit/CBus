using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBus.Json_Structs {
    class MorningMeal : IEquatable<MorningMeal> {
        public string name { get; set; }
        public bool hasSubtype { get; set; }
        public string subtypeName { get; set; }
        public List<String> subtypes { get; set; }

        public override string ToString() {
            return name;
        }

        public bool Equals(MorningMeal other) {
            return name.Equals(other.name);
        }
    }

    class NoonMeal : IEquatable<NoonMeal> {
        public string name { get; set; }
        public bool hasSubtype1 { get; set; }
        public string subtype1Name { get; set; }
        public bool hasSubtype2 { get; set; }
        public string subtype2Name { get; set; }
        public List<String> subtypes1 { get; set; }
        public List<String> subtypes2 { get; set; }

        public override string ToString() {
            return name;
        }

        public bool Equals(NoonMeal other) {
            return name.Equals(other.name);
        }
    }
}
