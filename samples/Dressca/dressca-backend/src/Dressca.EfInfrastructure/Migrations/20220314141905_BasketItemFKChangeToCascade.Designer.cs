﻿// <auto-generated />
using System;
using Dressca.EfInfrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dressca.EfInfrastructure.Migrations
{
    [DbContext(typeof(DresscaDbContext))]
    [Migration("20220314141905_BasketItemFKChangeToCascade")]
    partial class BasketItemFKChangeToCascade
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Dressca.ApplicationCore.Baskets.Basket", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("BuyerId")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.ToTable("Baskets", (string)null);
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Baskets.BasketItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("BasketId")
                        .HasColumnType("bigint");

                    b.Property<long>("CatalogItemId")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,6)");

                    b.HasKey("Id");

                    b.HasIndex("BasketId");

                    b.ToTable("BasketItems", (string)null);
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Catalog.CatalogBrand", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.ToTable("CatalogBrands", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "高級なブランド"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "カジュアルなブランド"
                        },
                        new
                        {
                            Id = 3L,
                            Name = "ノーブランド"
                        });
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Catalog.CatalogCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.ToTable("CatalogCategories", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "服"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "バッグ"
                        },
                        new
                        {
                            Id = 3L,
                            Name = "シューズ"
                        });
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Catalog.CatalogItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("CatalogBrandId")
                        .HasColumnType("bigint");

                    b.Property<long>("CatalogCategoryId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,6)");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("CatalogBrandId");

                    b.HasIndex("CatalogCategoryId");

                    b.HasIndex("ProductCode");

                    b.ToTable("CatalogItems", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            CatalogBrandId = 3L,
                            CatalogCategoryId = 1L,
                            Description = "定番の無地ロングTシャツです。",
                            Name = "クルーネック Tシャツ - グレー",
                            Price = 1980m,
                            ProductCode = "C000000001"
                        },
                        new
                        {
                            Id = 2L,
                            CatalogBrandId = 2L,
                            CatalogCategoryId = 1L,
                            Description = "裏起毛で温かいパーカーです。",
                            Name = "無地 パーカー",
                            Price = 5800m,
                            ProductCode = "C000000002"
                        },
                        new
                        {
                            Id = 3L,
                            CatalogBrandId = 1L,
                            CatalogCategoryId = 1L,
                            Description = "ウール生地のテーラードジャケットです。セットアップだけでなく単体でも使いやすい商品となっています。",
                            Name = "テーラードジャケット",
                            Price = 49800m,
                            ProductCode = "C000000003"
                        },
                        new
                        {
                            Id = 4L,
                            CatalogBrandId = 2L,
                            CatalogCategoryId = 1L,
                            Description = "ファー襟付きのデニムジャケットです。",
                            Name = "デニムジャケット",
                            Price = 19800m,
                            ProductCode = "C000000004"
                        },
                        new
                        {
                            Id = 5L,
                            CatalogBrandId = 3L,
                            CatalogCategoryId = 2L,
                            Description = "シンプルなデザインのトートバッグです。",
                            Name = "トートバッグ",
                            Price = 18800m,
                            ProductCode = "B000000001"
                        },
                        new
                        {
                            Id = 6L,
                            CatalogBrandId = 2L,
                            CatalogCategoryId = 2L,
                            Description = "軽くしなやかなレザーを使用しています。",
                            Name = "ショルダーバッグ",
                            Price = 38000m,
                            ProductCode = "B000000002"
                        },
                        new
                        {
                            Id = 7L,
                            CatalogBrandId = 3L,
                            CatalogCategoryId = 2L,
                            Description = "外側は高級感のある牛革、内側に丈夫で傷つきにくい豚革を採用した細身で持ち運びやすいビジネスバッグです。",
                            Name = "ビジネスバッグ",
                            Price = 24800m,
                            ProductCode = "B000000003"
                        },
                        new
                        {
                            Id = 8L,
                            CatalogBrandId = 1L,
                            CatalogCategoryId = 2L,
                            Description = "丁寧に編み込まれた馬革ハンドバッグです。落ち着いた色調で、オールシーズン使いやすくなっています。",
                            Name = "編み込みトートバッグ",
                            Price = 58800m,
                            ProductCode = "B000000004"
                        },
                        new
                        {
                            Id = 9L,
                            CatalogBrandId = 1L,
                            CatalogCategoryId = 2L,
                            Description = "卓越した素材と緻密な縫製作業によって、デザインが完璧に表現されています。",
                            Name = "ハンドバッグ",
                            Price = 258000m,
                            ProductCode = "B000000005"
                        },
                        new
                        {
                            Id = 10L,
                            CatalogBrandId = 2L,
                            CatalogCategoryId = 3L,
                            Description = "定番のハイカットスニーカーです。",
                            Name = "ハイカットスニーカー - ブラック",
                            Price = 12800m,
                            ProductCode = "S000000001"
                        },
                        new
                        {
                            Id = 11L,
                            CatalogBrandId = 1L,
                            CatalogCategoryId = 3L,
                            Description = "イタリアの職人が丁寧に手作業で作り上げた一品です。",
                            Name = "メダリオン ストレートチップ ドレスシューズ",
                            Price = 23800m,
                            ProductCode = "S000000002"
                        });
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Ordering.Order", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("BuyerId")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTimeOffset>("OrderDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Ordering.OrderItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,6)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems", (string)null);
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Baskets.BasketItem", b =>
                {
                    b.HasOne("Dressca.ApplicationCore.Baskets.Basket", "Basket")
                        .WithMany("Items")
                        .HasForeignKey("BasketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_BasketItems_Baskets");

                    b.Navigation("Basket");
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Catalog.CatalogItem", b =>
                {
                    b.HasOne("Dressca.ApplicationCore.Catalog.CatalogBrand", "CatalogBrand")
                        .WithMany("Items")
                        .HasForeignKey("CatalogBrandId")
                        .IsRequired()
                        .HasConstraintName("FK_CatalogItems_CatalogBrands");

                    b.HasOne("Dressca.ApplicationCore.Catalog.CatalogCategory", "CatalogCategory")
                        .WithMany("Items")
                        .HasForeignKey("CatalogCategoryId")
                        .IsRequired()
                        .HasConstraintName("FK_CatalogItems_CatalogCategories");

                    b.Navigation("CatalogBrand");

                    b.Navigation("CatalogCategory");
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Ordering.Order", b =>
                {
                    b.OwnsOne("Dressca.ApplicationCore.Ordering.ShipTo", "ShipToAddress", b1 =>
                        {
                            b1.Property<long>("OrderId")
                                .HasColumnType("bigint");

                            b1.Property<string>("FullName")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("nvarchar(64)")
                                .HasColumnName("ShipToFullName");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");

                            b1.OwnsOne("Dressca.ApplicationCore.Ordering.Address", "Address", b2 =>
                                {
                                    b2.Property<long>("ShipToOrderId")
                                        .HasColumnType("bigint");

                                    b2.Property<string>("AzanaAndOthers")
                                        .IsRequired()
                                        .HasMaxLength(128)
                                        .HasColumnType("nvarchar(128)")
                                        .HasColumnName("ShipToAzanaAndOthers");

                                    b2.Property<string>("PostalCode")
                                        .IsRequired()
                                        .HasMaxLength(16)
                                        .HasColumnType("nvarchar(16)")
                                        .HasColumnName("ShipToPostalCode");

                                    b2.Property<string>("Shikuchoson")
                                        .IsRequired()
                                        .HasMaxLength(32)
                                        .HasColumnType("nvarchar(32)")
                                        .HasColumnName("ShipToShikuchoson");

                                    b2.Property<string>("Todofuken")
                                        .IsRequired()
                                        .HasMaxLength(16)
                                        .HasColumnType("nvarchar(16)")
                                        .HasColumnName("ShipToTodofuken");

                                    b2.HasKey("ShipToOrderId");

                                    b2.ToTable("Orders");

                                    b2.WithOwner()
                                        .HasForeignKey("ShipToOrderId");
                                });

                            b1.Navigation("Address")
                                .IsRequired();
                        });

                    b.Navigation("ShipToAddress")
                        .IsRequired();
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Ordering.OrderItem", b =>
                {
                    b.HasOne("Dressca.ApplicationCore.Ordering.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .IsRequired()
                        .HasConstraintName("FK_OrderItems_Orders");

                    b.OwnsOne("Dressca.ApplicationCore.Ordering.CatalogItemOrdered", "ItemOrdered", b1 =>
                        {
                            b1.Property<long>("OrderItemId")
                                .HasColumnType("bigint");

                            b1.Property<long>("CatalogItemId")
                                .HasColumnType("bigint")
                                .HasColumnName("OrderedCatalogItemId");

                            b1.Property<string>("ProductCode")
                                .IsRequired()
                                .HasMaxLength(128)
                                .HasColumnType("nvarchar(128)")
                                .HasColumnName("OrderedProductCode");

                            b1.Property<string>("ProductName")
                                .IsRequired()
                                .HasMaxLength(512)
                                .HasColumnType("nvarchar(512)")
                                .HasColumnName("OrderedProductName");

                            b1.HasKey("OrderItemId");

                            b1.ToTable("OrderItems");

                            b1.WithOwner()
                                .HasForeignKey("OrderItemId");
                        });

                    b.Navigation("ItemOrdered")
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Baskets.Basket", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Catalog.CatalogBrand", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Catalog.CatalogCategory", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Ordering.Order", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
