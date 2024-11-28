using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlanCRUD.Models;

namespace PlanCRUD.Data
{
    public class PlanCRUDContext : DbContext
    {
        public PlanCRUDContext (DbContextOptions<PlanCRUDContext> options)
            : base(options)
        {
        }

        public DbSet<PlanCRUD.Models.Plan> Plan { get; set; } = default!;
    }
}
