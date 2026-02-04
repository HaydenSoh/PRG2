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


