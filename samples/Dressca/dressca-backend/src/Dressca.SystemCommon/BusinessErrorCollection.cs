using System.Collections;

namespace Dressca.SystemCommon;

/// <summary>
///  業務エラーのコレクションです。
/// </summary>
public class BusinessErrorCollection : IEnumerable<BusinessError>
{
    private readonly Dictionary<string, BusinessError> businessErrors = [];

    /// <summary>
    ///  <see cref="BusinessErrorCollection"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public BusinessErrorCollection()
    {
    }

    /// <summary>
    ///  件数を取得します。
    /// </summary>
    public int Count => this.businessErrors.Count;

    /// <summary>
    ///  指定した業務エラーの情報を追加またはマージします。
    ///  同じエラーコードの業務エラーが追加されている場合はマージされます。
    /// </summary>
    /// <param name="newBusinessError">追加する業務エラーの情報。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="newBusinessError"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public void AddOrMerge(BusinessError newBusinessError)
    {
        ArgumentNullException.ThrowIfNull(newBusinessError);
        if (this.businessErrors.TryGetValue(newBusinessError.ExceptionId, out var businessError))
        {
            businessError.AddErrorMessages([.. newBusinessError.ErrorMessages]);
        }
        else
        {
            this.businessErrors.Add(newBusinessError.ExceptionId, newBusinessError);
        }
    }

    /// <inheritdoc/>
    public IEnumerator<BusinessError> GetEnumerator() => this.businessErrors.Values.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    /// <inheritdoc/>
    public override string ToString() => $"[{string.Join(',', this.businessErrors.Values)}]";
}
