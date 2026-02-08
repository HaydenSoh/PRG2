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
            //        ModifyExistingOrder();
                }
                else if (choice == "6")
                {
                    DeleteExistingOrder();
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
            Console.WriteLine("Create New Order");
            Console.WriteLine("================");
            Console.WriteLine("Enter Customer Email: ");
            string Email = Console.ReadLine();
            Console.WriteLine("Enter Restaurant ID: ");
            string ID = Console.ReadLine();
            Console.WriteLine("Enter Delivery Date(dd/ mm / yyyy): ");

            Console.WriteLine("Enter Delivery Time(hh: mm): ");

            Console.WriteLine("Enter Delivery Address: ");
            string Address = Console.ReadLine();
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
    }