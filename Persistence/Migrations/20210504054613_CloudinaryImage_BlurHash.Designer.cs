// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence;

namespace Persistence.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210504054613_CloudinaryImage_BlurHash")]
    partial class CloudinaryImage_BlurHash
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Domain.CloudinaryImage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("BlurHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CloudinaryImages");
                });

            modelBuilder.Entity("Domain.Date", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("OtherId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasAlternateKey("UserId", "OtherId");

                    b.HasIndex("OtherId");

                    b.ToTable("Dates");
                });

            modelBuilder.Entity("Domain.Hate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.ToTable("Hates");
                });

            modelBuilder.Entity("Domain.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("OtherId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasAlternateKey("UserId", "OtherId");

                    b.HasIndex("OtherId", "Status");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Domain.Passion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.ToTable("Passions");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessLevel")
                        .HasColumnType("integer");

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<string>("Bio")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("FcmToken")
                        .HasMaxLength(65536)
                        .HasColumnType("character varying(65536)");

                    b.Property<string>("FieldOfStudy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("ProfileImageId")
                        .HasColumnType("text");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)");

                    b.Property<string>("SexualOrientation")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<int>("YearOfStudy")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Age");

                    b.HasIndex("FieldOfStudy");

                    b.HasIndex("ProfileImageId");

                    b.HasIndex("Sex");

                    b.HasIndex("SexualOrientation");

                    b.HasIndex("YearOfStudy");

                    b.ToTable("Users");

                    b.HasCheckConstraint("CK_ValidSexValue", "\"Sex\" in ('Male', 'Female', 'Other')");

                    b.HasCheckConstraint("CK_ValidSexualOrientationValue", "\"SexualOrientation\" in ('Straight', 'Gay', 'Lesbian','Bisexual', 'Transgender', 'Queer')");

                    b.HasCheckConstraint("CK_ValidAgeValue", "\"Age\" >= 16 and \"Age\" <= 100");
                });

            modelBuilder.Entity("HateUser", b =>
                {
                    b.Property<Guid>("HatesId")
                        .HasColumnType("uuid");

                    b.Property<string>("UsersId")
                        .HasColumnType("text");

                    b.HasKey("HatesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("HateUser");
                });

            modelBuilder.Entity("PassionUser", b =>
                {
                    b.Property<Guid>("PassionsId")
                        .HasColumnType("uuid");

                    b.Property<string>("UsersId")
                        .HasColumnType("text");

                    b.HasKey("PassionsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("PassionUser");
                });

            modelBuilder.Entity("Domain.Date", b =>
                {
                    b.HasOne("Domain.User", "Other")
                        .WithMany()
                        .HasForeignKey("OtherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Other");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Match", b =>
                {
                    b.HasOne("Domain.User", "Other")
                        .WithMany()
                        .HasForeignKey("OtherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Other");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.HasOne("Domain.CloudinaryImage", "ProfileImage")
                        .WithMany()
                        .HasForeignKey("ProfileImageId");

                    b.Navigation("ProfileImage");
                });

            modelBuilder.Entity("HateUser", b =>
                {
                    b.HasOne("Domain.Hate", null)
                        .WithMany()
                        .HasForeignKey("HatesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PassionUser", b =>
                {
                    b.HasOne("Domain.Passion", null)
                        .WithMany()
                        .HasForeignKey("PassionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
