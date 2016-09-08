using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBus.Json_Structs {
    class Recipment : IEquatable<Recipment> {
        public string customername { get; set; }
        public MorningMealDetails morningMeal { get; set; }
        public NoonMealDetails noonMeal { get; set; }
        public string hour { get; set; }
        public Location location { get; set; }

        public override string ToString() {
            return customername;
        }

        public bool Equals(Recipment other) {
            return customername.Equals(other.customername);
        }
    }

    class MorningMealDetails {
        public string foodSourceName { get; set; }
        public string main { get; set; }
        public string side { get; set; }
        public string drink { get; set; }
    }

    class NoonMealDetails {
        public string foodSourceName { get; set; }
        public string main { get; set; }
        public string side1 { get; set; }
        public string side2 { get; set; }
        public string salad1 { get; set; }
        public string salad2 { get; set; }
        public string drink { get; set; }
    }
}
