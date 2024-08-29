﻿
using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Invoicing.Domain;
using YourBrand.Invoicing.Domain.Entities;

namespace YourBrand.Invoicing.Application.Commands;

public record CreateInvoice(string OrganizationId, DateTime? Date, int? Status, string? Note, SetCustomerDto? Customer) : IRequest<InvoiceDto>
{
    public class Handler(IInvoicingContext context) : IRequestHandler<CreateInvoice, InvoiceDto>
    {
        public async Task<InvoiceDto> Handle(CreateInvoice request, CancellationToken cancellationToken)
        {
            var invoice = new YourBrand.Invoicing.Domain.Entities.Invoice(request.Date, note: request.Note);

            invoice.Id = Guid.NewGuid().ToString();

            try
            {
                invoice.InvoiceNo = context.Invoices.Select(x => x.InvoiceNo).ToList().Select(x => x).Max() + 1;
            }
            catch
            {
                invoice.InvoiceNo = 1;
            }

            if(request.Status is not null) 
            {
                invoice.UpdateStatus(request.Status.GetValueOrDefault());
            }

            if (request.Customer is not null)
            {
                if (invoice.Customer is null)
                {
                    invoice.Customer = new Domain.Entities.Customer();
                }

                invoice.Customer.Id = request.Customer.Id;
                invoice.Customer.Name = request.Customer.Name;
            }

            invoice.OrganizationId = request.OrganizationId;

            context.Invoices.Add(invoice);

            await context.SaveChangesAsync(cancellationToken);

            invoice = await context.Invoices
                .Include(i => i.Status)
                .Include(i => i.Items)
                .AsSplitQuery()
                .AsNoTracking()
                .InOrganization(request.OrganizationId)
                .FirstAsync(x => x.Id == invoice.Id, cancellationToken);

            return invoice.ToDto();
        }
    }
}

public record SetCustomerDto(string Id, string Name);