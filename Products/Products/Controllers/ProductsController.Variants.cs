using Microsoft.AspNetCore.Mvc;

using YourBrand.Products.Application;
using YourBrand.Products.Application.Products.Variants;

namespace YourBrand.Products.Controllers;

partial class ProductsController : Controller
{
    [HttpGet("{productId}/Variants")]
    public async Task<ActionResult<IEnumerable<ProductVariantDto>>> GetVariants(string productId)
    {
        return Ok(await _mediator.Send(new GetProductVariants(productId)));
    }

    [HttpDelete("{productId}/Variants/{variantId}")]
    public async Task<ActionResult> DeleteVariant(string productId, string variantId)
    {
        await _mediator.Send(new DeleteProductVariant(productId, variantId));
        return Ok();
    }

    [HttpGet("{productId}/Variants/{variantId}")]
    public async Task<ActionResult<ProductVariantDto>> GetVariant(string productId, string variantId)
    {
        return Ok( await _mediator.Send(new GetProductVariant(productId, variantId)));
    }

    [HttpPost("{productId}/Variants/Find")]
    public async Task<ActionResult<ProductVariantDto>> FindVariantByOptionValues(string productId, Dictionary<string, string?> selectedOptions)
    {
        return Ok(await _mediator.Send(new FindProductVariant(productId, selectedOptions)));
    }

    [HttpGet("{productId}/Variants/{variantId}/Options")]
    public async Task<ActionResult<ProductVariantDto>> GetVariantOptions(string productId, string variantId)
    {
        return Ok(_mediator.Send(new GetProductVariantOptions(productId, variantId)));
    }

    [HttpPost("{productId}/Variants")]
    [ProducesResponseType(typeof(ProductVariantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductVariantDto>> CreateVariant(string productId, ApiCreateProductVariant data)
    {
        try
        {
            return Ok(await _mediator.Send(new CreateProductVariant(productId, data)));
        }
        catch (VariantAlreadyExistsException e)
        {
            return Problem(
                title: "Variant already exists.",
                detail: "There is already a variant with the chosen options.",
                instance: Request.Path,
                statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpPut("{productId}/Variants/{variantId}")]
    [ProducesResponseType(typeof(ProductVariantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductVariantDto>> UpdateVariant(string productId, string variantId, ApiUpdateProductVariant data)
    {
        try
        {
            return Ok(await _mediator.Send(new UpdateProductVariant(productId, variantId, data)));
        }
        catch (VariantAlreadyExistsException e)
        {
            return Problem(
                title: "Variant already exists.",
                detail: "There is already a variant with the chosen options.",
                instance: Request.Path,
                statusCode: StatusCodes.Status400BadRequest);
        }
    }
    
    [HttpPost("{productId}/Variants/{variantId}/UploadImage")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadVariantImage([FromRoute] string productId, string variantId, IFormFile file, CancellationToken cancellationToken)
    {
        var url = await _mediator.Send(new UploadProductVariantImage(productId, variantId, file.Name, file.OpenReadStream()), cancellationToken); 
        return Ok(url);
    }
}