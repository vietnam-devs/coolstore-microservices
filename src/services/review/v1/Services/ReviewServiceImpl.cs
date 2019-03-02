using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.Mongo;
using NetCoreKit.Utils.Extensions;
using VND.CoolStore.Services.Review.v1.Extensions;
using VND.CoolStore.Services.Review.v1.Grpc;
using Empty = Google.Protobuf.WellKnownTypes.Empty;

namespace VND.CoolStore.Services.Review.v1.Services
{
    public class AuthNInterceptor : Interceptor
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;

        public AuthNInterceptor(IServiceProvider resolver)
        {
            _hostingEnvironment = resolver.GetService<IHostingEnvironment>();
            _config = resolver.GetService<IConfiguration>();
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
            ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var attribute = (CheckPolicyAttribute)continuation.Method.GetCustomAttributes(typeof(CheckPolicyAttribute), false).FirstOrDefault();
            if (attribute == null)
            {
                return await continuation(request, context);
            }

            var userToken = string.Empty;
            if (context.RequestHeaders.Any(x => x.Key == "Authorization"))
            {
                userToken = context.RequestHeaders.FirstOrDefault(x => x.Key == "Authorization")?.Value;
            }
            else
            {
                if (_hostingEnvironment.IsDevelopment())
                {
                    userToken = _config.GetValue<string>("Jwt_Token_Dev");
                }
            }

            var disco = await DiscoveryClient.GetAsync(_config.GetValue<string>("AuthorityUri"));
            var keys = new List<SecurityKey>();

            foreach (var webKey in disco.KeySet.Keys)
            {
                var e = Base64Url.Decode(webKey.E);
                var n = Base64Url.Decode(webKey.N);

                var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
                {
                    KeyId = webKey.Kid
                };

                keys.Add(key);
            }

            var parameters = new TokenValidationParameters
            {
                ValidIssuer = disco.Issuer,
                ValidAudience = _config.GetValue<string>("Jwt_Audience"),
                IssuerSigningKeys = keys,

                NameClaimType = JwtClaimTypes.Name,
                RoleClaimType = JwtClaimTypes.Role,

                RequireSignedTokens = true,
                ValidateLifetime = false
            };

            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();

            var user = handler.ValidateToken(userToken.TrimStart("Bearer").TrimStart(" "), parameters, out _);
            if (!user.HasClaim(c => c.Value == attribute.Name))
            {
                throw new AuthenticationException("Couldn't access to this API, please check your permission.");
            }

            return await continuation(request, context);
        }
    }

    public class CheckPolicyAttribute : Attribute
    {
        public CheckPolicyAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class PingServiceImpl : PingService.PingServiceBase
    {
        private readonly ILogger<PingServiceImpl> _logger;

        public PingServiceImpl(IServiceProvider resolver)
        {
            _logger = resolver.GetService<ILoggerFactory>()?.CreateLogger<PingServiceImpl>();
        }

        //[CheckPolicy("review_api_scope")]
        public override async Task<PingResponse> Ping(Empty request, ServerCallContext context)
        {
            return await Task.FromResult(new PingResponse
            {
                Message = $"Say hello from {Environment.MachineName} machine!!!"
            });
        }
    }

    public class ReviewServiceImpl : ReviewService.ReviewServiceBase
    {
        private readonly ILogger<ReviewServiceImpl> _logger;
        private readonly IQueryRepositoryFactory _queryFactory;
        private readonly IUnitOfWorkAsync _commandFactory;

        public ReviewServiceImpl(IServiceProvider resolver)
        {
            _logger = resolver.GetService<ILoggerFactory>()?.CreateLogger<ReviewServiceImpl>();

            _queryFactory = resolver.GetService<IQueryRepositoryFactory>();
            _commandFactory = resolver.GetService<IUnitOfWorkAsync>();
        }

        [CheckPolicy("review_api_scope")]
        public override async Task<GetReviewsResponse> GetReviews(GetReviewsRequest request, ServerCallContext context)
        {
            var reviewQueryRepo = _queryFactory.MongoQueryRepository<Domain.Review>();
            var reviews =
                await reviewQueryRepo.FindListByFieldAsync(r =>
                    r.ReviewProduct.Id == request.ProductId.ConvertTo<Guid>());

            if (reviews == null)
                throw new CoreException($"Couldn't find the review with productId#{request.ProductId}.");

            var response = new GetReviewsResponse();
            response.Reviews.AddRange(
                reviews
                    .Select(x => x.ToDto())
                    .ToList());

            return response;
        }

        [CheckPolicy("review_api_scope")]
        public override async Task<CreateReviewResponse> CreateReview(CreateReviewRequest request,
            ServerCallContext context)
        {
            var reviewRepository = _commandFactory.RepositoryAsync<Domain.Review>();

            var review = Domain.Review
                .Load(request.Content)
                .AddAuthor(request.UserId.ConvertTo<Guid>())
                .AddProduct(request.ProductId.ConvertTo<Guid>());

            var result = await reviewRepository.AddAsync(review);

            return new CreateReviewResponse
            {
                Result = result.ToDto()
            };
        }

        [CheckPolicy("review_api_scope")]
        public override async Task<EditReviewResponse> EditReview(EditReviewRequest request, ServerCallContext context)
        {
            var reviewQueryRepo = _queryFactory.MongoQueryRepository<Domain.Review>();
            var reviewRepo = _commandFactory.RepositoryAsync<Domain.Review>();

            var review = await reviewQueryRepo.FindOneAsync(x => x.Id, request.ReviewId.ConvertTo<Guid>());
            if (review == null)
                throw new Exception($"Couldn't find the review #{request.ReviewId}.");

            review.Content = request.Content;
            var result = await reviewRepo.UpdateAsync(review);

            return new EditReviewResponse
            {
                Result = result.ToDto()
            };
        }

        [CheckPolicy("review_api_scope")]
        public override async Task<DeleteReviewResponse> DeleteReview(DeleteReviewRequest request,
            ServerCallContext context)
        {
            var reviewQueryRepo = _queryFactory.MongoQueryRepository<Domain.Review>();
            var reviewRepo = _commandFactory.RepositoryAsync<Domain.Review>();

            var review = await reviewQueryRepo.FindOneAsync(x => x.Id, request.ReviewId.ConvertTo<Guid>());
            if (review == null)
                throw new Exception($"Couldn't find the review #{request.ReviewId}.");

            var result = await reviewRepo.DeleteAsync(review);

            return new DeleteReviewResponse
            {
                Id = result.Id.ToString()
            };
        }
    }
}
