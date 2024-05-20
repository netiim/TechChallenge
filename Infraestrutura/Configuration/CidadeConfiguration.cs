using Core.Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Configuration
{
    public class CidadeConfiguration : IEntityTypeConfiguration<Regiao>
    {
        public void Configure(EntityTypeBuilder<Regiao> builder)
        {
            builder.ToTable("Regiao");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("INT").ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(p => p.DataCriacao).HasColumnName("DataCriacao").HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.numeroDDD).HasColumnType("INT").IsRequired();
            builder.Property(p => p.EstadoId).HasColumnType("INT").IsRequired();

            builder.HasOne(c => c.Estado)
                   .WithMany(e => e.Regioes)  
                   .HasForeignKey(c => c.EstadoId)  
                   .OnDelete(DeleteBehavior.Restrict);  
        }
    }
}
