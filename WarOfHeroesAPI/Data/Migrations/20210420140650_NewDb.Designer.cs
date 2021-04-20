﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WarOfHeroesUsersAPI.Data;

namespace WarOfHeroesUsersAPI.Data.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20210420140650_NewDb")]
    partial class NewDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WarOfHeroesUsersAPI.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GoogleId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WarOfHeroesUsersAPI.Data.Entities.UserHeroDeck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HeroId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserHeroDeck");
                });

            modelBuilder.Entity("WarOfHeroesUsersAPI.Data.Entities.UserHeroInventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HeroId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserHeroInventory");
                });

            modelBuilder.Entity("WarOfHeroesUsersAPI.Data.Entities.UserHeroDeck", b =>
                {
                    b.HasOne("WarOfHeroesUsersAPI.Data.Entities.User", null)
                        .WithMany("Deck")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WarOfHeroesUsersAPI.Data.Entities.UserHeroInventory", b =>
                {
                    b.HasOne("WarOfHeroesUsersAPI.Data.Entities.User", null)
                        .WithMany("Inventory")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WarOfHeroesUsersAPI.Data.Entities.User", b =>
                {
                    b.Navigation("Deck");

                    b.Navigation("Inventory");
                });
#pragma warning restore 612, 618
        }
    }
}
