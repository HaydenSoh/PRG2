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
        public double OrderTotal { get; private set; }
        public OrderStatus Status { get; set; }

        public DateTime DeliveryDateTime { get; set; }
        public string DeliveryAddress { get; set; }
        public string PaymentMethod { get; set; }

        private List<OrderedFoodItem> orderedItems = new List<OrderedFoodItem>();

        public Order(int id, DateTime deliveryDateTime, string address, string paymentMethod)
        {
            OrderId = id;
            DeliveryDateTime = deliveryDateTime;
            DeliveryAddress = address;
            PaymentMethod = paymentMethod;
            OrderDateTime = DateTime.Now;
            Status = OrderStatus.Pending;
        }

        public void AddOrderedFoodItem(OrderedFoodItem item)
        {
            orderedItems.Add(item);
        }

        public bool RemoveOrderedFoodItem(OrderedFoodItem item)
        {
            return orderedItems.Remove(item);
        }

        public void DisplayOrderedFoodItems()
        {
            foreach (OrderedFoodItem item in orderedItems)
            {
                System.Console.WriteLine(item);
            }
        }

        public double CalculateOrderTotal()
        {
            double total = 0;

            foreach (OrderedFoodItem item in orderedItems)
            {
                total += item.CalculateSubTotal();
            }

            double deliveryFee = 5.0;
            double platformFee = total * 0.30;

            OrderTotal = total + deliveryFee + platformFee;
            return OrderTotal;
        }

        public override string ToString()
        {
            return $"Order {OrderId} - {Status} - ${OrderTotal:F2}";
        }
    }
}
