using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TestTask.WebApi.Entities;
using TestTask.WebApi.Exceptions;
using TestTask.WebApi.Infrastructure;
using TestTask.WebApi.ResponseModels;

namespace TestTask.WebApi.Controllers
{
    [Tags(Tags.Journal)]
    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly TestTaskDbContext _context;

        public sealed class Filter
        {
            public DateTimeOffset? From { get; set; }
            public DateTimeOffset? To { get; set; }
            public string? Search { get; set; }
        }

        public JournalController(TestTaskDbContext context)
        {
            _context = context;
        }

        /// <remarks>
        /// Provides the pagination API. Skip means the number of items should be skipped by server. Take means the maximum number items should be returned by server. All fields of the filter are optional.
        /// </remarks>
        [HttpPost("api.user.journal.getRange")]
        public async Task<IReadOnlyList<ExceptionReportModel>> GetRange([Required] int skip, [Required] int take, [Required][FromBody] Filter filter)
        {
            if (skip < 0)
            {
                throw new SecureException("Skip parameter can't be less than 0");
            }

            if (skip < 0)
            {
                throw new SecureException("Take parameter can't be less than or equal 0");
            }

            IQueryable<ExceptionReport> query = _context.Exceptions;

            if (filter is not null)
            {
                if (filter.From is not null && filter.To is not null)
                {
                    if (filter.From.Value >= filter.To.Value)
                    {
                        throw new SecureException("From can't be greater than To");
                    }
                }

                if (filter.From is not null)
                {
                    query = query.Where(x => x.CreatedAt >= filter.From);
                }

                if (filter.To is not null)
                {
                    query = query.Where(x => x.CreatedAt <= filter.To);
                }

                if (!string.IsNullOrWhiteSpace(filter.Search))
                {
                    query = query.Where(x => x.Text.Contains(filter.Search));
                }
            }

            var result = await query.Skip(skip).Take(take).ToListAsync();

            return result.Select(x => ToExceptionReportModel(x)).ToList();
        }

        /// <remarks>
        /// Returns the information about an particular event by ID.
        /// </remarks>
        [HttpPost("api.user.journal.getSingle")]
        public async Task<ExceptionReportModel> GetSingle([Required] long id)
        {
            var report = await _context.Exceptions.FirstOrDefaultAsync(x => x.Id == id);

            if (report is null)
            {
                throw new SecureException($"There is no report with Id: {id}");
            }

            return ToExceptionReportModel(report);
        }

        private ExceptionReportModel ToExceptionReportModel(ExceptionReport report)
        {
            return new ExceptionReportModel() { Id = report.Id, EventId = report.EventId, CreatedAt = report.CreatedAt, Text = report.Text };
        }
    }
}
