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
    public class OrderedFoodItem
    {
        public int QtyOrdered { get; set; }
        public double SubTotal { get; private set; }
        public FoodItem FoodItem { get; set; }

        public OrderedFoodItem(FoodItem item, int quantity)
        {
            FoodItem = item;
            QtyOrdered = quantity;
            SubTotal = CalculateSubTotal();
        }

        public double CalculateSubTotal()
        {
            return FoodItem.ItemPrice * QtyOrdered;
        }

        public override string ToString()
        {
            return $"{FoodItem.ItemName}, {QtyOrdered}";
        }
    }
}
