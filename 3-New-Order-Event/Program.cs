using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_New_Order_Event
{
    public class OrderEventArgs
    {
        public int OrderID { get; }
        public decimal OrderPrice { get; }
        public string ClientEmail { get; }
    
    
        public OrderEventArgs(int orderID, decimal orderPrice, string clientEmail)
        {
            OrderID = orderID;
            OrderPrice = orderPrice;
            ClientEmail = clientEmail;
        }
    }

    public class Order
    {
        public event EventHandler<OrderEventArgs> NewOrderCreated;

        public void Create(int orderID,  decimal orderPrice, string clientEmail)
        {
            Console.WriteLine($"\nNew Order Created for {clientEmail}. Subscribed Services will get notified\n");

            //this is another way to invoke the event, instead of using NewOrderCreated?.Invoke()
            if (NewOrderCreated != null)
            { 
                NewOrderCreated(this, new OrderEventArgs(orderID, orderPrice, clientEmail));
            }
            //the best practice is to Invoke it in a Virtual Void funtion suing ?.Invoke()
        }

    }

    public class EmailService
    {
        public void Subscribe (Order order)
        {
            order.NewOrderCreated += Order_NewOrderCreated;
        }
        public void Unsubscribe (Order order)
        {
            order.NewOrderCreated -= Order_NewOrderCreated;
        }

        private void Order_NewOrderCreated(object sender, OrderEventArgs e)
        {
            Console.WriteLine("------------ Email Service ------------");
            Console.WriteLine($"Order ID: {e.OrderID}");
            Console.WriteLine($"Order Price: {e.OrderPrice}");
            Console.WriteLine($"Client Email: {e.ClientEmail}");
            Console.WriteLine("Email sent.");
            Console.WriteLine("---------------------------------------\n\n");
        }
    }
    
    public class SMSService
    {
        public void Subscribe(Order order)
        {
            order.NewOrderCreated += Order_NewOrderCreated;
        }
        public void Unsubscribe(Order order)
        {
            order.NewOrderCreated -= Order_NewOrderCreated;
        }

        private void Order_NewOrderCreated(object sender, OrderEventArgs e)
        {
            Console.WriteLine("------------ SMS Service ------------");
            Console.WriteLine($"Order ID: {e.OrderID}");
            Console.WriteLine($"Order Price: {e.OrderPrice}");
            Console.WriteLine($"Client Email: {e.ClientEmail}");
            Console.WriteLine("SMS sent.");
            Console.WriteLine("-------------------------------------\n\n");
        }
    }
    
    public class ShippingService
    {
        public void Subscribe(Order order)
        {
            order.NewOrderCreated += Order_NewOrderCreated;
        }
        public void Unsubscribe(Order order)
        {
            order.NewOrderCreated -= Order_NewOrderCreated;
        }

        private void Order_NewOrderCreated(object sender, OrderEventArgs e)
        {
            Console.WriteLine("------------ Shipping Service ------------");
            Console.WriteLine($"Order ID: {e.OrderID}");
            Console.WriteLine($"Order Price: {e.OrderPrice}");
            Console.WriteLine($"Client Email: {e.ClientEmail}");
            Console.WriteLine("Shipping Handled.");
            Console.WriteLine("------------------------------------------\n\n");
        }
    }
    
    public class Program
    {
        static void Main(string[] args)
        {
            // Create an order instance
            Order order = new Order();

            // Create service instances
            EmailService emailService = new EmailService();
            SMSService smsService = new SMSService();
            ShippingService shippingService = new ShippingService();

            // Subscribe services to the order event
            emailService.Subscribe(order);
            smsService.Subscribe(order);
            shippingService.Subscribe(order);

            Console.WriteLine("=== Creating First Order ===");
            order.Create(1001, 250.75m, "customer1@example.com");

            // Unsubscribe SMSService and ShippingService
            smsService.Unsubscribe(order);
            shippingService.Unsubscribe(order);

            Console.WriteLine("=== Creating Second Order (Only EmailService should respond) ===");
            order.Create(1002, 99.99m, "customer2@example.com");

            emailService.Unsubscribe(order);

            Console.WriteLine("=== Creating Third Order (No service should respond) ===");
            order.Create(1003, 499.50m, "reda@example.com");

            Console.ReadLine();
        }
    }

}
