﻿
using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.TimeReport.Application.Common.Interfaces;
using YourBrand.TimeReport.Application.Common.Models;
using YourBrand.TimeReport.Domain.Exceptions;

namespace YourBrand.TimeReport.Application.Projects.Queries;

public record GetProjectStatisticsForProjectQuery(string OrganizationId, string ProjectId, DateTime? From = null, DateTime? To = null) : IRequest<Data>
{
    public class GetProjectStatisticsForProjectQueryHandler(ITimeReportContext context) : IRequestHandler<GetProjectStatisticsForProjectQuery, Data>
    {
        public async Task<Data> Handle(GetProjectStatisticsForProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await context.Projects
                .Include(x => x.Activities)
                .ThenInclude(x => x.Entries)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == request.ProjectId);

            if (project is null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            List<DateTime> months = new();

            const int monthSpan = 5;

            DateTime lastDate = request.To?.Date ?? DateTime.Now.Date;
            DateTime firstDate = request.From?.Date ?? lastDate.AddMonths(-monthSpan)!;

            for (DateTime dt = firstDate; dt <= lastDate; dt = dt.AddMonths(1))
            {
                months.Add(dt);
            }

            List<Series> series = new();

            var firstMonth = DateOnly.FromDateTime(firstDate);
            var lastMonth = DateOnly.FromDateTime(lastDate);

            foreach (var activity in project.Activities)
            {
                List<decimal> values = new();

                foreach (var month in months)
                {
                    var value = activity.Entries
                        .Where(e => e.Date.Year == month.Year && e.Date.Month == month.Month)
                        .Select(x => x.Hours.GetValueOrDefault())
                        .Sum();

                    values.Add((decimal)value);
                }

                series.Add(new Series(activity.Name, values));
            }

            return new Data(
                months.Select(d => d.ToString("MMM yy")).ToArray(),
                series);
        }
    }
}