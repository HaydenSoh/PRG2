//==========================================================
// Student Number : S10275174
// Student Name : Ang Zheng Yang
// Partner Name : Hayden Soh Kai Jun
// This FoodItem.cs is done by Hayden!
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10275174_PRG2Assignment
{
    public class FoodItem
    {
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public double ItemPrice { get; set; }
        public string RestaurantId { get; set; }

        public FoodItem(string name, string desc, double price, string restaurantId)
        {
            ItemName = name;
            ItemDesc = desc;
            ItemPrice = price;
            RestaurantId = restaurantId;
        }

        public override string ToString()
        {
            return $"Item Name: {ItemName}, Item Price: ${ItemPrice:F2}";
        }
    }
}
