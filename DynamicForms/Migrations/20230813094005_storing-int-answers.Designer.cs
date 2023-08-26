﻿// <auto-generated />
using DynamicForms.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DynamicForms.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230813094005_storing-int-answers")]
    partial class storingintanswers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DynamicForms.Models.Answers.DoubleAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("InputId")
                        .HasColumnType("int");

                    b.Property<int>("ProgressId")
                        .HasColumnType("int");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("DoubleAnswers");
                });

            modelBuilder.Entity("DynamicForms.Models.Answers.IntegerAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("InputId")
                        .HasColumnType("int");

                    b.Property<int>("ProgressId")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("IntegerAnswers");
                });

            modelBuilder.Entity("DynamicForms.Models.Answers.TextAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("InputId")
                        .HasColumnType("int");

                    b.Property<int>("ProgressId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TextAnswers");
                });

            modelBuilder.Entity("DynamicForms.Models.Choice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("InputId")
                        .HasColumnType("int");

                    b.Property<string>("Label")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("InputId");

                    b.ToTable("Choices");
                });

            modelBuilder.Entity("DynamicForms.Models.Condition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DependentInput")
                        .HasColumnType("int");

                    b.Property<int>("RequirementId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RequirementId");

                    b.ToTable("Conditions");
                });

            modelBuilder.Entity("DynamicForms.Models.Form", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Forms");
                });

            modelBuilder.Entity("DynamicForms.Models.Input", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("DefaultValue")
                        .HasColumnType("float");

                    b.Property<int>("InputType")
                        .HasColumnType("int");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Label")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Placeholder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StepId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StepId");

                    b.ToTable("Inputs");
                });

            modelBuilder.Entity("DynamicForms.Models.Progress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FormId")
                        .HasColumnType("int");

                    b.Property<int>("StepId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Progresses");
                });

            modelBuilder.Entity("DynamicForms.Models.Requirement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("InputId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("InputId");

                    b.ToTable("Requirements");
                });

            modelBuilder.Entity("DynamicForms.Models.Step", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FormId")
                        .HasColumnType("int");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FormId");

                    b.ToTable("Steps");
                });

            modelBuilder.Entity("DynamicForms.Models.Choice", b =>
                {
                    b.HasOne("DynamicForms.Models.Input", null)
                        .WithMany("Choices")
                        .HasForeignKey("InputId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DynamicForms.Models.Condition", b =>
                {
                    b.HasOne("DynamicForms.Models.Requirement", "Requirement")
                        .WithMany()
                        .HasForeignKey("RequirementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Requirement");
                });

            modelBuilder.Entity("DynamicForms.Models.Input", b =>
                {
                    b.HasOne("DynamicForms.Models.Step", "Step")
                        .WithMany("Inputs")
                        .HasForeignKey("StepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Step");
                });

            modelBuilder.Entity("DynamicForms.Models.Requirement", b =>
                {
                    b.HasOne("DynamicForms.Models.Input", null)
                        .WithMany("Requirements")
                        .HasForeignKey("InputId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DynamicForms.Models.Step", b =>
                {
                    b.HasOne("DynamicForms.Models.Form", "Form")
                        .WithMany("Steps")
                        .HasForeignKey("FormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Form");
                });

            modelBuilder.Entity("DynamicForms.Models.Form", b =>
                {
                    b.Navigation("Steps");
                });

            modelBuilder.Entity("DynamicForms.Models.Input", b =>
                {
                    b.Navigation("Choices");

                    b.Navigation("Requirements");
                });

            modelBuilder.Entity("DynamicForms.Models.Step", b =>
                {
                    b.Navigation("Inputs");
                });
#pragma warning restore 612, 618
        }
    }
}
