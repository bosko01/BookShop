using BookShop.Application.AdminAnalytics.Contracts.Response;
using BookShop.Application.Interfaces.Persistence;
using Bookstore.Domain.Enums;

namespace BookShop.Application.AdminAnalytics.Services;

public sealed class AdminAnalyticsService : IAdminAnalyticsService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBookRepository _bookRepository;

    public AdminAnalyticsService(
        IOrderRepository orderRepository,
        IBookRepository bookRepository)
    {
        _orderRepository = orderRepository;
        _bookRepository = bookRepository;
    }

    public async Task<AdminAnalyticsResponse> GetAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        var books = await _bookRepository.GetAllAsync(cancellationToken);

        var paidOrders = orders
            .Where(order => order.Status is OrderStatus.Paid or OrderStatus.Shipped or OrderStatus.Delivered)
            .ToList();

        var totalSales = paidOrders.Sum(order => order.TotalAmount);
        var customersWithPaidOrders = paidOrders
            .Select(order => order.UserId)
            .Distinct()
            .Count();

        return new AdminAnalyticsResponse(
            TotalSales: totalSales,
            Orders: paidOrders.Count,
            Books: books.Count,
            Customers: customersWithPaidOrders);
    }
}
