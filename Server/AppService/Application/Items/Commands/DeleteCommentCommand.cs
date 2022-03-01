﻿
using Skynet.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Skynet.Application.Items.Commands;

public record DeleteCommentCommand(string CommentId) : IRequest
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
    {
        private readonly ICatalogContext context;

        public DeleteCommentCommandHandler(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(i => i.Id == request.CommentId, cancellationToken);

            if (comment is null) throw new Exception();

            context.Comments.Remove(comment);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}