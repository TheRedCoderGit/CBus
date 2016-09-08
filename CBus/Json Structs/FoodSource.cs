using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBus.Json_Structs {

    class MorningFoodSource : IEquatable<MorningFoodSource> {
        public string name { get; set; }
        public List<MorningMeal> meals { get; set; }
        public List<string> drinks { get; set; }

        public override string ToString() {
            return name;
        }

        public bool Equals(MorningFoodSource other) {
            return name.Equals(other.name);
        }

        public static MorningFoodSource Example() {
            return new MorningFoodSource() { name = "Example Restaurant", meals = new List<MorningMeal>() { new MorningMeal() { name = "Meal1", hasSubtype = true, subtypeName = "sides", subtypes = new List<string>() { "side1", "side2" } }, new MorningMeal() { name = "Meal2", hasSubtype = true, subtypeName = "type", subtypes = new List<string>() { "type1", "type2" } }, new MorningMeal() { name = "Meal3", hasSubtype = false } }, drinks = new List<string>() { "drink1", "drink2", "drink3" } };
        }
    }

    class NoonFoodSource : IEquatable<NoonFoodSource> {
        public string name { get; set; }
        public List<NoonMeal> meals { get; set; }
        public List<string> salads { get; set; }
        public List<string> drinks { get; set; }

        public override string ToString() {
            return name;
        }

        public bool Equals(NoonFoodSource other) {
            return name.Equals(other.name);
        }

        public static NoonFoodSource Example() {
            return new NoonFoodSource() { name = "Example Restaurant", meals = new List<NoonMeal>() { new NoonMeal() { name = "Meal1", hasSubtype1 = true, subtype1Name = "types", subtypes1 = new List<string>() { "type1", "type2", "type3", "type4" }, hasSubtype2 = true, subtype2Name = "side", subtypes2 = new List<string>() { "side1", "side2" } } }, drinks = new List<string>() { "drink1", "drink2", "drink3" }, salads = new List<string>() { "salad1", "salad2", "salad3", "salad4" } };
        }
    }
}
