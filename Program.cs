//==========================================================
// Student Number : S10268816C
// Student Name : Hayden Soh Kai Jun
// Partner Name : Ang Zheng Yang
//==========================================================

using PRG2_ASG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                ModifyExistingOrder();
            }
            else if (choice == "6")
            {
                DeleteExistingOrder();
            }
            else if (choice == "7")
            {
                BulkProcessing();
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
        Console.WriteLine("7. Bulk process orders");
        Console.WriteLine("0. Exit");
    }

    // ==============================================
    // Basic Feature 1
    // By Zheng Yang
    // ==============================================
    static void LoadRestaurants()
    {
        try
        {
            if (!File.Exists("restaurants.csv"))
            {
                Console.WriteLine("Warning: restaurants.csv not found.");
                return;
            }

            string[] lines = File.ReadAllLines("restaurants.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts.Length >= 3)
                {
                    restaurants.Add(new Restaurant(parts[0], parts[1], parts[2]));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading restaurants: {ex.Message}");
        }
    }

    static void LoadFoodItems()
    {
        try
        {
            if (!File.Exists("fooditems.csv"))
            {
                Console.WriteLine("Warning: fooditems.csv not found.");
                return;
            }

            string[] lines = File.ReadAllLines("fooditems.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts.Length >= 4)
                {
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
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading food items: {ex.Message}");
        }
    }

    // ==============================================
    // Basic Feature 2
    // By Hayden Soh
    // ==============================================
    static void LoadCustomers()
    {
        try
        {
            if (!File.Exists("customers.csv"))
            {
                Console.WriteLine("Warning: customers.csv not found.");
                return;
            }

            string[] lines = File.ReadAllLines("customers.csv");
            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts.Length >= 2)
                {
                    customers.Add(new Customer(parts[0], parts[1]));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading customers: {ex.Message}");
        }
    }

    static void LoadOrders()
    {
        try
        {
            if (!File.Exists("orders.csv"))
            {
                Console.WriteLine("Warning: orders.csv not found.");
                return;
            }

            string[] lines = File.ReadAllLines("orders.csv");

            // Skip header
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                // Parse CSV line properly handling quoted fields
                List<string> parts = ParseCSVLine(line);

                if (parts.Count < 10)
                {
                    Console.WriteLine($"Warning: Skipping invalid order line (has {parts.Count} parts, needs 10): {line}");
                    continue;
                }

                int orderId = int.Parse(parts[0]);
                string customerEmail = parts[1];
                string restaurantId = parts[2];

                // Find customer and restaurant
                Customer customer = customers.FirstOrDefault(c => c.EmailAddress == customerEmail);
                Restaurant restaurant = restaurants.FirstOrDefault(r => r.RestaurantId == restaurantId);

                if (customer == null)
                {
                    Console.WriteLine($"Warning: Customer {customerEmail} not found for order {orderId}");
                    continue;
                }

                if (restaurant == null)
                {
                    Console.WriteLine($"Warning: Restaurant {restaurantId} not found for order {orderId}");
                    continue;
                }

                // Parse dates - handle different date formats
                DateTime deliveryDateTime;
                try
                {
                    deliveryDateTime = DateTime.Parse($"{parts[3]} {parts[4]}");
                }
                catch
                {
                    Console.WriteLine($"Warning: Invalid delivery date/time for order {orderId}: {parts[3]} {parts[4]}");
                    continue;
                }

                DateTime orderDateTime;
                try
                {
                    orderDateTime = DateTime.Parse(parts[6]);
                }
                catch
                {
                    Console.WriteLine($"Warning: Invalid order date for order {orderId}: {parts[6]}");
                    orderDateTime = DateTime.Now;
                }

                // Create order
                Order order = new Order(orderId, customer, restaurant);
                order.DeliveryDateTime = deliveryDateTime;
                order.DeliveryAddress = parts[5];
                order.OrderDateTime = orderDateTime;
                order.OrderTotal = double.Parse(parts[7]);
                order.Status = Enum.Parse<OrderStatus>(parts[8]);

                // Load ordered food items
                if (parts.Count > 9 && !string.IsNullOrEmpty(parts[9]))
                {
                    string itemsString = parts[9];
                    // Remove quotes if present
                    if (itemsString.StartsWith("\"") && itemsString.EndsWith("\""))
                    {
                        itemsString = itemsString.Substring(1, itemsString.Length - 2);
                    }

                    if (!string.IsNullOrEmpty(itemsString))
                    {
                        string[] itemPairs = itemsString.Split('|');
                        foreach (string itemPair in itemPairs)
                        {
                            string[] itemInfo = itemPair.Split(',');
                            if (itemInfo.Length >= 2)
                            {
                                string itemName = itemInfo[0].Trim();
                                string quantityStr = itemInfo[1].Trim();

                                if (int.TryParse(quantityStr, out int quantity))
                                {
                                    // Find the food item in restaurant's menu
                                    FoodItem foodItem = restaurant.FoodItems
                                        .FirstOrDefault(f => f.ItemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));

                                    if (foodItem != null)
                                    {
                                        order.AddOrderedFoodItem(new OrderedFoodItem(foodItem, quantity));
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Warning: Food item '{itemName}' not found in restaurant {restaurant.RestaurantName} for order {orderId}");
                                    }
                                }
                            }
                        }
                    }
                }

                orders.Add(order);
                customer.AddOrder(order);
            }

            Console.WriteLine($"Successfully loaded {orders.Count} orders.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading orders: {ex.Message}");
        }
    }

    // Helper method to parse CSV lines with quoted fields
    static List<string> ParseCSVLine(string line)
    {
        List<string> result = new List<string>();
        string currentField = "";
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char currentChar = line[i];

            if (currentChar == '"')
            {
                inQuotes = !inQuotes;
                // Don't add the quote to the field
            }
            else if (currentChar == ',' && !inQuotes)
            {
                result.Add(currentField);
                currentField = "";
            }
            else
            {
                currentField += currentChar;
            }
        }

        // Add the last field
        result.Add(currentField);
        return result;
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
            if (r.FoodItems.Count == 0)
            {
                Console.WriteLine("  No food items available.");
            }
            else
            {
                foreach (FoodItem f in r.FoodItems)
                    Console.WriteLine($"  - {f.ItemName}: {f.ItemDesc} - ${f.ItemPrice:F2}");
            }
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

        Console.Write("Enter Customer Email: ");
        string email = Console.ReadLine();

        Customer customer = customers
            .FirstOrDefault(c => c.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        Console.Write("Enter Restaurant ID: ");
        string restId = Console.ReadLine();

        Restaurant restaurant = restaurants.FirstOrDefault(r => r.RestaurantId.Equals(restId, StringComparison.OrdinalIgnoreCase));

        if (restaurant == null)
        {
            Console.WriteLine("Restaurant not found.");
            return;
        }

        Console.Write("Enter Delivery Date (dd/MM/yyyy): ");
        string date = Console.ReadLine();

        Console.Write("Enter Delivery Time (HH:mm): ");
        string time = Console.ReadLine();

        DateTime deliveryDateTime;
        try
        {
            deliveryDateTime = DateTime.Parse($"{date} {time}");
        }
        catch
        {
            Console.WriteLine("Invalid date/time format. Please use dd/MM/yyyy and HH:mm.");
            return;
        }

        Console.Write("Enter Delivery Address: ");
        string address = Console.ReadLine();

        int newOrderId = orders.Any() ? orders.Max(o => o.OrderId) + 1 : 1000;

        Order order = new Order(newOrderId, customer, restaurant);
        order.DeliveryDateTime = deliveryDateTime;
        order.DeliveryAddress = address;
        order.OrderDateTime = DateTime.Now;

        Console.WriteLine("\nAvailable Food Items:");
        if (restaurant.FoodItems.Count == 0)
        {
            Console.WriteLine("No food items available for this restaurant.");
            return;
        }

        for (int i = 0; i < restaurant.FoodItems.Count; i++)
        {
            FoodItem f = restaurant.FoodItems[i];
            Console.WriteLine($"{i + 1}. {f.ItemName} - ${f.ItemPrice:F2}");
        }

        while (true)
        {
            Console.Write("Enter item number (0 to finish): ");
            if (!int.TryParse(Console.ReadLine(), out int itemChoice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            if (itemChoice == 0)
                break;

            if (itemChoice < 1 || itemChoice > restaurant.FoodItems.Count)
            {
                Console.WriteLine("Invalid item number.");
                continue;
            }

            FoodItem selectedItem = restaurant.FoodItems[itemChoice - 1];

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty < 1)
            {
                Console.WriteLine("Invalid quantity. Please enter a positive number.");
                continue;
            }

            order.AddOrderedFoodItem(new OrderedFoodItem(selectedItem, qty));
        }

        Console.Write("Add special request? [Y/N]: ");
        string special = Console.ReadLine().ToUpper();

        if (special == "Y")
        {
            Console.Write("Enter special request: ");
            Console.ReadLine();
        }

        double foodTotal = order.CalculateOrderTotal();
        double deliveryFee = 5.00;
        double finalTotal = foodTotal + deliveryFee;

        order.OrderTotal = finalTotal;

        Console.WriteLine(
            $"\nOrder Total: ${foodTotal:F2} + ${deliveryFee:F2} (delivery) = ${finalTotal:F2}"
        );

        Console.Write("Proceed to payment? [Y/N]: ");
        if (Console.ReadLine().ToUpper() != "Y")
            return;

        Console.WriteLine("\nPayment method:");
        Console.Write("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");

        order.OrderPaymentMethod = Console.ReadLine().ToUpper();
        order.OrderPaid = true;

        order.Status = OrderStatus.Pending;

        orders.Add(order);
        customer.AddOrder(order);

        // Format items string exactly like the CSV format
        string items = "";
        if (order.OrderedFoodItems != null && order.OrderedFoodItems.Count > 0)
        {
            items = string.Join("|", order.OrderedFoodItems
                .Select(i => $"{i.FoodItem.ItemName},{i.QtyOrdered}"));
        }

        // Append to CSV file - make sure we append with proper encoding
        try
        {
            using (StreamWriter sw = new StreamWriter("orders.csv", true))
            {
                sw.WriteLine(
                    $"{order.OrderId}," +
                    $"{customer.EmailAddress}," +
                    $"{restaurant.RestaurantId}," +
                    $"{order.DeliveryDateTime:dd/MM/yyyy}," +
                    $"{order.DeliveryDateTime:HH:mm}," +
                    $"{order.DeliveryAddress}," +
                    $"{order.OrderDateTime:dd/MM/yyyy HH:mm}," +
                    $"{order.OrderTotal}," +
                    $"{order.Status}," +
                    $"\"{items}\""
                );
            }
            Console.WriteLine($"Order {order.OrderId} created successfully! Status: {order.Status}");
            Console.WriteLine($"Order saved to orders.csv");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving order to CSV: {ex.Message}");
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

        List<Order> restOrders = orders.Where(o => o.Restaurant.RestaurantId == restId).ToList();

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
                    UpdateOrderInCSV(o);
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
                    UpdateOrderInCSV(o);
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
                    UpdateOrderInCSV(o);
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

        var pendingOrders = orders.Where(o => o.Customer.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase) && o.Status == OrderStatus.Pending).ToList();

        if (pendingOrders.Count == 0)
        {
            Console.WriteLine("No pending orders found.");
            return;
        }

        Console.WriteLine("Pending Orders:");
        foreach (Order o in pendingOrders)
            Console.WriteLine($"  Order ID: {o.OrderId}");

        // ---------------- Select Order ----------------
        Console.Write("Enter Order ID: ");
        if (!int.TryParse(Console.ReadLine(), out int orderId))
        {
            Console.WriteLine("Invalid Order ID.");
            return;
        }

        Order order = pendingOrders.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null)
        {
            Console.WriteLine("Invalid Order ID.");
            return;
        }

        bool continueModifying = true;
        while (continueModifying)
        {
            // ---------------- Display Order ----------------
            Console.WriteLine("\nOrder Items:");
            if (order.OrderedFoodItems == null || order.OrderedFoodItems.Count == 0)
            {
                Console.WriteLine("No items in this order.");
            }
            else
            {
                for (int i = 0; i < order.OrderedFoodItems.Count; i++)
                {
                    var item = order.OrderedFoodItems[i];
                    Console.WriteLine($"{i + 1}. {item.FoodItem.ItemName} - {item.QtyOrdered}");
                }
            }

            Console.WriteLine($"Address:\n{order.DeliveryAddress}");
            Console.WriteLine($"Delivery Date/Time:\n{order.DeliveryDateTime:dd/MM/yyyy, HH:mm}");

            // ---------------- Modify Menu ----------------
            Console.WriteLine("\nModify: [1] Items [2] Address [3] Delivery Time [0] Finish");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            // ---------------- Modify Items ----------------
            if (choice == "1")
            {
                Console.WriteLine("\nModify Items Menu:");
                Console.WriteLine("[A] Add new item");
                Console.WriteLine("[R] Remove item");
                Console.WriteLine("[C] Change quantity");
                Console.WriteLine("[0] Back");
                Console.Write("Enter your choice: ");
                string itemChoice = Console.ReadLine().ToUpper();

                if (itemChoice == "A")
                {
                    Console.WriteLine("\nAvailable Food Items from " + order.Restaurant.RestaurantName + ":");
                    for (int i = 0; i < order.Restaurant.FoodItems.Count; i++)
                    {
                        FoodItem f = order.Restaurant.FoodItems[i];
                        Console.WriteLine($"{i + 1}. {f.ItemName} - ${f.ItemPrice:F2}");
                    }

                    Console.Write("Enter item number to add: ");
                    if (int.TryParse(Console.ReadLine(), out int itemNum))
                    {
                        if (itemNum < 1 || itemNum > order.Restaurant.FoodItems.Count)
                        {
                            Console.WriteLine("Invalid item number.");
                        }
                        else
                        {
                            FoodItem selectedItem = order.Restaurant.FoodItems[itemNum - 1];
                            Console.Write("Enter quantity: ");
                            if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
                            {
                                order.AddOrderedFoodItem(new OrderedFoodItem(selectedItem, qty));
                                Console.WriteLine($"{selectedItem.ItemName} added to order.");

                                // Recalculate total
                                double foodTotal = order.CalculateOrderTotal();
                                double deliveryFee = 5.00;
                                order.OrderTotal = foodTotal + deliveryFee;
                                Console.WriteLine($"Order total updated: ${order.OrderTotal:F2}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid quantity.");
                            }
                        }
                    }
                }
                else if (itemChoice == "R")
                {
                    if (order.OrderedFoodItems.Count == 0)
                    {
                        Console.WriteLine("No items to remove.");
                    }
                    else
                    {
                        Console.WriteLine("\nCurrent items:");
                        for (int i = 0; i < order.OrderedFoodItems.Count; i++)
                        {
                            var item = order.OrderedFoodItems[i];
                            Console.WriteLine($"{i + 1}. {item.FoodItem.ItemName} - Qty: {item.QtyOrdered}");
                        }

                        Console.Write("Enter item number to remove: ");
                        if (int.TryParse(Console.ReadLine(), out int removeNum))
                        {
                            if (removeNum < 1 || removeNum > order.OrderedFoodItems.Count)
                            {
                                Console.WriteLine("Invalid item number.");
                            }
                            else
                            {
                                string removedItemName = order.OrderedFoodItems[removeNum - 1].FoodItem.ItemName;
                                order.OrderedFoodItems.RemoveAt(removeNum - 1);
                                Console.WriteLine($"{removedItemName} removed from order.");

                                // Recalculate total
                                double foodTotal = order.CalculateOrderTotal();
                                double deliveryFee = 5.00;
                                order.OrderTotal = foodTotal + deliveryFee;
                                Console.WriteLine($"Order total updated: ${order.OrderTotal:F2}");
                            }
                        }
                    }
                }
                else if (itemChoice == "C")
                {
                    if (order.OrderedFoodItems.Count == 0)
                    {
                        Console.WriteLine("No items to modify.");
                    }
                    else
                    {
                        Console.WriteLine("\nCurrent items:");
                        for (int i = 0; i < order.OrderedFoodItems.Count; i++)
                        {
                            var item = order.OrderedFoodItems[i];
                            Console.WriteLine($"{i + 1}. {item.FoodItem.ItemName} - Qty: {item.QtyOrdered}");
                        }

                        Console.Write("Enter item number to change quantity: ");
                        if (int.TryParse(Console.ReadLine(), out int changeNum))
                        {
                            if (changeNum < 1 || changeNum > order.OrderedFoodItems.Count)
                            {
                                Console.WriteLine("Invalid item number.");
                            }
                            else
                            {
                                Console.Write("Enter new quantity: ");
                                if (int.TryParse(Console.ReadLine(), out int newQty) && newQty > 0)
                                {
                                    order.OrderedFoodItems[changeNum - 1].QtyOrdered = newQty;
                                    Console.WriteLine($"Quantity updated for {order.OrderedFoodItems[changeNum - 1].FoodItem.ItemName}.");

                                    // Recalculate total
                                    double foodTotal = order.CalculateOrderTotal();
                                    double deliveryFee = 5.00;
                                    order.OrderTotal = foodTotal + deliveryFee;
                                    Console.WriteLine($"Order total updated: ${order.OrderTotal:F2}");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid quantity.");
                                }
                            }
                        }
                    }
                }
                else if (itemChoice == "0")
                {
                    // Back to main menu
                }
                else
                {
                    Console.WriteLine("Invalid option.");
                }
            }
            // ---------------- Modify Address ----------------
            else if (choice == "2")
            {
                Console.Write("Enter new Delivery Address: ");
                string newAddress = Console.ReadLine();
                order.DeliveryAddress = newAddress;
                Console.WriteLine($"Address updated for Order {order.OrderId}.");
            }
            // ---------------- Modify Delivery Time ----------------
            else if (choice == "3")
            {
                Console.Write("Enter new Delivery Time (hh:mm): ");
                string newTime = Console.ReadLine();

                try
                {
                    DateTime newDateTime = DateTime.Parse($"{order.DeliveryDateTime:dd/MM/yyyy} {newTime}");
                    order.DeliveryDateTime = newDateTime;
                    Console.WriteLine($"Order {order.OrderId} updated. New Delivery Time: {newTime}");
                }
                catch
                {
                    Console.WriteLine("Invalid time format. Please use hh:mm.");
                }
            }
            else if (choice == "0")
            {
                continueModifying = false;
                // Save changes to CSV
                UpdateOrderInCSV(order);
                Console.WriteLine($"Modification complete for Order {order.OrderId}. Changes saved to CSV.");
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
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
            .Where(o => o.Customer.EmailAddress == email && o.Status == OrderStatus.Pending)
            .ToList();

        if (pendingOrders.Count == 0)
        {
            Console.WriteLine("No pending orders found.");
            return;
        }

        Console.WriteLine("\nPending Orders:");
        foreach (Order o in pendingOrders)
            Console.WriteLine($"  Order ID: {o.OrderId}");

        Console.Write("\nEnter Order ID: ");
        if (!int.TryParse(Console.ReadLine(), out int orderId))
        {
            Console.WriteLine("Invalid Order ID.");
            return;
        }

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
            UpdateOrderInCSV(order);
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }
    }

    // ==============================================
    // Advance Feature A
    // By Hayden Soh
    // ==============================================
    static void BulkProcessing()
    {
        Console.WriteLine("\nBulk Process Orders");
        Console.WriteLine("===================");

        // 1. Identify all orders with status "Pending"
        DateTime currentDay = DateTime.Today;
        var pendingOrders = orders
            .Where(o => o.Status == OrderStatus.Pending &&
                        o.DeliveryDateTime.Date == currentDay)
            .ToList();

        // 2. Display the total number in the Order Queues with this status
        Console.WriteLine($"Total pending orders for today ({currentDay:dd/MM/yyyy}): {pendingOrders.Count}\n");

        if (pendingOrders.Count == 0)
        {
            Console.WriteLine("No pending orders to process for today.");
            return;
        }

        // 3. For each order in the queue: attempt to process it
        int processedCount = 0;
        int preparingCount = 0;
        int rejectedCount = 0;

        Console.WriteLine("Processing orders...\n");
        Console.WriteLine("Order ID | Delivery Time | Time Until Delivery | Status Decision | Reason");
        Console.WriteLine("---------|---------------|---------------------|-----------------|--------");

        foreach (var order in pendingOrders)
        {
            TimeSpan timeUntilDelivery = order.DeliveryDateTime - DateTime.Now;
            string statusDecision;
            string reason;

            // Check if delivery time has already passed (negative time)
            if (timeUntilDelivery.TotalMinutes < 0)
            {
                // Delivery time is in the past - auto-reject
                order.Status = OrderStatus.Rejected;
                refundList.Push(order);
                rejectedCount++;
                statusDecision = "REJECTED";
                reason = $"Delivery time has passed ({Math.Abs(timeUntilDelivery.TotalMinutes):F0} minutes ago)";
            }
            // Check if delivery time is WITHIN 1 hour from now (less than 1 hour)
            else if (timeUntilDelivery.TotalHours < 1)
            {
                // Automatically set status to "Preparing" if delivery is WITHIN 1 hour
                order.Status = OrderStatus.Preparing;
                preparingCount++;
                statusDecision = "PREPARING";
                reason = $"Delivery in {timeUntilDelivery.TotalMinutes:F0} minutes (within 1 hour)";
            }
            else
            {
                // Otherwise (more than 1 hour away) automatically set status to "Rejected"
                order.Status = OrderStatus.Rejected;
                refundList.Push(order);
                rejectedCount++;
                statusDecision = "REJECTED";
                reason = $"Delivery in {timeUntilDelivery.TotalHours:F1} hours (> 1 hour)";
            }

            processedCount++;
            Console.WriteLine($"{order.OrderId,-8} | {order.DeliveryDateTime:HH:mm} | " +
                             $"{timeUntilDelivery.TotalMinutes,6:F0} min | " +
                             $"{statusDecision,-15} | {reason}");

            // Update CSV after processing each order
            UpdateOrderInCSV(order);
        }

        // 4. Display summary statistics
        Console.WriteLine("\n" + new string('=', 70));
        Console.WriteLine("SUMMARY STATISTICS");
        Console.WriteLine(new string('=', 70));
        Console.WriteLine($"Total orders processed: {processedCount}");
        Console.WriteLine($"Orders set to 'Preparing': {preparingCount}");
        Console.WriteLine($"Orders set to 'Rejected': {rejectedCount}");

        // Calculate percentage of automatically processed orders against all orders
        if (orders.Count > 0)
        {
            double percentage = (double)processedCount / orders.Count * 100;
            Console.WriteLine($"Percentage of automatically processed orders against all orders: {percentage:F1}%");
        }
        else
        {
            Console.WriteLine("Percentage: N/A (no orders in system)");
        }

        Console.WriteLine("All changes have been saved to orders.csv");
        Console.WriteLine(new string('=', 70) + "\n");
    }

    // ==============================================
    // Helper function to update a single order in CSV
    // ==============================================
    static void UpdateOrderInCSV(Order orderToUpdate)
    {
        try
        {
            // Read all lines from CSV
            string[] lines = File.ReadAllLines("orders.csv");

            // Find and update the specific order
            for (int i = 1; i < lines.Length; i++) // Start from 1 to skip header
            {
                string line = lines[i];
                List<string> parts = ParseCSVLine(line);

                if (parts.Count >= 1 && int.Parse(parts[0]) == orderToUpdate.OrderId)
                {
                    // Format items string
                    string items = "";
                    if (orderToUpdate.OrderedFoodItems != null && orderToUpdate.OrderedFoodItems.Count > 0)
                    {
                        items = string.Join("|", orderToUpdate.OrderedFoodItems
                            .Select(i => $"{i.FoodItem.ItemName},{i.QtyOrdered}"));
                    }

                    // Update the line
                    lines[i] = $"{orderToUpdate.OrderId}," +
                              $"{orderToUpdate.Customer.EmailAddress}," +
                              $"{orderToUpdate.Restaurant.RestaurantId}," +
                              $"{orderToUpdate.DeliveryDateTime:dd/MM/yyyy}," +
                              $"{orderToUpdate.DeliveryDateTime:HH:mm}," +
                              $"{orderToUpdate.DeliveryAddress}," +
                              $"{orderToUpdate.OrderDateTime:dd/MM/yyyy HH:mm}," +
                              $"{orderToUpdate.OrderTotal}," +
                              $"{orderToUpdate.Status}," +
                              $"\"{items}\"";
                    break;
                }
            }

            // Write all lines back to the file
            File.WriteAllLines("orders.csv", lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating order {orderToUpdate.OrderId} in CSV: {ex.Message}");
        }
    }

    // ==============================================
    // Helper function to update ALL orders in CSV (for bulk operations)
    // ==============================================
    static void UpdateAllOrdersInCSV()
    {
        try
        {
            List<string> lines = new List<string>();
            // Add header
            lines.Add("OrderId,CustomerEmail,RestaurantId,DeliveryDate,DeliveryTime,DeliveryAddress,CreatedDateTime,TotalAmount,Status,Items");

            // Add all orders
            foreach (var order in orders)
            {
                // Format items string
                string items = "";
                if (order.OrderedFoodItems != null && order.OrderedFoodItems.Count > 0)
                {
                    items = string.Join("|", order.OrderedFoodItems
                        .Select(i => $"{i.FoodItem.ItemName},{i.QtyOrdered}"));
                }

                lines.Add(
                    $"{order.OrderId}," +
                    $"{order.Customer.EmailAddress}," +
                    $"{order.Restaurant.RestaurantId}," +
                    $"{order.DeliveryDateTime:dd/MM/yyyy}," +
                    $"{order.DeliveryDateTime:HH:mm}," +
                    $"{order.DeliveryAddress}," +
                    $"{order.OrderDateTime:dd/MM/yyyy HH:mm}," +
                    $"{order.OrderTotal}," +
                    $"{order.Status}," +
                    $"\"{items}\""
                );
            }

            // Write all lines back to the file
            File.WriteAllLines("orders.csv", lines);
            Console.WriteLine("All orders saved to orders.csv");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving all orders to CSV: {ex.Message}");
        }
    }
}
