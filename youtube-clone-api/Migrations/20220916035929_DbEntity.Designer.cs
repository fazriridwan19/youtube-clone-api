﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using youtube_clone_api.Data;

#nullable disable

namespace youtube_clone_api.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220916035929_DbEntity")]
    partial class DbEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("youtube_clone_api.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CommentatorEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CommentedVideoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CommentedVideoId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("youtube_clone_api.Models.Subscriber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("SubscribedUserId")
                        .HasColumnType("int");

                    b.Property<string>("SubscriberEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SubscribedUserId");

                    b.ToTable("Subscribers");
                });

            modelBuilder.Entity("youtube_clone_api.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("SubscribersCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("youtube_clone_api.Models.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlThumbnail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlVideo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VideoOwnerId")
                        .HasColumnType("int");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VideoOwnerId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("youtube_clone_api.Models.Comment", b =>
                {
                    b.HasOne("youtube_clone_api.Models.Video", "CommentedVideo")
                        .WithMany("Comments")
                        .HasForeignKey("CommentedVideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CommentedVideo");
                });

            modelBuilder.Entity("youtube_clone_api.Models.Subscriber", b =>
                {
                    b.HasOne("youtube_clone_api.Models.User", "SubscribedUser")
                        .WithMany("Subscribers")
                        .HasForeignKey("SubscribedUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SubscribedUser");
                });

            modelBuilder.Entity("youtube_clone_api.Models.Video", b =>
                {
                    b.HasOne("youtube_clone_api.Models.User", "VideoOwner")
                        .WithMany("Videos")
                        .HasForeignKey("VideoOwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VideoOwner");
                });

            modelBuilder.Entity("youtube_clone_api.Models.User", b =>
                {
                    b.Navigation("Subscribers");

                    b.Navigation("Videos");
                });

            modelBuilder.Entity("youtube_clone_api.Models.Video", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
