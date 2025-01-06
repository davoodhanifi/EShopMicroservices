using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService (DiscountContext dbContext, 
                              ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly DiscountContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private readonly ILogger<DiscountService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _dbContext.Coupons
                                    .FirstOrDefaultAsync(i => i.ProductName == request.ProductName);
        if (coupon == null)
        {
            coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
        }

        _logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));
        }

        _dbContext.Coupons.Add(coupon);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"Discount is successfully created. ProductName : {coupon.ProductName}");

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));
        }

        _dbContext.Coupons.Update(coupon);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"Discount is successfully updated. ProductName : {coupon.ProductName}");

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
        }

        _dbContext.Coupons.Remove(coupon);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"Discount is successfully deleted. ProductName : {coupon.ProductName}");

        return new DeleteDiscountResponse { Success = true };
    }
}
