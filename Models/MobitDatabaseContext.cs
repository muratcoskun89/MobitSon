namespace MobitBilismDgerlendirmeProjesi.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MobitDatabaseContext : DbContext
    {
        public MobitDatabaseContext()
            : base("name=MobitDatabaseContext")
        {
        }

        public virtual DbSet<Kullanicilar> Kullanicilar { get; set; }
        public virtual DbSet<Urunler> Urunler { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
