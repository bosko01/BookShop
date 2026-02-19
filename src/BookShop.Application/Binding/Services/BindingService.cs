using BookShop.Application.Binding.Contracts;
using BookShop.Application.Binding.Contracts.Request;
using BookShop.Application.Binding.Contracts.Response;
using BookShop.Application.Common.Exceptions;
using BookShop.Application.Interfaces.Persistence;
using BookShop.Application.Interfaces.Persistence.Common;
using BindingEntity = BookShop.Domain.Entities.Binding;

namespace BookShop.Application.Binding.Services;

public sealed class BindingService : IBindingService
{
    private readonly IBindingRepository _bindingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BindingService(IBindingRepository bindingRepository, IUnitOfWork unitOfWork)
    {
        _bindingRepository = bindingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<BindingResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await _bindingRepository.GetAllAsync(cancellationToken)).Select(x => new BindingResponse(x.Id, x.Name)).ToList();

    public async Task<BindingResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var binding = await _bindingRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Binding", id);
        return new BindingResponse(binding.Id, binding.Name);
    }

    public async Task<int> CreateAsync(CreateBindingRequest request, CancellationToken cancellationToken = default)
    {
        var binding = BindingEntity.Create(request.Name);
        await _bindingRepository.AddAsync(binding, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return binding.Id;
    }

    public async Task<BindingResponse> UpdateAsync(int id, UpdateBindingRequest request, CancellationToken cancellationToken = default)
    {
        var binding = await _bindingRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Binding", id);
        binding.Rename(request.Name);
        await _unitOfWork.SaveAsync(cancellationToken);
        return new BindingResponse(binding.Id, binding.Name);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var binding = await _bindingRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Binding", id);
        _bindingRepository.Remove(binding);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
