﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entidades;

namespace Infraestrutura.Configuration
{
    public class ContatoConfiguration : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.ToTable("Contato");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("INT").ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(p => p.DataCriacao).HasColumnName("DataCriacao").HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.Nome).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(p => p.Email).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(p => p.Telefone).HasColumnType("VARCHAR(11)").IsRequired();
            builder.Property(p => p.RegiaoId).HasColumnType("INT").IsRequired();

            builder.HasOne(c => c.Regiao)
                   .WithMany(e => e.Contatos)
                   .HasForeignKey(c => c.RegiaoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
