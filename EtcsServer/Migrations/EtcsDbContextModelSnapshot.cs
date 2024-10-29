﻿// <auto-generated />
using System;
using EtcsServer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EtcsServer.Migrations
{
    [DbContext(typeof(EtcsDbContext))]
    partial class EtcsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EtcsServer.Database.Entity.Crossing", b =>
                {
                    b.Property<int>("CrossingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CrossingId"));

                    b.Property<int>("TrackId")
                        .HasColumnType("int");

                    b.HasKey("CrossingId");

                    b.HasIndex("TrackId");

                    b.ToTable("Crossings");
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MessageId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.RailroadSign", b =>
                {
                    b.Property<int>("RailroadSignId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RailroadSignId"));

                    b.Property<double>("DistanceFromTrackStart")
                        .HasColumnType("float");

                    b.Property<bool>("IsFacedUp")
                        .HasColumnType("bit");

                    b.Property<double>("MaxSpeed")
                        .HasColumnType("float");

                    b.Property<int>("TrackId")
                        .HasColumnType("int");

                    b.HasKey("RailroadSignId");

                    b.HasIndex("TrackId");

                    b.ToTable("Signs");
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.RailwaySignal", b =>
                {
                    b.Property<int>("RailwaySignalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RailwaySignalId"));

                    b.Property<double>("DistanceFromTrackStart")
                        .HasColumnType("float");

                    b.Property<bool>("IsFacedUp")
                        .HasColumnType("bit");

                    b.Property<int>("TrackId")
                        .HasColumnType("int");

                    b.HasKey("RailwaySignalId");

                    b.HasIndex("TrackId");

                    b.ToTable("TrackSignals");
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.SwitchRoute", b =>
                {
                    b.Property<int>("SwitchRouteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SwitchRouteId"));

                    b.Property<double>("MaxSpeedMps")
                        .HasColumnType("float");

                    b.Property<int>("SwitchId")
                        .HasColumnType("int");

                    b.Property<int>("TrackFromId")
                        .HasColumnType("int");

                    b.Property<int>("TrackToId")
                        .HasColumnType("int");

                    b.HasKey("SwitchRouteId");

                    b.HasIndex("SwitchId");

                    b.HasIndex("TrackFromId");

                    b.HasIndex("TrackToId");

                    b.ToTable("TrackSwitches");
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.TrackageElement", b =>
                {
                    b.Property<int>("TrackageElementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TrackageElementId"));

                    b.Property<int?>("LeftSideElementId")
                        .HasColumnType("int");

                    b.Property<int?>("RightSideElementId")
                        .HasColumnType("int");

                    b.HasKey("TrackageElementId");

                    b.HasIndex("LeftSideElementId");

                    b.HasIndex("RightSideElementId");

                    b.ToTable("TrackageElement");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.Train", b =>
                {
                    b.Property<int>("TrainId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TrainId"));

                    b.Property<double>("BrakeWeight")
                        .HasColumnType("float");

                    b.Property<double>("LengthMeters")
                        .HasColumnType("float");

                    b.Property<double>("MaxSpeedMps")
                        .HasColumnType("float");

                    b.HasKey("TrainId");

                    b.ToTable("Trains");
                });

            modelBuilder.Entity("MessageTrain", b =>
                {
                    b.Property<int>("MessagesMessageId")
                        .HasColumnType("int");

                    b.Property<int>("ReceiversTrainId")
                        .HasColumnType("int");

                    b.HasKey("MessagesMessageId", "ReceiversTrainId");

                    b.HasIndex("ReceiversTrainId");

                    b.ToTable("MessageTrain");
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.Switch", b =>
                {
                    b.HasBaseType("EtcsServer.Database.Entity.TrackageElement");

                    b.ToTable("Switch", (string)null);
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.Track", b =>
                {
                    b.HasBaseType("EtcsServer.Database.Entity.TrackageElement");

                    b.Property<double>("Gradient")
                        .HasColumnType("float");

                    b.Property<double>("Kilometer")
                        .HasColumnType("float");

                    b.Property<double>("Length")
                        .HasColumnType("float");

                    b.Property<int>("LineNumber")
                        .HasColumnType("int");

                    b.Property<double>("MaxDownSpeedMps")
                        .HasColumnType("float");

                    b.Property<double>("MaxUpSpeedMps")
                        .HasColumnType("float");

                    b.Property<string>("TrackNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TrackPosition")
                        .HasColumnType("int");

                    b.ToTable("Track", (string)null);
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.Crossing", b =>
                {
                    b.HasOne("EtcsServer.Database.Entity.Track", "Track")
                        .WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Track");
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.RailroadSign", b =>
                {
                    b.HasOne("EtcsServer.Database.Entity.Track", "Track")
                        .WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Track");
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.RailwaySignal", b =>
                {
                    b.HasOne("EtcsServer.Database.Entity.Track", "Track")
                        .WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Track");
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.SwitchRoute", b =>
                {
                    b.HasOne("EtcsServer.Database.Entity.Switch", "Switch")
                        .WithMany()
                        .HasForeignKey("SwitchId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EtcsServer.Database.Entity.Track", "TrackFrom")
                        .WithMany()
                        .HasForeignKey("TrackFromId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EtcsServer.Database.Entity.Track", "TrackTo")
                        .WithMany()
                        .HasForeignKey("TrackToId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Switch");

                    b.Navigation("TrackFrom");

                    b.Navigation("TrackTo");
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.TrackageElement", b =>
                {
                    b.HasOne("EtcsServer.Database.Entity.TrackageElement", "LeftSideElement")
                        .WithMany()
                        .HasForeignKey("LeftSideElementId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("EtcsServer.Database.Entity.TrackageElement", "RightSideElement")
                        .WithMany()
                        .HasForeignKey("RightSideElementId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("LeftSideElement");

                    b.Navigation("RightSideElement");
                });

            modelBuilder.Entity("MessageTrain", b =>
                {
                    b.HasOne("EtcsServer.Database.Entity.Message", null)
                        .WithMany()
                        .HasForeignKey("MessagesMessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EtcsServer.Database.Entity.Train", null)
                        .WithMany()
                        .HasForeignKey("ReceiversTrainId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.Switch", b =>
                {
                    b.HasOne("EtcsServer.Database.Entity.TrackageElement", null)
                        .WithOne()
                        .HasForeignKey("EtcsServer.Database.Entity.Switch", "TrackageElementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EtcsServer.Database.Entity.Track", b =>
                {
                    b.HasOne("EtcsServer.Database.Entity.TrackageElement", null)
                        .WithOne()
                        .HasForeignKey("EtcsServer.Database.Entity.Track", "TrackageElementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
