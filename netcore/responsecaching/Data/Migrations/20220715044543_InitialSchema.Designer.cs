﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleChat.Data;

#nullable disable

namespace SimpleChat.Migrations
{
    [DbContext(typeof(ChatContext))]
    [Migration("20220715044543_InitialSchema")]
    partial class InitialSchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SimpleChat.Data.ChatUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Shawn Wildermuth"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Jake Smith"
                        });
                });

            modelBuilder.Entity("SimpleChat.Data.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("MessageDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId")
                        .IsUnique();

                    b.HasIndex("SenderId")
                        .IsUnique();

                    b.ToTable("Messages");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MessageDate = new DateTime(2022, 7, 15, 4, 45, 43, 438, DateTimeKind.Utc).AddTicks(8806),
                            ReceiverId = 2,
                            SenderId = 1,
                            Text = "Hello World"
                        },
                        new
                        {
                            Id = 2,
                            MessageDate = new DateTime(2022, 7, 15, 4, 45, 43, 438, DateTimeKind.Utc).AddTicks(8807),
                            ReceiverId = 1,
                            SenderId = 2,
                            Text = "Replied"
                        });
                });

            modelBuilder.Entity("SimpleChat.Data.Message", b =>
                {
                    b.HasOne("SimpleChat.Data.ChatUser", "Receiver")
                        .WithOne()
                        .HasForeignKey("SimpleChat.Data.Message", "ReceiverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SimpleChat.Data.ChatUser", "Sender")
                        .WithOne()
                        .HasForeignKey("SimpleChat.Data.Message", "SenderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });
#pragma warning restore 612, 618
        }
    }
}
