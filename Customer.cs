//==========================================================
// Student Number : S10275174
// Student Name : Ang Zheng Yang
// Partner Name : Hayden Soh Kai Jun
// This Customer.cs is done by Hayden!
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
        public string EmailAddress { get; set; }
        public string CustomerName { get; set; }
        public List<Order> Orders { get; set; }

        public Customer(string name, string email)
        {
            CustomerName = name;
            EmailAddress = email;
            Orders = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }

        public bool RemoveOrder(Order order)
        {
            return Orders.Remove(order);
        }

        public void DisplayAllOrders()
        {
            foreach (Order o in Orders)
                System.Console.WriteLine(o);
        }

        public override string ToString()
        {
            return $"Customer Name: {CustomerName}";
        }
    }
}
