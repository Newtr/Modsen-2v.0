﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Modsen.Infrastructure;

#nullable disable

namespace Modsen.Infrastructure.Migrations
{
    [DbContext(typeof(ModsenContext))]
    partial class ModsenContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("EventsAndMembers", b =>
                {
                    b.Property<int>("EventID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MemberID")
                        .HasColumnType("INTEGER");

                    b.HasKey("EventID", "MemberID");

                    b.HasIndex("MemberID");

                    b.ToTable("EventsAndMembers");
                });

            modelBuilder.Entity("Modsen.Domain.EventImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EventId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("EventImages");
                });

            modelBuilder.Entity("Modsen.Domain.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Modsen.Domain.MyEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateOfEvent")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("EventCategory")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("EventLocation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxMember")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Modsen.Domain.Role", b =>
                {
                    b.Property<int>("RoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("RoleID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Modsen.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EventsAndMembers", b =>
                {
                    b.HasOne("Modsen.Domain.MyEvent", null)
                        .WithMany()
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Modsen.Domain.Member", null)
                        .WithMany()
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Modsen.Domain.EventImage", b =>
                {
                    b.HasOne("Modsen.Domain.MyEvent", "MyEvent")
                        .WithMany("EventImages")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MyEvent");
                });

            modelBuilder.Entity("Modsen.Domain.User", b =>
                {
                    b.HasOne("Modsen.Domain.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Modsen.Domain.MyEvent", b =>
                {
                    b.Navigation("EventImages");
                });
#pragma warning restore 612, 618
        }
    }
}
