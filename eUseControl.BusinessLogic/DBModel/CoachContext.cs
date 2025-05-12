using System;
using System.Collections.Generic;
using eUseControl.Domain.Entities;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace eUseControl.BusinessLogic.DBModel
{
    public class CoachContext : DbContext
    {
        public CoachContext() : base("name=eUseControl")
        {
        }
        public virtual DbSet<Coach> Coaches { get; set; }

    }
}
