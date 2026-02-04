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
        public List<FoodItem> FoodItems { get; set; }

        public Menu(string id, string name)
        {
            MenuId = id;
            MenuName = name;
            FoodItems = new List<FoodItem>();
        }

        public void AddFoodItem(FoodItem food)
        {
            FoodItems.Add(food);
        }

        public bool RemoveFoodItem(FoodItem food)
        {
            return FoodItems.Remove(food);
        }

        public void DisplayFoodItems()
        {
            foreach (FoodItem f in FoodItems)
                System.Console.WriteLine(f);
        }

        public override string ToString()
        {
            return MenuName;
        }
    }
}
