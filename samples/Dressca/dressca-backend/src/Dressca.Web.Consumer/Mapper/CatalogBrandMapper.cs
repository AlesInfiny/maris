﻿using System.Diagnostics.CodeAnalysis;
using Dressca.ApplicationCore.Catalog;
using Dressca.SystemCommon.Mapper;
using Dressca.Web.Consumer.Dto.Catalog;

namespace Dressca.Web.Consumer.Mapper;

/// <summary>
///  <see cref="CatalogBrand"/> と <see cref="CatalogBrandResponse"/> のマッパーです。
/// </summary>
public class CatalogBrandMapper : IObjectMapper<CatalogBrand, CatalogBrandResponse>
{
    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(value))]
    public CatalogBrandResponse? Convert(CatalogBrand? value)
    {
        if (value is null)
        {
            return null;
        }

        return new() { Id = value.Id, Name = value.Name };
    }
}
