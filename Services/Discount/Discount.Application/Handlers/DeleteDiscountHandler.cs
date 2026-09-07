using Discount.Application.Commands;
using Discount.Application.Extensions;
using Discount.Core.Repositories;
using MediatR;

namespace Discount.Application.Handlers;

public class DeleteDiscountHandler(ICouponRepository couponRepository):IRequestHandler<DeleteDiscountCommand,bool>
{
    public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        var validationErros = new Dictionary<string, string>();
        if (string.IsNullOrEmpty(request.ProductName))
            validationErros["ProductName"] = "Productname Must not be empty";
        if (validationErros.Any()) throw GrpcErrorHelper.CreateValidationException(validationErros);

        var delted = await couponRepository.DeleteDiscount(request.ProductName);
        return delted;
    }
}