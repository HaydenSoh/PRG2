//==========================================================
// Student Number : S10275174
// Student Name : Ang Zheng Yang
// Partner Name : Hayden Soh Kai Jun
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using S10275174_PRG2Assignment;

class Program
{
    static List<Restaurant> restaurants = new List<Restaurant>(); // list for restaurants
    static List<Customer> customers = new List<Customer>(); // list for customers
    static List<Order> orders = new List<Order>(); // list for orders
    static Stack<Order> refundList = new Stack<Order>(); // list for refundList

    // ==============================================
    // Start Of Program
    // By Zheng Yang, Hayden Soh
    // ==============================================
    static void Main(string[] args)
    {
        LoadRestaurants();
        LoadFoodItems();
        LoadCustomers();
        LoadOrders();
        ;

        Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
        Console.WriteLine($"{restaurants.Count} restaurants loaded!");
        Console.WriteLine($"{restaurants.Sum(r => r.FoodItems.Count)} food items loaded!");
        Console.WriteLine($"{customers.Count} customers loaded!");
        Console.WriteLine($"{orders.Count} orders loaded!");
        Console.WriteLine();

        while (true)
        {
            DisplayMenu();
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            if (choice == "0")
            {
                SaveQueueToCSV();
                SaveRefundStackToCSV();
                Console.WriteLine("Queue and refund stack saved. Goodbye!");
                break;
            }
            else if (choice == "1")
            {
                ListRestaurantsAndMenus();
            }
            else if (choice == "2")
            {
                ListAllOrders();
            }
            else if (choice == "3")
            {
                CreateNewOrder();
            }
            else if (choice == "4")
            {
                ProcessOrder();
            }
            else if (choice == "5")
            {
                ModifyExistingOrder();
            }
            else if (choice == "6")
            {
                DeleteExistingOrder();
            }
            else if (choice == "8")
            {
                DisplayTotalOrderAmount();
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }
    }


    // ==============================================
    // Main Menu
    // By Zheng Yang, Hayden Soh
    // ==============================================
    static void DisplayMenu()
    {
        Console.WriteLine("==== Gruberoo Food Delivery System ====");
        Console.WriteLine("1. List all restaurants and menu items");
        Console.WriteLine("2. List all orders");
        Console.WriteLine("3. Create a new order");
        Console.WriteLine("4. Process an order");
        Console.WriteLine("5. Modify an existing order");
        Console.WriteLine("6. Delete an existing order");
        Console.WriteLine("8. Display total order amount");
        Console.WriteLine("0. Exit");
    }

    // ==============================================
    // Basic Feature 1
    // By Zheng Yang
    // ==============================================
    static void LoadRestaurants()
    {
        string[] lines = File.ReadAllLines("restaurants.csv");
        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(',');
            restaurants.Add(new Restaurant(parts[0], parts[1], parts[2]));
        }
    }

