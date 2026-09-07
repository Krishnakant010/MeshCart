namespace Discount.Application.DTOs;

public record CouponDto(
    string ProductName,
    int Id,
    string Description,
    int Amount
);
