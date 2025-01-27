using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EF6Demo.Data;
using EF6Demo.Models;

namespace EF6Demo.Repositories
{
    public class SalesTicketRepository : IDisposable
    {
        private readonly SalesContext _context;
        private bool _disposed = false;

        public SalesTicketRepository()
        {
            _context = new SalesContext();
        }

        public async Task<SalesTicket> GetTicketByIdAsync(int id)
        {
            return await _context.SalesTickets.FindAsync(id);
        }

        public async Task<SalesTicket> GetTicketByNumberAsync(string ticketNumber)
        {
            return await _context.SalesTickets
                .FirstOrDefaultAsync(t => t.TicketNumber == ticketNumber);
        }

        public async Task<IEnumerable<SalesTicket>> GetAllTicketsAsync()
        {
            return await _context.SalesTickets
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalesTicket>> GetTicketsByStatusAsync(string status)
        {
            return await _context.SalesTickets
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalesTicket>> GetTicketsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.SalesTickets
                .Where(t => t.CreatedDate >= startDate && t.CreatedDate <= endDate)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
        }

        public async Task<SalesTicket> CreateTicketAsync(SalesTicket ticket)
        {
            _context.SalesTickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<bool> UpdateTicketAsync(SalesTicket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateTicketStatusAsync(int ticketId, string newStatus)
        {
            var ticket = await GetTicketByIdAsync(ticketId);
            if (ticket == null) return false;

            ticket.Status = newStatus;
            if (newStatus == "Completed")
            {
                ticket.CompletedDate = DateTime.UtcNow;
            }

            return await UpdateTicketAsync(ticket);
        }

        public async Task<bool> MarkTicketAsPaidAsync(int ticketId)
        {
            var ticket = await GetTicketByIdAsync(ticketId);
            if (ticket == null) return false;

            ticket.IsPaid = true;
            return await UpdateTicketAsync(ticket);
        }

        public async Task<decimal> GetTotalSalesAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.SalesTickets
                .Where(t => t.CreatedDate >= startDate && 
                           t.CreatedDate <= endDate && 
                           t.Status == "Completed")
                .SumAsync(t => t.NetAmount);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
} 