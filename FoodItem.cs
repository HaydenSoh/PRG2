//==========================================================
// Student Number : S10275174
// Student Name : Hayden Soh Kai Jun
// Partner Name : Ang Zheng Yang
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10268816_PRG2Assignment
{
    public class FoodItem
    {
        public string ItemName { get; set; }
        public string Description { get; set; }
        public double ItemPrice { get; set; }

        public FoodItem(string name, string description, double price)
        {
            ItemName = name;
            Description = description;
            ItemPrice = price;
        }

        public override string ToString()
        {
            return $"{ItemName} - ${ItemPrice:F2}";
        }
    }
}

