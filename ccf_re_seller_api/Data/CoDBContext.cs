using System;
using Microsoft.EntityFrameworkCore;

namespace ccf_re_seller_api.Data
{
    public class CoDBContext : DbContext
    {
        public CoDBContext(DbContextOptions<CoDBContext> options) : base(options)
        {
        }

        
    }
}

