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
    [Migration("20220414134435_AddNowPrintingAsset")]
    partial class AddNowPrintingAsset
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Dressca.ApplicationCore.Assets.Asset", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("AssetCode")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("AssetType")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("AssetCode");

                    b.ToTable("Assets", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            AssetCode = "b52dc7f712d94ca5812dd995bf926c04",
                            AssetType = "png"
                        },
                        new
                        {
                            Id = 2L,
                            AssetCode = "80bc8e167ccb4543b2f9d51913073492",
                            AssetType = "png"
                        },
                        new
                        {
                            Id = 3L,
                            AssetCode = "05d38fad5693422c8a27dd5b14070ec8",
                            AssetType = "png"
                        },
                        new
                        {
                            Id = 4L,
                            AssetCode = "45c22ba3da064391baac91341067ffe9",
                            AssetType = "png"
                        },
                        new
                        {
                            Id = 5L,
                            AssetCode = "4aed07c4ed5d45a5b97f11acedfbb601",
                            AssetType = "png"
                        },
                        new
                        {
                            Id = 6L,
                            AssetCode = "082b37439ecc44919626ba00fc60ee85",
                            AssetType = "png"
                        },
                        new
                        {
                            Id = 7L,
                            AssetCode = "f5f89954281747fa878129c29e1e0f83",
                            AssetType = "png"
                        },
                        new
                        {
                            Id = 8L,
                            AssetCode = "a8291ef2e8e14869a7048e272915f33c",
                            AssetType = "png"
                        },
                        new
                        {
                            Id = 9L,
                            AssetCode = "66237018c769478a90037bd877f5fba1",
                            AssetType = "png"
                        },
                        new
                        {
                            Id = 10L,
                            AssetCode = "d136d4c81b86478990984dcafbf08244",
                            AssetType = "png"
                        },
                        new
                        {
                            Id = 11L,
                            AssetCode = "47183f32f6584d7fb661f9216e11318b",
                            AssetType = "png"
                        },
                        new
                        {
                            Id = 12L,
                            AssetCode = "cf151206efd344e1b86854f4aa49fdef",
                            AssetType = "png"
                        },
                        new
                        {
                            Id = 13L,
                            AssetCode = "ab2e78eb7fe3408aadbf1e17a9945a8c",
                            AssetType = "png"
                        },
                        new
                        {
                            Id = 14L,
                            AssetCode = "0e557e96bc054f10bc91c27405a83e85",
                            AssetType = "png"
                        },
                        new
                        {
                            Id = 15L,
                            AssetCode = "e622b0098808492cb883831c05486b58",
                            AssetType = "png"
                        });
                });

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
                            Name = "クルーネック Tシャツ - ブラック",
                            Price = 1980m,
                            ProductCode = "C000000001"
                        },
                        new
                        {
                            Id = 2L,
                            CatalogBrandId = 2L,
                            CatalogCategoryId = 1L,
                            Description = "暖かいのに着膨れしない起毛デニムです。",
                            Name = "裏起毛 スキニーデニム",
                            Price = 4800m,
                            ProductCode = "C000000002"
                        },
                        new
                        {
                            Id = 3L,
                            CatalogBrandId = 1L,
                            CatalogCategoryId = 1L,
                            Description = "あたたかく肌ざわりも良いウール100%のロングコートです。",
                            Name = "ウールコート",
                            Price = 49800m,
                            ProductCode = "C000000003"
                        },
                        new
                        {
                            Id = 4L,
                            CatalogBrandId = 2L,
                            CatalogCategoryId = 1L,
                            Description = "コットン100%の柔らかい着心地で、春先から夏、秋口まで万能に使いやすいです。",
                            Name = "無地 ボタンダウンシャツ",
                            Price = 2800m,
                            ProductCode = "C000000004"
                        },
                        new
                        {
                            Id = 5L,
                            CatalogBrandId = 3L,
                            CatalogCategoryId = 2L,
                            Description = "コンパクトサイズのバッグですが収納力は抜群です",
                            Name = "レザーハンドバッグ",
                            Price = 18800m,
                            ProductCode = "B000000001"
                        },
                        new
                        {
                            Id = 6L,
                            CatalogBrandId = 2L,
                            CatalogCategoryId = 2L,
                            Description = "エイジング加工したレザーを使用しています。",
                            Name = "ショルダーバッグ",
                            Price = 38000m,
                            ProductCode = "B000000002"
                        },
                        new
                        {
                            Id = 7L,
                            CatalogBrandId = 3L,
                            CatalogCategoryId = 2L,
                            Description = "春の季節にぴったりのトートバッグです。インナーポーチまたは単体でも使用可能なポーチ付。",
                            Name = "トートバッグ ポーチ付き",
                            Price = 24800m,
                            ProductCode = "B000000003"
                        },
                        new
                        {
                            Id = 8L,
                            CatalogBrandId = 1L,
                            CatalogCategoryId = 2L,
                            Description = "さらりと気軽に纏える、キュートなミニサイズショルダー。",
                            Name = "ショルダーバッグ",
                            Price = 2800m,
                            ProductCode = "B000000004"
                        },
                        new
                        {
                            Id = 9L,
                            CatalogBrandId = 1L,
                            CatalogCategoryId = 2L,
                            Description = "エレガントな雰囲気を放つキルティングデザインです。",
                            Name = "レザー チェーンショルダーバッグ",
                            Price = 258000m,
                            ProductCode = "B000000005"
                        },
                        new
                        {
                            Id = 10L,
                            CatalogBrandId = 2L,
                            CatalogCategoryId = 3L,
                            Description = "柔らかいソールは快適な履き心地で、ランニングに最適です。",
                            Name = "ランニングシューズ - ブルー",
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

            modelBuilder.Entity("Dressca.ApplicationCore.Catalog.CatalogItemAsset", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("AssetCode")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<long>("CatalogItemId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CatalogItemId");

                    b.ToTable("CatalogItemAssets", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            AssetCode = "45c22ba3da064391baac91341067ffe9",
                            CatalogItemId = 1L
                        },
                        new
                        {
                            Id = 2L,
                            AssetCode = "4aed07c4ed5d45a5b97f11acedfbb601",
                            CatalogItemId = 2L
                        },
                        new
                        {
                            Id = 3L,
                            AssetCode = "082b37439ecc44919626ba00fc60ee85",
                            CatalogItemId = 3L
                        },
                        new
                        {
                            Id = 4L,
                            AssetCode = "f5f89954281747fa878129c29e1e0f83",
                            CatalogItemId = 4L
                        },
                        new
                        {
                            Id = 5L,
                            AssetCode = "a8291ef2e8e14869a7048e272915f33c",
                            CatalogItemId = 5L
                        },
                        new
                        {
                            Id = 6L,
                            AssetCode = "66237018c769478a90037bd877f5fba1",
                            CatalogItemId = 6L
                        },
                        new
                        {
                            Id = 7L,
                            AssetCode = "d136d4c81b86478990984dcafbf08244",
                            CatalogItemId = 7L
                        },
                        new
                        {
                            Id = 8L,
                            AssetCode = "47183f32f6584d7fb661f9216e11318b",
                            CatalogItemId = 8L
                        },
                        new
                        {
                            Id = 9L,
                            AssetCode = "cf151206efd344e1b86854f4aa49fdef",
                            CatalogItemId = 9L
                        },
                        new
                        {
                            Id = 10L,
                            AssetCode = "ab2e78eb7fe3408aadbf1e17a9945a8c",
                            CatalogItemId = 10L
                        },
                        new
                        {
                            Id = 11L,
                            AssetCode = "0e557e96bc054f10bc91c27405a83e85",
                            CatalogItemId = 11L
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

                    b.Property<decimal>("ConsumptionTax")
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("ConsumptionTaxRate")
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("DeliveryCharge")
                        .HasColumnType("decimal(18,6)");

                    b.Property<DateTimeOffset>("OrderDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("TotalItemsPrice")
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,6)");

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

            modelBuilder.Entity("Dressca.ApplicationCore.Ordering.OrderItemAsset", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("AssetCode")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<long>("OrderItemId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OrderItemId");

                    b.ToTable("OrderItemAssets", (string)null);
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
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_CatalogItems_CatalogBrands");

                    b.HasOne("Dressca.ApplicationCore.Catalog.CatalogCategory", "CatalogCategory")
                        .WithMany("Items")
                        .HasForeignKey("CatalogCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_CatalogItems_CatalogCategories");

                    b.Navigation("CatalogBrand");

                    b.Navigation("CatalogCategory");
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Catalog.CatalogItemAsset", b =>
                {
                    b.HasOne("Dressca.ApplicationCore.Catalog.CatalogItem", "CatalogItem")
                        .WithMany("Assets")
                        .HasForeignKey("CatalogItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_CatalogItemAssets_CatalogItems");

                    b.Navigation("CatalogItem");
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
                        .OnDelete(DeleteBehavior.Cascade)
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

            modelBuilder.Entity("Dressca.ApplicationCore.Ordering.OrderItemAsset", b =>
                {
                    b.HasOne("Dressca.ApplicationCore.Ordering.OrderItem", "OrderItem")
                        .WithMany("Assets")
                        .HasForeignKey("OrderItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_OrderItemAssets_OrderItems");

                    b.Navigation("OrderItem");
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

            modelBuilder.Entity("Dressca.ApplicationCore.Catalog.CatalogItem", b =>
                {
                    b.Navigation("Assets");
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Ordering.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Dressca.ApplicationCore.Ordering.OrderItem", b =>
                {
                    b.Navigation("Assets");
                });
#pragma warning restore 612, 618
        }
    }
}
