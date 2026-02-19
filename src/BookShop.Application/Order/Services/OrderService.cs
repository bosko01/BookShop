using BookShop.Application.Common.Exceptions;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Application.Order.Contracts;
using BookShop.Application.Order.Contracts.Request;
using BookShop.Application.Order.Contracts.Response;

namespace BookShop.Application.Order.Services;

public sealed class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<OrderResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await _orderRepository.GetAllAsync(cancellationToken)).Select(Map).ToList();

    public async Task<OrderResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id, cancellationToken) ?? throw new NotFoundException("Order", id);
        return Map(order);
    }

    public async Task<int> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        if (!await _userRepository.ExistsAsync(request.UserId, cancellationToken))
            throw new NotFoundException("User", request.UserId);

        var order = Domain.Entities.Order.Create(request.UserId);
        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return order.Id;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Order", id);
        _orderRepository.Remove(order);
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task MarkAsPaidAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Order", id);
        order.MarkAsPaid();
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task MarkAsShippedAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Order", id);
        order.MarkAsShipped();
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task MarkAsDeliveredAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Order", id);
        order.MarkAsDelivered();
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task CancelAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Order", id);
        order.Cancel();
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task ChangeItemQuantityAsync(int id, ChangeOrderItemQuantityRequest request, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id, cancellationToken) ?? throw new NotFoundException("Order", id);
        order.ChangeItemQuantity(request.OrderItemId, request.Quantity);
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    private static OrderResponse Map(Domain.Entities.Order order)
        => new(order.Id, order.UserId, order.TotalAmount, order.Status, order.CreatedAtUtc, order.Items.Count);
}
