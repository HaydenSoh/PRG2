//==========================================================
// Student Number : S10268816C
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
        public string ItemDesc { get; set; }
        public double ItemPrice { get; set; }
        public string Customise { get; set; }

        public FoodItem(string name, string desc, double price)
        {
            ItemName = name;
            ItemDesc = desc;
            ItemPrice = price;
            Customise = "";
        }

        public override string ToString()
        {
            return $"{ItemName} - ${ItemPrice:F2}";
        }
    }
}




