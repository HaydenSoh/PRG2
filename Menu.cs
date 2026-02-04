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
    public class Menu
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }

        private List<FoodItem> foodItems = new List<FoodItem>();

        public Menu(string id, string name)
        {
            MenuId = id;
            MenuName = name;
        }

        public void AddFoodItem(FoodItem item)
        {
            foodItems.Add(item);
        }

        public bool RemoveFoodItem(FoodItem item)
        {
            return foodItems.Remove(item);
        }

        public void DisplayFoodItems()
        {
            foreach (FoodItem item in foodItems)
            {
                System.Console.WriteLine(item);
            }
        }

        public override string ToString()
        {
            return $"{MenuName}";
        }
    }
}
