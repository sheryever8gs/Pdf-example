using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example1.Models
{
    public class PurchaseOrder
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> Items { get; set; }

        public static PurchaseOrder GetFake()
        {
            return new PurchaseOrder
            {
                Number = "PO123456",
                Description = "Sample Purchase Order",
                Amount = 123.45m,
                OrderDate = DateTime.Now,
                Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Number = "Item1",
                            Description = "Sample Item 1",
                            Quantity = 1,
                            Price = 12.34m
                        },
                        new OrderItem
                        {
                            Number = "Item2",
                            Description = "Sample Item 2",
                            Quantity = 2,
                            Price = 23.45m
                        }
                    }
            };
        }
    }
}
