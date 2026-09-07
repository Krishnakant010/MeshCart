using Discount.Application.DTOs;
using Discount.Application.Extensions;
using Discount.Application.Mappers;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers;

public class GetDiscountQueryHandler(ICouponRepository couponRepository ):IRequestHandler<GetDiscountQuery,CouponDto>
{
    public async Task<CouponDto> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.productName))
        {
            var validationErrors = new Dictionary<string, string>()
            {
                { "ProductName", "ProductName Must not be Empty" }
            };
            throw  GrpcErrorHelper.CreateValidationException(validationErrors);
        }


        var coupon = await couponRepository.GetDiscount(request.productName);
        return coupon == null ? throw new RpcException( new Status(StatusCode.Internal,$"Could not create discount for product: {request.productName}")) : coupon.ToDto();
    }
}