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
    public class Customer
    {
        public string CustomerName { get; set; }
        public string EmailAddress { get; set; }

        private List<Order> orders = new List<Order>();

        public Customer(string name, string email)
        {
            CustomerName = name;
            EmailAddress = email;
        }

        public void AddOrder(Order order)
        {
            orders.Add(order);
        }

        public bool RemoveOrder(Order order)
        {
            return orders.Remove(order);
        }

        public void DisplayAllOrders()
        {
            foreach (Order order in orders)
            {
                System.Console.WriteLine(order);
            }
        }

        public override string ToString()
        {
            return $"{CustomerName} ({EmailAddress})";
        }
    }
}
