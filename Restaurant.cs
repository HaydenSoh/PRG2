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

        private List<Menu> menus = new List<Menu>();
        private List<SpecialOffer> specialOffers = new List<SpecialOffer>();
        private List<Order> orders = new List<Order>();

        public Restaurant(string id, string name, string email)
        {
            RestaurantId = id;
            RestaurantName = name;
            RestaurantEmail = email;
        }

        public void AddMenu(Menu menu)
        {
            menus.Add(menu);
        }

        public bool RemoveMenu(Menu menu)
        {
            return menus.Remove(menu);
        }

        public void DisplayMenus()
        {
            foreach (Menu menu in menus)
            {
                System.Console.WriteLine(menu);
            }
        }

        public void DisplaySpecialOffers()
        {
            foreach (SpecialOffer offer in specialOffers)
            {
                System.Console.WriteLine(offer);
            }
        }

        public void AddSpecialOffer(SpecialOffer offer)
        {
            specialOffers.Add(offer);
        }

        public void ReceiveOrder(Order order)
        {
            orders.Add(order);
        }

        public override string ToString()
        {
            return $"{RestaurantName} ({RestaurantId})";
        }
    }
}
