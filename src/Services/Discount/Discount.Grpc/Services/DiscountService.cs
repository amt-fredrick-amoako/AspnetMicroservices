using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase //inherit from DiscountProtoService's base in discount.proto file
    {
        private readonly IDiscountRepository _repository; //get discount data with repository obj
        private readonly ILogger<DiscountService> _logger; //use logger to log information and errors
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repository.GetDiscount(request.ProductName);

            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found"));
            }

            var couponModel = _mapper.Map<CouponModel>(coupon); //convert Entities coupon to Protobuf couponModel with automapper
            return couponModel; // return CouponModel obj
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon); //convert couponModel from request.Coupon to coupon

            await _repository.CreateDiscount(coupon); //call repository CreateDiscount and pass in coupon as argument

            _logger.LogInformation("Discount has been created successfully for ProductName: {ProductName}", coupon.ProductName); //log information
            var couponModel = _mapper.Map<CouponModel>(coupon); //convert coupon back to coupon model and return it
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon); //map to coupon
            await _repository.UpdateDiscount(coupon);
            _logger.LogInformation("Discount has been updated successfully. ProductName: {ProductName}", coupon?.ProductName);
            var couponModel = _mapper.Map<CouponModel>(coupon); //map to couponModel
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _repository.DeleteDiscount(request.ProductName);
            var response = new DeleteDiscountResponse
            {
                Success = deleted,
            };
            return response;
        }
    }
}
