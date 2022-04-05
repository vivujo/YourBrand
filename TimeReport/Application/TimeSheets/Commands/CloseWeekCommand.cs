﻿
using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.TimeReport.Application.Common.Interfaces;
using YourBrand.TimeReport.Domain.Entities;
using YourBrand.TimeReport.Domain.Exceptions;

namespace YourBrand.TimeReport.Application.TimeSheets.Commands;

public class CloseWeekCommand : IRequest
{
    public CloseWeekCommand(string timeSheetId)
    {
        TimeSheetId = timeSheetId;
    }

    public string TimeSheetId { get; }

    public class CloseWeekCommandHandler : IRequestHandler<CloseWeekCommand>
    {
        private readonly ITimeReportContext _context;

        public CloseWeekCommandHandler(ITimeReportContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CloseWeekCommand request, CancellationToken cancellationToken)
        {
            var timeSheet = await _context.TimeSheets.GetTimeSheetAsync(request.TimeSheetId, cancellationToken);

            if (timeSheet is null)
            {
                throw new TimeSheetNotFoundException(request.TimeSheetId);
            }

            timeSheet.UpdateStatus(TimeSheetStatus.Closed);

            foreach (var entry in timeSheet.Entries)
            {
                entry.UpdateStatus(EntryStatus.Locked);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
