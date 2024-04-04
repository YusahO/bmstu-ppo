﻿// <auto-generated />
using System;
using MewingPad.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MewingPad.Database.Context.Migrations
{
    [DbContext(typeof(MewingPadDbContext))]
    partial class MewingPadDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MewingPad.Database.Models.AudiotrackDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<float>("Duration")
                        .HasColumnType("real")
                        .HasColumnName("duration");

                    b.Property<string>("Filepath")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("filepath");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Audiotracks");
                });

            modelBuilder.Entity("MewingPad.Database.Models.CommentaryDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AudiotrackId")
                        .HasColumnType("uuid")
                        .HasColumnName("audiotrack_id");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.HasKey("Id");

                    b.HasIndex("AudiotrackId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Commentaries");
                });

            modelBuilder.Entity("MewingPad.Database.Models.PlaylistAudiotrackDbModel", b =>
                {
                    b.Property<Guid>("AudiotrackId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PlaylistId")
                        .HasColumnType("uuid");

                    b.HasKey("AudiotrackId", "PlaylistId");

                    b.HasIndex("PlaylistId");

                    b.ToTable("PlaylistsAudiotracks");
                });

            modelBuilder.Entity("MewingPad.Database.Models.PlaylistDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasColumnName("title");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("MewingPad.Database.Models.ReportDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AudiotrackId")
                        .HasColumnType("uuid")
                        .HasColumnName("audiotrack_id");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("status");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.HasKey("Id");

                    b.HasIndex("AudiotrackId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("MewingPad.Database.Models.ScoreDbModel", b =>
                {
                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<Guid>("AudiotrackId")
                        .HasColumnType("uuid")
                        .HasColumnName("audiotrack_id");

                    b.Property<int>("Value")
                        .HasColumnType("integer")
                        .HasColumnName("value");

                    b.HasKey("AuthorId", "AudiotrackId");

                    b.HasIndex("AudiotrackId");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("MewingPad.Database.Models.TagAudiotrackDbModel", b =>
                {
                    b.Property<Guid>("AudiotrackId")
                        .HasColumnType("uuid")
                        .HasColumnName("audiotrack_id");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uuid")
                        .HasColumnName("tag_id");

                    b.HasKey("AudiotrackId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("TagsAudiotracks");
                });

            modelBuilder.Entity("MewingPad.Database.Models.TagDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("MewingPad.Database.Models.UserDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(320)")
                        .HasColumnName("email");

                    b.Property<Guid>("FavouritesId")
                        .HasColumnType("uuid")
                        .HasColumnName("favourties_id");

                    b.Property<Guid?>("FavouritesPlaylistId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bool")
                        .HasColumnName("is_admin");

                    b.Property<string>("PasswordHashed")
                        .IsRequired()
                        .HasColumnType("varchar(128)")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex("FavouritesId")
                        .IsUnique();

                    b.HasIndex("FavouritesPlaylistId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MewingPad.Database.Models.AudiotrackDbModel", b =>
                {
                    b.HasOne("MewingPad.Database.Models.UserDbModel", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("MewingPad.Database.Models.CommentaryDbModel", b =>
                {
                    b.HasOne("MewingPad.Database.Models.AudiotrackDbModel", "Audiotrack")
                        .WithMany()
                        .HasForeignKey("AudiotrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MewingPad.Database.Models.UserDbModel", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Audiotrack");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("MewingPad.Database.Models.PlaylistAudiotrackDbModel", b =>
                {
                    b.HasOne("MewingPad.Database.Models.AudiotrackDbModel", "Audiotrack")
                        .WithMany("PlaylistsAudiotracks")
                        .HasForeignKey("AudiotrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MewingPad.Database.Models.PlaylistDbModel", "Playlist")
                        .WithMany("PlaylistsAudiotracks")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Audiotrack");

                    b.Navigation("Playlist");
                });

            modelBuilder.Entity("MewingPad.Database.Models.PlaylistDbModel", b =>
                {
                    b.HasOne("MewingPad.Database.Models.UserDbModel", "User")
                        .WithMany("Playlists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MewingPad.Database.Models.ReportDbModel", b =>
                {
                    b.HasOne("MewingPad.Database.Models.AudiotrackDbModel", "Audiotrack")
                        .WithMany()
                        .HasForeignKey("AudiotrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MewingPad.Database.Models.UserDbModel", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Audiotrack");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("MewingPad.Database.Models.ScoreDbModel", b =>
                {
                    b.HasOne("MewingPad.Database.Models.AudiotrackDbModel", "Audiotrack")
                        .WithMany()
                        .HasForeignKey("AudiotrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MewingPad.Database.Models.UserDbModel", "Author")
                        .WithMany("Scores")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Audiotrack");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("MewingPad.Database.Models.TagAudiotrackDbModel", b =>
                {
                    b.HasOne("MewingPad.Database.Models.AudiotrackDbModel", "Audiotrack")
                        .WithMany("TagsAudiotracks")
                        .HasForeignKey("AudiotrackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MewingPad.Database.Models.TagDbModel", "Tag")
                        .WithMany("TagsAudiotracks")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Audiotrack");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("MewingPad.Database.Models.TagDbModel", b =>
                {
                    b.HasOne("MewingPad.Database.Models.UserDbModel", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("MewingPad.Database.Models.UserDbModel", b =>
                {
                    b.HasOne("MewingPad.Database.Models.PlaylistDbModel", "FavouritesPlaylist")
                        .WithMany()
                        .HasForeignKey("FavouritesPlaylistId");

                    b.Navigation("FavouritesPlaylist");
                });

            modelBuilder.Entity("MewingPad.Database.Models.AudiotrackDbModel", b =>
                {
                    b.Navigation("PlaylistsAudiotracks");

                    b.Navigation("TagsAudiotracks");
                });

            modelBuilder.Entity("MewingPad.Database.Models.PlaylistDbModel", b =>
                {
                    b.Navigation("PlaylistsAudiotracks");
                });

            modelBuilder.Entity("MewingPad.Database.Models.TagDbModel", b =>
                {
                    b.Navigation("TagsAudiotracks");
                });

            modelBuilder.Entity("MewingPad.Database.Models.UserDbModel", b =>
                {
                    b.Navigation("Playlists");

                    b.Navigation("Scores");
                });
#pragma warning restore 612, 618
        }
    }
}
