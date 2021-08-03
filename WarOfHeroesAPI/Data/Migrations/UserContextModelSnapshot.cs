﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WarOfHeroesUsersAPI.Data;

namespace WarOfHeroesUsersAPI.Migrations
{
    [DbContext(typeof(UserContext))]
    partial class UserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("AccessToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Coins")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GoogleId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Losses")
                        .HasColumnType("int");

                    b.Property<int>("Wins")
                        .HasColumnType("int");

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

                    b.ToTable("UserHeroDecks");
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

                    b.ToTable("UserHeroInventories");
                });

            modelBuilder.Entity("WarOfHeroesUsersAPI.Data.Entities.UserHeroDeck", b =>
                {
                    b.HasOne("WarOfHeroesUsersAPI.Data.Entities.User", null)
                        .WithMany("UserHeroDecks")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WarOfHeroesUsersAPI.Data.Entities.UserHeroInventory", b =>
                {
                    b.HasOne("WarOfHeroesUsersAPI.Data.Entities.User", null)
                        .WithMany("UserHeroInventories")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WarOfHeroesUsersAPI.Data.Entities.User", b =>
                {
                    b.Navigation("UserHeroDecks");

                    b.Navigation("UserHeroInventories");
                });
#pragma warning restore 612, 618
        }
    }
}
