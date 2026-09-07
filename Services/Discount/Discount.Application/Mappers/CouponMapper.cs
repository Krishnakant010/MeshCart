using Discount.Application.Commands;
using Discount.Application.DTOs;
using Discount.Core.Entities;
using Discount.Grpc.Protos;

namespace Discount.Application.Mappers;

public static class CouponMapper
{
    public static CouponDto ToDto(this Coupon coupon)
    {
        return new CouponDto
        (
            coupon.ProductName,
            coupon.Id,
            coupon.Description,
            coupon.Amount
        );
    }

    public static Coupon ToEntity(this CreateDiscountCommand coupon)
    {
        return new Coupon
        {
            Amount = coupon.Amount,
            ProductName = coupon.ProductName,
            Description = coupon.Description,
        };
    }

    public static Coupon ToEntity(this UpdateDiscountCommand coupon)
    {
        return new Coupon
        {
            Amount = coupon.Amount,
            ProductName = coupon.ProductName,
            Description = coupon.Description,
        };
    }

    public static CouponModel ToModel(this CouponDto coupon)
    {
        return new CouponModel
        {
            Id = coupon.Id,
            Amount = coupon.Amount,
            ProductName = coupon.ProductName,
            Description = coupon.Description,
        };
    }

    public static CreateDiscountCommand ToCreateDiscountCommand(this CouponModel coupon)
    {
        return new CreateDiscountCommand
        (
            coupon.ProductName,
            coupon.Description,
            coupon.Amount
        );
    }

    public static UpdateDiscountCommand ToUpdateDiscountCommand(this CouponModel coupon)
    {
        return new UpdateDiscountCommand
        (
            coupon.Id,
            coupon.ProductName,
            coupon.Description,
            coupon.Amount
        );
    }
}