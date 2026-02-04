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
    public class OrderedFoodItem
    {
        public FoodItem FoodItem { get; set; }
        public int QtyOrdered { get; set; }
        public double SubTotal { get; set; }

        public OrderedFoodItem(FoodItem item, int qty)
        {
            FoodItem = item;
            QtyOrdered = qty;
            CalculateSubTotal();
        }

        public double CalculateSubTotal()
        {
            SubTotal = FoodItem.ItemPrice * QtyOrdered;
            return SubTotal;
        }

        public override string ToString()
        {
            return $"{FoodItem.ItemName} - {QtyOrdered}";
        }
    }
}
