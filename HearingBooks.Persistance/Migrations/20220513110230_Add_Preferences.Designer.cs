﻿// <auto-generated />


#nullable disable

using System;
using HearingBooks.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
namespace HearingBooks.Persistance.Migrations
{
    [DbContext(typeof(HearingBooksDbContext))]
    [Migration("20220513110230_Add_Preferences")]
    partial class Add_Preferences
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HearingBooks.Domain.Entities.DialogueSynthesis", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BlobContainerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BlobName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CharacterCount")
                        .HasColumnType("integer");

                    b.Property<string>("DialogueText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("DurationInSeconds")
                        .HasColumnType("integer");

                    b.Property<string>("FirstSpeakerVoice")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RequestingUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("SecondSpeakerVoice")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DialogueSyntheses");
                });

            modelBuilder.Entity("HearingBooks.Domain.Entities.Language", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("HearingBooks.Domain.Entities.Preference", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("AcrylicEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Preferences");
                });

            modelBuilder.Entity("HearingBooks.Domain.Entities.TextSynthesis", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BlobContainerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BlobName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CharacterCount")
                        .HasColumnType("integer");

                    b.Property<int>("DurationInSeconds")
                        .HasColumnType("integer");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RequestingUserId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("SynthesisText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Voice")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TextSyntheses");
                });

            modelBuilder.Entity("HearingBooks.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("EmailIsUsername")
                        .HasColumnType("boolean");

                    b.Property<bool>("EmailNotificationsEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HearingBooks.Domain.Entities.Voice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsMultilingual")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("LanguageId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.ToTable("Voices");
                });

            modelBuilder.Entity("HearingBooks.Domain.Entities.Voice", b =>
                {
                    b.HasOne("HearingBooks.Domain.Entities.Language", null)
                        .WithMany("Voices")
                        .HasForeignKey("LanguageId");
                });

            modelBuilder.Entity("HearingBooks.Domain.Entities.Language", b =>
                {
                    b.Navigation("Voices");
                });
#pragma warning restore 612, 618
        }
    }
}
