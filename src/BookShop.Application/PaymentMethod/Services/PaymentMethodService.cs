using BookShop.Application.Common.Exceptions;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;
using BookShop.Application.PaymentMethod.Contracts;
using BookShop.Application.PaymentMethod.Contracts.Request;
using BookShop.Application.PaymentMethod.Contracts.Response;

namespace BookShop.Application.PaymentMethod.Services;

public sealed class PaymentMethodService : IPaymentMethodService
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository, IUnitOfWork unitOfWork)
    {
        _paymentMethodRepository = paymentMethodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<PaymentMethodResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await _paymentMethodRepository.GetAllAsync(cancellationToken)).Select(Map).ToList();

    public async Task<PaymentMethodResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("PaymentMethod", id);
        return Map(paymentMethod);
    }

    public async Task<int> CreateAsync(CreatePaymentMethodRequest request, CancellationToken cancellationToken = default)
    {
        var paymentMethod = Domain.Entities.PaymentMethod.Create(request.Name, request.Description);
        await _paymentMethodRepository.AddAsync(paymentMethod, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return paymentMethod.Id;
    }

    public async Task<PaymentMethodResponse> UpdateAsync(int id, UpdatePaymentMethodRequest request, CancellationToken cancellationToken = default)
    {
        var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("PaymentMethod", id);

        paymentMethod.Rename(request.Name);
        paymentMethod.UpdateDescription(request.Description);
        await _unitOfWork.SaveAsync(cancellationToken);
        return Map(paymentMethod);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("PaymentMethod", id);

        _paymentMethodRepository.Remove(paymentMethod);
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task ActivateAsync(int id, CancellationToken cancellationToken = default)
    {
        var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("PaymentMethod", id);
        paymentMethod.Activate();
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task DeactivateAsync(int id, CancellationToken cancellationToken = default)
    {
        var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("PaymentMethod", id);
        paymentMethod.Deactivate();
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    private static PaymentMethodResponse Map(Domain.Entities.PaymentMethod x) => new(x.Id, x.Name, x.Description, x.IsActive);
}
