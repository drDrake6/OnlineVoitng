﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VotingApp.Models;

namespace VotingApp.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("UserVoting", b =>
                {
                    b.Property<int>("matesId")
                        .HasColumnType("int");

                    b.Property<int>("votesId")
                        .HasColumnType("int");

                    b.HasKey("matesId", "votesId");

                    b.HasIndex("votesId");

                    b.ToTable("UserVoting");
                });

            modelBuilder.Entity("VotingApp.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("VotingApp.Models.Candidate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("VotingApp.Models.Result", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Votes")
                        .HasColumnType("int");

                    b.Property<int?>("VotingId")
                        .HasColumnType("int");

                    b.Property<int?>("candidateId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VotingId");

                    b.HasIndex("candidateId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("VotingApp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VotingApp.Models.Voting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Votings");
                });

            modelBuilder.Entity("UserVoting", b =>
                {
                    b.HasOne("VotingApp.Models.User", null)
                        .WithMany()
                        .HasForeignKey("matesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VotingApp.Models.Voting", null)
                        .WithMany()
                        .HasForeignKey("votesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VotingApp.Models.Result", b =>
                {
                    b.HasOne("VotingApp.Models.Voting", null)
                        .WithMany("results")
                        .HasForeignKey("VotingId");

                    b.HasOne("VotingApp.Models.Candidate", "candidate")
                        .WithMany()
                        .HasForeignKey("candidateId");

                    b.Navigation("candidate");
                });

            modelBuilder.Entity("VotingApp.Models.Voting", b =>
                {
                    b.Navigation("results");
                });
#pragma warning restore 612, 618
        }
    }
}
