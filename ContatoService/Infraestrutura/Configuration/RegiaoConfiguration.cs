using Core.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Configuration
{
    public class RegiaoConfiguration : IEntityTypeConfiguration<Regiao>
    {
        public void Configure(EntityTypeBuilder<Regiao> builder)
        {
            builder.ToTable("Regiao");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("INT").ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(p => p.DataCriacao).HasColumnName("DataCriacao").HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.NumeroDDD).HasColumnType("INT").IsRequired();
            builder.Property(p => p.EstadoId).HasColumnType("INT").IsRequired();
            builder.Property(p => p.IdLocalidadeAPI).HasColumnType("VARCHAR(MAX)").IsRequired();

            builder.HasOne(c => c.Estado)
                   .WithMany(e => e.Regioes)  
                   .HasForeignKey(c => c.EstadoId)  
                   .OnDelete(DeleteBehavior.Restrict);  
        }
    }
}
