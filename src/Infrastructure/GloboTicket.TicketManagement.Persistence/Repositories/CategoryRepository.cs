using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.TicketManagement.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(GloboTicketDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Category>> GetCategoriesWithEvents(bool includePassedEvents)
        {
            var allCategories = await _dbContext.Categories.Include(x => x.Events).ToListAsync();

            if (!includePassedEvents)
            {
                allCategories.ForEach(c => c.Events.ToList().RemoveAll(c => c.Date < DateTime.Now));
            }

            return allCategories;
        }

        public Task<bool> IsCategoryNameUnique(string name)
        {
            var matches = _dbContext.Categories.Any(c => c.Name.Equals(name));
            return Task.FromResult(matches);
        }
    }
}
