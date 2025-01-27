using System;
using System.Threading.Tasks;
using EF6Demo.Models;
using EF6Demo.Repositories;

namespace EF6Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using (var repository = new SalesTicketRepository())
                {
                    // Create sample tickets
                    Console.WriteLine("Creating sample sales tickets...");
                    await CreateSampleTickets(repository);

                    // Get all tickets
                    Console.WriteLine("\nAll sales tickets:");
                    var tickets = await repository.GetAllTicketsAsync();
                    foreach (var ticket in tickets)
                    {
                        Console.WriteLine($"Ticket: {ticket.TicketNumber} - Customer: {ticket.CustomerName} - Amount: ${ticket.NetAmount:F2} - Status: {ticket.Status}");
                    }

                    // Get tickets by status
                    Console.WriteLine("\nNew tickets:");
                    var newTickets = await repository.GetTicketsByStatusAsync("New");
                    foreach (var ticket in newTickets)
                    {
                        Console.WriteLine($"Ticket: {ticket.TicketNumber} - Customer: {ticket.CustomerName}");
                    }

                    // Update a ticket status
                    var firstTicket = await repository.GetTicketByIdAsync(1);
                    if (firstTicket != null)
                    {
                        Console.WriteLine($"\nUpdating status for ticket {firstTicket.TicketNumber}...");
                        await repository.UpdateTicketStatusAsync(firstTicket.TicketId, "InProgress");
                    }

                    // Mark a ticket as paid
                    Console.WriteLine("\nMarking ticket as paid...");
                    await repository.MarkTicketAsPaidAsync(2);

                    // Get tickets for last 7 days
                    var startDate = DateTime.UtcNow.AddDays(-7);
                    var endDate = DateTime.UtcNow;
                    Console.WriteLine($"\nTickets from {startDate:d} to {endDate:d}:");
                    var recentTickets = await repository.GetTicketsByDateRangeAsync(startDate, endDate);
                    foreach (var ticket in recentTickets)
                    {
                        Console.WriteLine($"Ticket: {ticket.TicketNumber} - Date: {ticket.CreatedDate:g} - Paid: {ticket.IsPaid}");
                    }

                    // Get total sales for the period
                    var totalSales = await repository.GetTotalSalesAsync(startDate, endDate);
                    Console.WriteLine($"\nTotal sales for the period: ${totalSales:F2}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static async Task CreateSampleTickets(SalesTicketRepository repository)
        {
            var tickets = new[]
            {
                new SalesTicket
                {
                    CustomerName = "John Smith",
                    TotalAmount = 1299.99m,
                    SalesRepresentative = "Alice Johnson",
                    Notes = "Premium package purchase",
                    TaxAmount = 78.00m
                },
                new SalesTicket
                {
                    CustomerName = "Sarah Wilson",
                    TotalAmount = 899.50m,
                    SalesRepresentative = "Bob Anderson",
                    DiscountAmount = 50.00m,
                    TaxAmount = 51.00m,
                    Notes = "First-time customer discount applied"
                },
                new SalesTicket
                {
                    CustomerName = "Michael Brown",
                    TotalAmount = 2499.99m,
                    SalesRepresentative = "Charlie Davis",
                    Notes = "Bulk order",
                    TaxAmount = 150.00m,
                    DiscountAmount = 200.00m
                }
            };

            foreach (var ticket in tickets)
            {
                await repository.CreateTicketAsync(ticket);
            }
        }
    }
} 