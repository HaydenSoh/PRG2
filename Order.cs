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
    public enum OrderStatus
    {
        Pending,
        Preparing,
        Delivered,
        Cancelled,
        Rejected,
        Archived
    }

    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public double OrderTotal { get; set; }
        public OrderStatus Status { get; set; }

        public DateTime DeliveryDateTime { get; set; }
        public string DeliveryAddress { get; set; }
        public string OrderPaymentMethod { get; set; }
        public bool OrderPaid { get; set; }

        public Customer Customer { get; set; }
        public Restaurant Restaurant { get; set; }

        public List<OrderedFoodItem> OrderedFoodItems { get; set; }

        public Order(int id, Customer cust, Restaurant rest)
        {
            OrderId = id;
            Customer = cust;
            Restaurant = rest;
            OrderDateTime = DateTime.Now;
            Status = OrderStatus.Pending;
            OrderedFoodItems = new List<OrderedFoodItem>();
        }

        public void AddOrderedFoodItem(OrderedFoodItem item)
        {
            OrderedFoodItems.Add(item);
        }

        public bool RemoveOrderedFoodItem(OrderedFoodItem item)
        {
            return OrderedFoodItems.Remove(item);
        }

        public void DisplayOrderedFoodItems()
        {
            foreach (OrderedFoodItem i in OrderedFoodItems)
                System.Console.WriteLine(i);
        }

        public double CalculateOrderTotal()
        {
            double total = 0;
            foreach (OrderedFoodItem i in OrderedFoodItems)
                total += i.CalculateSubTotal();

            total += 5.00;          // delivery fee
            total += total * 0.30;  // platform fee

            OrderTotal = total;
            return total;
        }

        public override string ToString()
        {
            return $"Order {OrderId} - {Status} - ${OrderTotal:F2}";
        }
    }
}