    static void LoadFoodItems()
    {
        string[] lines = File.ReadAllLines("fooditems.csv");
        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(',');
            string restId = parts[0];

            Restaurant r = restaurants.FirstOrDefault(x => x.RestaurantId == restId);
            if (r != null)
            {
                FoodItem item = new FoodItem(
                    parts[1],
                    parts[2],
                    double.Parse(parts[3]),
                    restId
                );
                r.AddFoodItem(item);
            }
        }
    }

    // ==============================================
    // Basic Feature 2
    // By Hayden Soh
    // ==============================================
    static void LoadCustomers()
    {
        string[] lines = File.ReadAllLines("customers.csv");
        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(',');
            customers.Add(new Customer(parts[0], parts[1]));
        }
    }

    static void LoadOrders()
    {
        string[] lines = File.ReadAllLines("orders.csv");
        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(',');

            int orderId = int.Parse(parts[0]);
            Customer c = customers.First(x => x.EmailAddress == parts[1]);
            Restaurant r = restaurants.First(x => x.RestaurantId == parts[2]);

            Order o = new Order(orderId, c, r);
            o.DeliveryDateTime = DateTime.Parse($"{parts[3]} {parts[4]}");
            o.OrderTotal = double.Parse(parts[7]);
            o.Status = Enum.Parse<OrderStatus>(parts[8]);
            // ===== Delivery Address =====
            o.DeliveryAddress = parts[5];

            // ===== Ordered Items =====
            string itemsPart = string.Join(",", parts.Skip(9)).Trim('"');
            string[] items = itemsPart.Split('|');

            foreach (string item in items)
            {
                string[] itemParts = item.Split(',');

                if (itemParts.Length < 2)
                    continue;

                string itemName = itemParts[0].Trim();
                int qty = int.Parse(itemParts[1].Trim());

                FoodItem food = r.FoodItems
                    .FirstOrDefault(f => f.ItemName == itemName);

                if (food != null)
                {
                    o.AddOrderedFoodItem(new OrderedFoodItem(food, qty));
                }
            }

            orders.Add(o);
        }
    }

    // ==============================================
    // Basic Feature 3
    // By Hayden Soh
    // ==============================================
    static void ListRestaurantsAndMenus()
    {
        foreach (Restaurant r in restaurants)
        {
            Console.WriteLine($"\n{r.RestaurantName}");
            foreach (FoodItem f in r.FoodItems)
                Console.WriteLine($"- {f.ItemName}: {f.ItemDesc} -  ${f.ItemPrice:F2}");
        }
        Console.WriteLine();
    }

    // ==============================================
    // Basic Feature 4
    // By Zheng Yang
    // ==============================================
    static void ListAllOrders()
    {
        Console.WriteLine("\nAll Orders\n==========");
        Console.WriteLine("Order ID  Customer        Restaurant        Delivery Date/Time     Amount    Status");
        Console.WriteLine("--------  ----------      -------------     ------------------     ------    ---------");

        foreach (Order o in orders)
        {
            Console.WriteLine(
                $"{o.OrderId,-9} " +
                $"{o.Customer.CustomerName,-15} " +
                $"{o.Restaurant.RestaurantName,-17} " +
                $"{o.DeliveryDateTime,-22:dd/MM/yyyy HH:mm} " +
                $"${o.OrderTotal,-8:F2} " +
                $"{o.Status}"
            );
        }

        Console.WriteLine();
    }

    // ==============================================
    // Basic Feature 5
    // By Hayden Soh
    // ==============================================
    static void CreateNewOrder()
    {
        Console.WriteLine("\nCreate New Order");
        Console.WriteLine("================");

        // ---------------- Customer ----------------
        Console.Write("Enter Customer Email: ");
        string email = Console.ReadLine();

        Customer customer = customers
            .FirstOrDefault(c => c.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        // ---------------- Restaurant ----------------
        Console.Write("Enter Restaurant ID: ");
        string restId = Console.ReadLine();

        Restaurant restaurant = restaurants
            .FirstOrDefault(r => r.RestaurantId.Equals(restId, StringComparison.OrdinalIgnoreCase));

        if (restaurant == null)
        {
            Console.WriteLine("Restaurant not found.");
            return;
        }

        // ---------------- Delivery Details ----------------
        Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
        string date = Console.ReadLine();

        Console.Write("Enter Delivery Time (hh:mm): ");
        string time = Console.ReadLine();

        DateTime deliveryDateTime = DateTime.Parse($"{date} {time}");

        Console.Write("Enter Delivery Address: ");
        string address = Console.ReadLine();

        // ---------------- Create Order Object ----------------
        int newOrderId = orders.Any() ? orders.Max(o => o.OrderId) + 1 : 1000;

        Order order = new Order(newOrderId, customer, restaurant);
        order.DeliveryDateTime = deliveryDateTime;
        order.DeliveryAddress = address;

        // ---------------- Food Selection ----------------
        Console.WriteLine("\nAvailable Food Items:");

        for (int i = 0; i < restaurant.FoodItems.Count; i++)
        {
            FoodItem f = restaurant.FoodItems[i];
            Console.WriteLine($"{i + 1}. {f.ItemName} - ${f.ItemPrice:F2}");
        }

        while (true)
        {
            Console.Write("Enter item number (0 to finish): ");
            int itemChoice = int.Parse(Console.ReadLine());

            if (itemChoice == 0)
                break;

            if (itemChoice < 1 || itemChoice > restaurant.FoodItems.Count)
            {
                Console.WriteLine("Invalid item number.");
                continue;
            }

            FoodItem selectedItem = restaurant.FoodItems[itemChoice - 1];

            Console.Write("Enter quantity: ");
            int qty = int.Parse(Console.ReadLine());

            order.AddOrderedFoodItem(new OrderedFoodItem(selectedItem, qty));
        }

        // ---------------- Special Request ----------------
        Console.Write("Add special request? [Y/N]: ");
        string special = Console.ReadLine().ToUpper();

        if (special == "Y")
        {
            Console.Write("Enter special request: ");
            Console.ReadLine(); // Requirement only asks to prompt once
        }

        // ---------------- Calculate Total ----------------
        double foodTotal = order.CalculateOrderTotal();
        double deliveryFee = 5.00;
        double finalTotal = foodTotal + deliveryFee;

        order.OrderTotal = finalTotal;

        Console.WriteLine(
            $"\nOrder Total: ${foodTotal:F2} + ${deliveryFee:F2} (delivery) = ${finalTotal:F2}"
        );

        // ---------------- Payment ----------------
        Console.Write("Proceed to payment? [Y/N]: ");
        if (Console.ReadLine().ToUpper() != "Y")
            return;

        Console.Write("\nPayment method:");
        Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
        order.OrderPaymentMethod = Console.ReadLine().ToUpper();
        order.OrderPaid = true;

        // ---------------- Update Status ----------------
        order.Status = OrderStatus.Pending;
        Console.WriteLine($"Order {order.OrderId} created successfully! Status: {order.Status}");

        // ---------------- Update Collections ----------------
        orders.Add(order);
        customer.AddOrder(order);

        // ---------------- Append To CSV ----------------
        string items = string.Join("|",
            order.OrderedFoodItems.Select(i => $"{i.FoodItem.ItemName}, {i.QtyOrdered}"));

        using (StreamWriter sw = new StreamWriter("orders.csv", true))
        {
            sw.WriteLine(
                $"{order.OrderId}," +
                $"{customer.EmailAddress}," +
                $"{restaurant.RestaurantId}," +
                $"{order.DeliveryDateTime:dd/MM/yyyy}," +
                $"{order.DeliveryDateTime:HH:mm}," +
                $"{order.DeliveryAddress}," +
                $"{DateTime.Now:dd/MM/yyyy HH:mm}," +
                $"{order.OrderTotal}," +
                $"{order.Status}," +
                $"\"{items}\""
            );
        }
    }

    // ==============================================
    // Basic Feature 6
    // By Zheng Yang
    // ==============================================
    static void ProcessOrder()
    {
        Console.WriteLine("\nProcess Order");
        Console.WriteLine("=============");
        Console.Write("Enter Restaurant ID: ");
        string restId = Console.ReadLine();

        List<Order> restOrders = orders
            .Where(o => o.Restaurant.RestaurantId == restId)
            .ToList();

        if (restOrders.Count == 0)
        {
            Console.WriteLine("No orders found for this restaurant.");
            return;
        }

        foreach (Order o in restOrders)
        {
            Console.WriteLine($"\nOrder {o.OrderId}");
            Console.WriteLine($"Customer: {o.Customer.CustomerName}");
            Console.WriteLine("Ordered Items:");
            o.DisplayOrderedFoodItems();
            Console.WriteLine($"Delivery date/time: {o.DeliveryDateTime:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Total Amount: ${o.OrderTotal:F2}");
            Console.WriteLine($"Order Status: {o.Status}");

            Console.Write("\n[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
            string action = Console.ReadLine().ToUpper();

            if (action == "C")
            {
                if (o.Status == OrderStatus.Pending)
                {
                    o.Status = OrderStatus.Preparing;
                    Console.WriteLine($"Order {o.OrderId} confirmed. Status: Preparing");
                }
                else
                    Console.WriteLine("Order cannot be confirmed.");
            }
            else if (action == "R")
            {
                if (o.Status == OrderStatus.Pending)
                {
                    o.Status = OrderStatus.Rejected;
                    refundList.Push(o);
                    Console.WriteLine($"Order {o.OrderId} rejected. Refund queued.");
                }
                else
                    Console.WriteLine("Order cannot be rejected.");
            }
            else if (action == "D")
            {
                if (o.Status == OrderStatus.Preparing)
                {
                    o.Status = OrderStatus.Delivered;
                    Console.WriteLine($"Order {o.OrderId} delivered.");
                }
                else
                    Console.WriteLine("Order cannot be delivered.");
            }
            else if (action == "S")
            {
                continue;
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }
    }

    // ==============================================
    // Basic Feature 7
    // By Hayden Soh
    // ==============================================
    static void ModifyExistingOrder()
    {
        Console.WriteLine("\nModify Order");
        Console.WriteLine("============");

        // ---------------- Customer ----------------
        Console.Write("Enter Customer Email: ");
        string email = Console.ReadLine();

        var pendingOrders = orders
            .Where(o => o.Customer.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase)
                        && o.Status == OrderStatus.Pending)
            .ToList();

        if (pendingOrders.Count == 0)
        {
            Console.WriteLine("No pending orders found.");
            return;
        }

        Console.WriteLine("Pending Orders:");
        foreach (Order o in pendingOrders)
            Console.WriteLine(o.OrderId);

        // ---------------- Select Order ----------------
        Console.Write("Enter Order ID: ");
        int orderId = int.Parse(Console.ReadLine());

        Order order = pendingOrders.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null)
        {
            Console.WriteLine("Invalid Order ID.");
            return;
        }

        // ---------------- Display Order ----------------
        Console.WriteLine("\nOrder Items:");
        int index = 1;
        foreach (var item in order.OrderedFoodItems)
        {
            Console.WriteLine($"{index}. {item.FoodItem.ItemName} - {item.QtyOrdered}");
            index++;
        }

        Console.WriteLine($"Address:\n{order.DeliveryAddress}");
        Console.WriteLine($"Delivery Date/Time:\n{order.DeliveryDateTime:dd/MM/yyyy, HH:mm}");

        // ---------------- Modify Menu ----------------
        Console.Write("\nModify: [1] Items [2] Address [3] Delivery Time: ");
        string choice = Console.ReadLine();

        // ---------------- Modify Delivery Time ----------------
        if (choice == "3")
        {
            Console.Write("Enter new Delivery Time (hh:mm): ");
            string newTime = Console.ReadLine();

            DateTime newDateTime = DateTime.Parse(
                $"{order.DeliveryDateTime:dd/MM/yyyy} {newTime}"
            );

            order.DeliveryDateTime = newDateTime;

            Console.WriteLine(
                $"Order {order.OrderId} updated. New Delivery Time: {newTime}"
            );
        }
    }

    // ==============================================
    // Basic Feature 8
    // By Zheng Yang
    // ==============================================
    static void DeleteExistingOrder()
    {
        Console.WriteLine("\nDelete Order");
        Console.WriteLine("============");
        Console.Write("Enter Customer Email: ");
        string email = Console.ReadLine();

        List<Order> pendingOrders = orders
            .Where(o => o.Customer.EmailAddress == email &&
                        o.Status == OrderStatus.Pending)
            .ToList();

        if (pendingOrders.Count == 0)
        {
            Console.WriteLine("No pending orders found.");
            return;
        }

        Console.WriteLine("\nPending Orders:");
        foreach (Order o in pendingOrders)
            Console.WriteLine(o.OrderId);

        Console.Write("\nEnter Order ID: ");
        int orderId = int.Parse(Console.ReadLine());

        Order order = pendingOrders.FirstOrDefault(o => o.OrderId == orderId);

        if (order == null)
        {
            Console.WriteLine("Invalid Order ID.");
            return;
        }

        Console.WriteLine($"\nCustomer: {order.Customer.CustomerName}");
        Console.WriteLine("Ordered Items:");
        order.DisplayOrderedFoodItems();
        Console.WriteLine($"Delivery date/time: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Total Amount: ${order.OrderTotal:F2}");
        Console.WriteLine($"Order Status: {order.Status}");

        Console.Write("\nConfirm deletion? [Y/N]: ");
        string confirm = Console.ReadLine().ToUpper();

        if (confirm == "Y")
        {
            order.Status = OrderStatus.Cancelled;
            refundList.Push(order);
            Console.WriteLine($"Order {order.OrderId} cancelled. Refund of ${order.OrderTotal:F2} processed.");
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }
    }

    // ==============================================
    // Advanced Feature (a)
    // By Hayden
    // ==============================================


    // ==============================================
    // Advanced Feature (b)
    // By Zheng Yang
    // ==============================================
    static void DisplayTotalOrderAmount()
    {
        Console.WriteLine("\nTotal Order Amount Report");
        Console.WriteLine("==========================");

        double grandTotalOrders = 0;
        double grandTotalRefunds = 0;

        foreach (Restaurant r in restaurants)
        {
            double restaurantOrders = 0;
            double restaurantRefunds = 0;

            // Delivered orders
            foreach (Order o in orders)
            {
                if (o.Restaurant == r && o.Status == OrderStatus.Delivered)
                {
                    // Remove delivery fee ($5)
                    restaurantOrders += (o.OrderTotal - 5.00);
                }

                if (o.Restaurant == r &&
                   (o.Status == OrderStatus.Cancelled || o.Status == OrderStatus.Rejected))
                {
                    restaurantRefunds += o.OrderTotal;
                }
            }

            Console.WriteLine($"\nRestaurant: {r.RestaurantName}");
            Console.WriteLine($"Total Delivered Orders: ${restaurantOrders:F2}");
            Console.WriteLine($"Total Refunds: ${restaurantRefunds:F2}");

            grandTotalOrders += restaurantOrders;
            grandTotalRefunds += restaurantRefunds;
        }

        double gruberooEarnings = grandTotalOrders - grandTotalRefunds;

        Console.WriteLine("\n========== Overall Summary ==========");
        Console.WriteLine($"Total Order Amount: ${grandTotalOrders:F2}");
        Console.WriteLine($"Total Refunds: ${grandTotalRefunds:F2}");
        Console.WriteLine($"Gruberoo Final Earnings: ${gruberooEarnings:F2}");
        Console.WriteLine();
    }

        // ==============================================
        // Save Order Queue
        // By Zheng Yang
        // ==============================================
        static void SaveQueueToCSV()
        {
            using (StreamWriter sw = new StreamWriter("queue.csv"))
            {
                sw.WriteLine("OrderId,CustomerEmail,RestaurantId,DeliveryDateTime,TotalAmount,Status");

                foreach (Order o in orders)
                {
                    sw.WriteLine(
                        $"{o.OrderId}," +
                        $"{o.Customer.EmailAddress}," +
                        $"{o.Restaurant.RestaurantId}," +
                        $"{o.DeliveryDateTime:dd/MM/yyyy HH:mm}," +
                        $"{o.OrderTotal:F2}," +
                        $"{o.Status}"
                    );
                }
            }
        }

        // ==============================================
        // Save Refund Stack
        // By Zheng Yang
        // ==============================================
        static void SaveRefundStackToCSV()
        {
            using (StreamWriter sw = new StreamWriter("stack.csv"))
            {
                sw.WriteLine("OrderId,CustomerEmail,RestaurantId,RefundAmount,Status");

                foreach (Order o in refundList)
                {
                    sw.WriteLine(
                        $"{o.OrderId}," +
                        $"{o.Customer.EmailAddress}," +
                        $"{o.Restaurant.RestaurantId}," +
                        $"{o.OrderTotal:F2}," +
                        $"{o.Status}"
                    );
                }
            }
        }
    }