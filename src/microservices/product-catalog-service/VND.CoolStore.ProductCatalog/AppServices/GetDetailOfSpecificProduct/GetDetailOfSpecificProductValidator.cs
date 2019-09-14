using System;
using FluentValidation;
using VND.CoolStore.ProductCatalog.DataContracts.V1;

namespace VND.CoolStore.ProductCatalog.AppServices.GetDetailOfSpecificProduct
{
    public class GetDetailOfSpecificProductValidator : AbstractValidator<GetProductByIdRequest>
    {
        public GetDetailOfSpecificProductValidator()
        {

        }
    }
}
