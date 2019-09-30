using System.ComponentModel;
using MediatR;

namespace VND.CoolStore.ProductCatalog.DataContracts.Api.V1
{
    [DefaultValue("DefaultReflection")]
    public static partial class CatalogApiReflection
    {
    }

    public partial class GetProductsRequest : IRequest<GetProductsResponse>
    {
    }

    public partial class GetProductByIdRequest : IRequest<GetProductByIdResponse>
    {
    }

    public partial class CreateProductRequest : IRequest<CreateProductResponse>
    {
    }

    public partial class UpdateProductRequest : IRequest<UpdateProductResponse>
    {
    }

    public partial class DeleteProductRequest : IRequest<DeleteProductResponse>
    {
    }
}
