using Discount.Application.Commands;
using Discount.Application.DTOs;
using Discount.Application.Extensions;
using Discount.Application.Mappers;
using Discount.Core.Repositories;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers;

public class CreateDiscountHandler(ICouponRepository couponRepository):IRequestHandler<CreateDiscountCommand,CouponDto>
{
    public async Task<CouponDto> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        var validationErros = new Dictionary<string, string>();
        if (string.IsNullOrEmpty(request.ProductName))
            validationErros["ProductName"] = "Productname Must not be empty";
        if (string.IsNullOrEmpty(request.Description))
            validationErros["Description"] = "Description Must not be empty";
        if ((request.Amount==null )|| request.Amount<=0)
            validationErros["Amount"] = "Amount Must not be 0 or empty";

        if (validationErros.Any()) throw GrpcErrorHelper.CreateValidationException(validationErros);
        var coupon = request.ToEntity();
        
        var created = await couponRepository.CreateDiscount(coupon);
        if (!created)
        {
            throw new RpcException(new  (StatusCode.Internal,$"Could not create discount for product {request.ProductName}"));
        }

        return coupon.ToDto();
    }
}