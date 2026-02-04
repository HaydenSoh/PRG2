//==========================================================
// Student Number : S10275174
// Student Name : Ang Zheng Yang
// Partner Name : Hayden Soh Kai Jun
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10275174_PRG2Assignment
{
    public class Restaurant
    {
        public string RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantEmail { get; set; }

        public List<Menu> Menus { get; set; }
        public List<SpecialOffer> SpecialOffers { get; set; }

        public Restaurant(string id, string name, string email)
        {
            RestaurantId = id;
            RestaurantName = name;
            RestaurantEmail = email;
            Menus = new List<Menu>();
            SpecialOffers = new List<SpecialOffer>();
        }

        public void DisplayMenu()
        {
            foreach (Menu m in Menus)
            {
                System.Console.WriteLine(m.MenuName);
                m.DisplayFoodItems();
            }
        }

        public void AddMenu(Menu menu)
        {
            Menus.Add(menu);
        }

        public bool RemoveMenu(Menu menu)
        {
            return Menus.Remove(menu);
        }

        public void DisplayOrders()
        {
          
        }

        public void DisplaySpecialOffers()
        {
            foreach (SpecialOffer s in SpecialOffers)
                System.Console.WriteLine(s);
        }

        public override string ToString()
        {
            return $"{RestaurantId} - {RestaurantName}";
        }
    }
}
