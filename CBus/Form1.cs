using CBus.Json_Structs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CBus
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<User> users;
        List<Location> locations;
        List<MorningFoodSource> morningFoodSources;
        List<NoonFoodSource> noonFoodSources;

        private void Form1_Load(object sender, EventArgs e) {
            string[] dateString = System.DateTime.Now.ToShortDateString().Split("/".ToCharArray());
            for (int i = 0; i < dateString.Length; i++)
                if (dateString[i].Length < 2)
                    dateString[i] = "0" + dateString[i];
            date.Text = string.Join(".", dateString);

            if (!File.Exists("users.json")) {
                User exUser = new User { username = "username", name = "name", canEdit = false };
                List<User> exUserList = new List<User>() { exUser };
                File.WriteAllText("users.json", JsonConvert.SerializeObject(exUserList, Formatting.Indented));
            }
            String usersString = File.ReadAllText("users.json");
            try {
                users = JsonConvert.DeserializeObject<List<User>>(usersString);
            } catch {
                MessageBox.Show("!אירעה שגיאה בעת טעינת רשימת המשתמשים\r\n.משתמש בקובץ ברירת המחדל", "!שגיאה");
                User exUser = new User { username = "username", name = "name", canEdit = false };
                List<User> exUserList = new List<User>() { exUser };
                File.WriteAllText("users.json", JsonConvert.SerializeObject(exUserList, Formatting.Indented));
                users = exUserList;
            }
            username.Items.AddRange(users.ToArray());

            foreach (User u in username.Items)
                if (u.username.Equals(Environment.UserName)) {
                    username.SelectedItem = u;
                    break;
                }
            string name = null;
            if(users == null)
                users = new List<User>();
            User user = null;
            foreach (User u in users)
                if (u.Equals(username.SelectedItem)) {
                    name = u.name;
                    user = u;
                }
            if (string.IsNullOrEmpty(name)) {
                MessageBox.Show(this, new string("!אתה לא רשום במערכת\r\n.פנה לאחראי כדי להשתמש במערכת".ToCharArray()), "!שגיאה", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                Application.Exit();
                return;
            }
            if (System.DateTime.Now.Hour > 10 && !user.canEdit) {
                MessageBox.Show(this, new string("!אין אפשרות לבצע הזמנה אחרי השעה 10:00".ToCharArray()), "!שגיאה", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                Application.Exit();
                return;
            }
            if (user.canEdit) {
                username.Enabled = true;
                //username.BackColor = System.Drawing.SystemColors.ControlLightLight;
                //label4.BackColor = System.Drawing.SystemColors.ControlLightLight;
                generateText.Visible = true;
            }

            if (!File.Exists("locations.json")) {
                Location exLocation = new Location { name = "name", address = "address" };
                List<Location> exLocationList = new List<Location>() { exLocation };
                File.WriteAllText("locations.json", JsonConvert.SerializeObject(exLocationList, Formatting.Indented));
            }
            String locationsString = File.ReadAllText("locations.json");
            try {
                locations = JsonConvert.DeserializeObject<List<Location>>(locationsString);
            } catch {
                MessageBox.Show("!אירעה שגיאה בעת טעינת רשימת המקומות\r\n.משתמש בקובץ ברירת המחדל", "!שגיאה");
                Location exLocation = new Location { name = "name", address = "address" };
                List<Location> exLocationList = new List<Location>() { exLocation };
                File.WriteAllText("locations.json", JsonConvert.SerializeObject(exLocationList, Formatting.Indented));
                locations = exLocationList;
            }
            location.Items.AddRange(locations.ToArray());
            location.SelectedIndex = 0;

            if (!Directory.Exists("restaurants"))
                Directory.CreateDirectory("restaurants");
            if (!Directory.Exists("restaurants/morning"))
                Directory.CreateDirectory("restaurants/morning");
            if (!Directory.Exists("restaurants/noon"))
                Directory.CreateDirectory("restaurants/noon");
            if (!Directory.Exists("recipments"))
                Directory.CreateDirectory("recipments");
            if (!Directory.Exists("recipments/" + DateTime.Now.Year))
                Directory.CreateDirectory("recipments/" + DateTime.Now.Year);
            if (!Directory.Exists("recipments/" + DateTime.Now.Year + "/" + DateTime.Now.Month))
                Directory.CreateDirectory("recipments/" + DateTime.Now.Year + "/" + DateTime.Now.Month);

            List<String> morningRestaurants = new List<string>(Directory.GetFiles("restaurants/morning"));
            if (morningRestaurants.ToArray().Length == 0) {
                MorningFoodSource exFoodSource = MorningFoodSource.Example();
                File.WriteAllText("restaurants/morning/foodsource1.json", JsonConvert.SerializeObject(exFoodSource, Formatting.Indented));
            }
            morningRestaurants = new List<string>(Directory.GetFiles("restaurants/morning"));
            morningFoodSources = new List<MorningFoodSource>();
            morningFoodSource.SelectedIndex = 0;
            //foodSources.Add(new FoodSource { name = "בחר ספק" });
            foreach (string file in morningRestaurants) {
                String foodsourceString = File.ReadAllText(file);
                morningFoodSources.Add(JsonConvert.DeserializeObject<MorningFoodSource>(foodsourceString));
            }
            foreach (MorningFoodSource fs in morningFoodSources)
                morningFoodSource.Items.Add(fs);

            List<String> noonRestaurants = new List<string>(Directory.GetFiles("restaurants/noon"));
            if (noonRestaurants.ToArray().Length == 0) {
                NoonFoodSource exFoodSource = NoonFoodSource.Example();
                File.WriteAllText("restaurants/noon/foodsource1.json", JsonConvert.SerializeObject(exFoodSource, Formatting.Indented));
            }
            noonRestaurants = new List<string>(Directory.GetFiles("restaurants/noon"));
            noonFoodSources = new List<NoonFoodSource>();
            noonFoodSource.SelectedIndex = 0;
            //foodSources.Add(new FoodSource { name = "בחר ספק" });
            foreach (string file in noonRestaurants) {
                String foodsourceString = File.ReadAllText(file);
                noonFoodSources.Add(JsonConvert.DeserializeObject<NoonFoodSource>(foodsourceString));
            }
            foreach (NoonFoodSource fs in noonFoodSources)
                noonFoodSource.Items.Add(fs);

            hour.SelectedIndex = 0;
        }

        private void morningFoodSource_SelectedIndexChanged(object sender, EventArgs e) {
            morningDrink.Visible = false;
            morningDrinkLabel.Visible = false;
            morningSide.Visible = false;
            morningSideLabel.Visible = false;
            morningMain.Visible = false;
            morningMainLabel.Visible = false;
            if (morningFoodSource.SelectedIndex != 0) {
                morningDrink.Items.Clear();
                MorningFoodSource fs = (MorningFoodSource)morningFoodSource.SelectedItem;
                foreach (string drink in fs.drinks)
                    morningDrink.Items.Add(drink);
                if (fs.drinks.ToArray().Length > 0) {
                    morningDrink.Visible = true;
                    morningDrinkLabel.Visible = true;
                    morningDrink.SelectedIndex = 0;
                }

                morningMain.Items.Clear();
                foreach (MorningMeal meal in fs.meals)
                    morningMain.Items.Add(meal);
                if (fs.meals.ToArray().Length > 0) {
                    morningMain.Visible = true;
                    morningMainLabel.Visible = true;
                    morningMain.SelectedIndex = 0;

                    morningSide.Items.Clear();
                    MorningMeal m = (MorningMeal)morningMain.SelectedItem;
                    if (m.hasSubtype) {
                        morningSide.Visible = true;
                        morningSideLabel.Visible = true;
                        morningSideLabel.Text = m.subtypeName;
                        foreach (string side in m.subtypes)
                            morningSide.Items.Add(side);
                        morningSide.SelectedIndex = 0;
                    } else {
                        morningSide.Visible = false;
                        morningSideLabel.Visible = false;
                    }
                }
            }
        }

        private void morningMain_SelectedIndexChanged(object sender, EventArgs e) {
            morningSide.Items.Clear();
            MorningMeal m = (MorningMeal)morningMain.SelectedItem;
            if (m.hasSubtype) {
                morningSide.Visible = true;
                morningSideLabel.Visible = true;
                morningSideLabel.Text = m.subtypeName;
                foreach (string side in m.subtypes)
                    morningSide.Items.Add(side);
                morningSide.SelectedIndex = 0;
            } else {
                morningSide.Visible = false;
                morningSideLabel.Visible = false;
            }
        }

        private void noonFoodSource_SelectedIndexChanged(object sender, EventArgs e) {
            noonDrink.Visible = false;
            noonDrinkLabel.Visible = false;
            noonSide1.Visible = false;
            noonSide2.Visible = false;
            noonSide1Label.Visible = false;
            noonSide2Label.Visible = false;
            noonSalad1.Visible = false;
            noonSalad2.Visible = false;
            noonSalad1Label.Visible = false;
            noonSalad2Label.Visible = false;
            noonMain.Visible = false;
            noonMainLabel.Visible = false;
            if (noonFoodSource.SelectedIndex != 0) {
                noonDrink.Items.Clear();
                NoonFoodSource fs = (NoonFoodSource)noonFoodSource.SelectedItem;
                foreach (string drink in fs.drinks)
                    noonDrink.Items.Add(drink);
                if (fs.drinks.ToArray().Length > 0) {
                    noonDrink.Visible = true;
                    noonDrinkLabel.Visible = true;
                    noonDrink.SelectedIndex = 0;
                }

                noonMain.Items.Clear();
                foreach (NoonMeal meal in fs.meals)
                    noonMain.Items.Add(meal);
                if (fs.meals.ToArray().Length > 0) {
                    noonMain.Visible = true;
                    noonMainLabel.Visible = true;
                    noonMain.SelectedIndex = 0;

                    noonSide1.Items.Clear();
                    NoonMeal m = (NoonMeal)noonMain.SelectedItem;
                    if (m.hasSubtype1) {
                        noonSide1.Visible = true;
                        noonSide1Label.Visible = true;
                        noonSide1Label.Text = m.subtype1Name;
                        foreach (string side in m.subtypes1)
                            noonSide1.Items.Add(side);
                        noonSide1.SelectedIndex = 0;
                    } else {
                        noonSide1.Visible = false;
                        noonSide1Label.Visible = false;
                    }
                    noonSide2.Items.Clear();
                    if (m.hasSubtype2) {
                        noonSide2.Visible = true;
                        noonSide2Label.Visible = true;
                        noonSide2Label.Text = m.subtype2Name;
                        foreach (string side in m.subtypes2)
                            noonSide2.Items.Add(side);
                        noonSide2.SelectedIndex = 0;
                    } else {
                        noonSide2.Visible = false;
                        noonSide2Label.Visible = false;
                    }
                }

                noonSalad1.Items.Clear();
                foreach (string salad in fs.salads)
                    noonSalad1.Items.Add(salad);
                if (fs.salads.ToArray().Length > 0) {
                    noonSalad1.Visible = true;
                    noonSalad1Label.Visible = true;
                    noonSalad1.SelectedIndex = 0;

                    if (fs.salads.ToArray().Length > 1) {
                        foreach (string salad in fs.salads)
                            if(!salad.Equals(noonSalad1.SelectedItem))
                                noonSalad2.Items.Add(salad);
                        noonSalad2.Visible = true;
                        noonSalad2Label.Visible = true;
                        noonSalad2.SelectedIndex = 0;

                        noonSalad1.Items.Remove(noonSalad2.SelectedItem);
                    }
                }
            }
        }

        bool fromSalad1 = false;
        bool fromSalad2 = false;
        private void noonSalad1_SelectedIndexChanged(object sender, EventArgs e) {
            if (fromSalad2) {
                fromSalad2 = false;
                return;
            }
            if (!noonSalad2.Visible)
                return;
            String noonSalad2Selected = (string)noonSalad2.SelectedItem;
            NoonFoodSource fs = (NoonFoodSource)noonFoodSource.SelectedItem;
            noonSalad2.Items.Clear();
            foreach (string salad in fs.salads)
                if (!salad.Equals(noonSalad1.SelectedItem))
                    noonSalad2.Items.Add(salad);
            fromSalad1 = true;
            if (noonSalad2.Items.Contains(noonSalad2Selected))
                noonSalad2.SelectedItem = noonSalad2Selected;
            else
                noonSalad2.SelectedIndex = 0;

            noonSalad1.Items.Remove(noonSalad2.SelectedItem);
        }

        private void noonSalad2_SelectedIndexChanged(object sender, EventArgs e) {
            if (fromSalad1) {
                fromSalad1 = false;
                return;
            }
            if (!noonSalad1.Visible)
                return;
            String noonSalad1Selected = (string)noonSalad1.SelectedItem;
            NoonFoodSource fs = (NoonFoodSource)noonFoodSource.SelectedItem;
            noonSalad1.Items.Clear();
            foreach (string salad in fs.salads)
                if (!salad.Equals(noonSalad2.SelectedItem))
                    noonSalad1.Items.Add(salad);
            fromSalad2 = true;
            if (noonSalad1.Items.Contains(noonSalad1Selected))
                noonSalad1.SelectedItem = noonSalad1Selected;
            else
                noonSalad1.SelectedIndex = 0;

            noonSalad2.Items.Remove(noonSalad1.SelectedItem);
        }

        private void noonMain_SelectedIndexChanged(object sender, EventArgs e) {
            noonSide1.Visible = false;
            noonSide2.Visible = false;
            noonSide1Label.Visible = false;
            noonSide2Label.Visible = false;
            noonSide1.Items.Clear();
            NoonMeal m = (NoonMeal)noonMain.SelectedItem;
            if (m.hasSubtype1) {
                noonSide1.Visible = true;
                noonSide1Label.Visible = true;
                noonSide1Label.Text = m.subtype1Name;
                foreach (string side in m.subtypes1)
                    noonSide1.Items.Add(side);
                noonSide1.SelectedIndex = 0;
            } else {
                noonSide1.Visible = false;
                noonSide1Label.Visible = false;
            }
            noonSide2.Items.Clear();
            if (m.hasSubtype2) {
                noonSide2.Visible = true;
                noonSide2Label.Visible = true;
                noonSide2Label.Text = m.subtype2Name;
                foreach (string side in m.subtypes2)
                    noonSide2.Items.Add(side);
                noonSide2.SelectedIndex = 0;
            } else {
                noonSide2.Visible = false;
                noonSide2Label.Visible = false;
            }
        }

        private void username_SelectedIndexChanged(object sender, EventArgs e) {
            morningFoodSource.SelectedIndex = 0;
            noonFoodSource.SelectedIndex = 0;
        }

        private void save_Click(object sender, EventArgs e) {
            if (!File.Exists("recipments/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + ".json")) {
                Recipment exRecipment = new Recipment { customername = "customer name"  };
                List<Recipment> exLocationList = new List<Recipment>() { exRecipment };
                File.WriteAllText("recipments/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + ".json", JsonConvert.SerializeObject(exLocationList, Formatting.Indented));
            }
            Recipment save = new Recipment { customername = ((User)username.SelectedItem).username };
            string filePath = "recipments/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + ".json";
            List<Recipment> recipmentsList;
            string recipmentsString = File.ReadAllText(filePath);
            try {
                recipmentsList = JsonConvert.DeserializeObject<List<Recipment>>(recipmentsString);
            } catch {
                recipmentsList = new List<Recipment>();
            }

            foreach(Recipment r in recipmentsList);
            try {
                //locations = JsonConvert.DeserializeObject<List<Location>>(locationsString);
            } catch {
                MessageBox.Show("!אירעה שגיאה בעת טעינת רשימת המקומות\r\n.משתמש בקובץ ברירת המחדל", "!שגיאה");
                Location exLocation = new Location { name = "name", address = "address" };
                List<Location> exLocationList = new List<Location>() { exLocation };
                File.WriteAllText("locations.json", JsonConvert.SerializeObject(exLocationList, Formatting.Indented));
                locations = exLocationList;
            }
            location.Items.AddRange(locations.ToArray());
            location.SelectedIndex = 0;
        }
    }
}
