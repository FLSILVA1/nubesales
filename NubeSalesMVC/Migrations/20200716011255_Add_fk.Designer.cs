﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NubeSalesMVC.Data;

namespace NubeSalesMVC.Migrations
{
    [DbContext(typeof(NubeSalesMVCContext))]
    [Migration("20200716011255_Add_fk")]
    partial class Add_fk
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("NubeSalesMVC.Models.Pagar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DtaMovimento");

                    b.Property<int>("IdPessoa");

                    b.Property<int>("IdTipo");

                    b.Property<double>("Valor");

                    b.HasKey("Id");

                    b.ToTable("Pagar");
                });

            modelBuilder.Entity("NubeSalesMVC.Models.Pessoa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Pessoa");
                });

            modelBuilder.Entity("NubeSalesMVC.Models.Receber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DtaMovimento");

                    b.Property<int>("IdTipo");

                    b.Property<int>("PessoaId");

                    b.Property<double>("Valor");

                    b.HasKey("Id");

                    b.HasIndex("PessoaId");

                    b.ToTable("Receber");
                });

            modelBuilder.Entity("NubeSalesMVC.Models.Receber", b =>
                {
                    b.HasOne("NubeSalesMVC.Models.Pessoa", "IdPessoa")
                        .WithMany()
                        .HasForeignKey("PessoaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}