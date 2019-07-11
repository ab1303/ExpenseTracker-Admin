﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PartnerUser.Repositories.DbContext;

namespace PartnerUser.Repositories.Migrations
{
    [DbContext(typeof(PartnerUserDbContext))]
    [Migration("20190611210041_DateTimeDataType")]
    partial class DateTimeDataType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PartnerUser.Domain.Model.PartnerUser", b =>
                {
                    b.Property<Guid>("PartnerUserId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<Guid?>("BeneficiaryId")
                        .HasMaxLength(36);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("DATETIME");

                    b.Property<Guid>("OfxUserGuid")
                        .HasMaxLength(36);

                    b.Property<Guid>("PartnerAppId")
                        .HasMaxLength(36);

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("DATETIME");

                    b.HasKey("PartnerUserId");

                    b.HasIndex("OfxUserGuid", "PartnerAppId")
                        .IsUnique();

                    b.ToTable("PartnerUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
