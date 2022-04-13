﻿using System.ComponentModel.DataAnnotations;

namespace Dressca.Web.Dto.Accounting;

/// <summary>
///  会計情報を表す DTO です。
/// </summary>
public class AccountDto
{
    /// <summary>
    ///  消費税率を取得または設定します。
    /// </summary>
    [Required]
    public decimal ConsumptionTaxRate { get; set; }

    /// <summary>
    ///  注文アイテムの税抜き合計金額を取得または設定します。
    /// </summary>
    [Required]
    public decimal TotalItemsPrice { get; set; }

    /// <summary>
    ///  送料を取得または設定します。
    /// </summary>
    [Required]
    public decimal DeliveryCharge { get; set; }

    /// <summary>
    ///  消費税額を取得または設定します。
    /// </summary>
    [Required]
    public decimal ConsumptionTax { get; set; }

    /// <summary>
    ///  送料、税込みの合計金額を取得または設定します。
    /// </summary>
    [Required]
    public decimal TotalPrice { get; set; }
}
