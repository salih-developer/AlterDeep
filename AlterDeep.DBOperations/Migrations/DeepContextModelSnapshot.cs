﻿// <auto-generated />
using System;
using AlterDeep.DBOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AlterDeep.DBOperations.Migrations
{
    [DbContext(typeof(DeepContext))]
    partial class DeepContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AlterDeep.DBOperations.Content", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ContentText")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Content");
                });

            modelBuilder.Entity("AlterDeep.DBOperations.Flow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Flow");
                });

            modelBuilder.Entity("AlterDeep.DBOperations.TransactionPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("FriendlyName")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TransactionPage");
                });

            modelBuilder.Entity("AlterDeep.DBOperations.TransactionPageContents", b =>
                {
                    b.Property<int>("TransactionPageId")
                        .HasColumnType("int");

                    b.Property<int>("ContentId")
                        .HasColumnType("int");

                    b.HasKey("TransactionPageId", "ContentId");

                    b.HasIndex("ContentId");

                    b.ToTable("TransactionPageContents");
                });

            modelBuilder.Entity("AlterDeep.DBOperations.TransactionPageContents", b =>
                {
                    b.HasOne("AlterDeep.DBOperations.Content", "Content")
                        .WithMany("TransactionPageContents")
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AlterDeep.DBOperations.TransactionPage", "TransactionPage")
                        .WithMany("TransactionPageContents")
                        .HasForeignKey("TransactionPageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}