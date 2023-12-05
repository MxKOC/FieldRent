﻿// <auto-generated />
using System;
using FieldRent.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FieldRent.Migrations
{
    [DbContext(typeof(BlogContext))]
    partial class BlogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("FieldRent.Entity.Field", b =>
                {
                    b.Property<int>("FieldId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FieldCoordinate")
                        .HasColumnType("TEXT");

                    b.Property<string>("FieldImage")
                        .HasColumnType("TEXT");

                    b.Property<bool>("FieldIsActive")
                        .HasColumnType("INTEGER");

                    b.HasKey("FieldId");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("FieldRent.Entity.Map", b =>
                {
                    b.Property<int>("MapId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FieldId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MapCondition")
                        .HasColumnType("TEXT");

                    b.Property<string>("MapCoordinate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("MapIsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MapPrice")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("MapStart")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("MapStop")
                        .HasColumnType("TEXT");

                    b.Property<string>("MapUrl")
                        .HasColumnType("TEXT");

                    b.Property<int>("Time")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("MapId");

                    b.HasIndex("FieldId");

                    b.HasIndex("UserId");

                    b.ToTable("Maps");
                });

            modelBuilder.Entity("FieldRent.Entity.Request", b =>
                {
                    b.Property<int>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("RequestIsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RequestName")
                        .HasColumnType("TEXT");

                    b.Property<int>("RequestPrice")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("RequestStart")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RequestStop")
                        .HasColumnType("TEXT");

                    b.HasKey("RequestId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("FieldRent.Entity.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserPrice")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MapRequest", b =>
                {
                    b.Property<int>("MapsMapId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RequestsRequestId")
                        .HasColumnType("INTEGER");

                    b.HasKey("MapsMapId", "RequestsRequestId");

                    b.HasIndex("RequestsRequestId");

                    b.ToTable("MapRequest");
                });

            modelBuilder.Entity("FieldRent.Entity.Map", b =>
                {
                    b.HasOne("FieldRent.Entity.Field", "Field")
                        .WithMany("Maps")
                        .HasForeignKey("FieldId");

                    b.HasOne("FieldRent.Entity.User", "User")
                        .WithMany("Maps")
                        .HasForeignKey("UserId");

                    b.Navigation("Field");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MapRequest", b =>
                {
                    b.HasOne("FieldRent.Entity.Map", null)
                        .WithMany()
                        .HasForeignKey("MapsMapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FieldRent.Entity.Request", null)
                        .WithMany()
                        .HasForeignKey("RequestsRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FieldRent.Entity.Field", b =>
                {
                    b.Navigation("Maps");
                });

            modelBuilder.Entity("FieldRent.Entity.User", b =>
                {
                    b.Navigation("Maps");
                });
#pragma warning restore 612, 618
        }
    }
}
