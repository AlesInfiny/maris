using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Maris.ConsoleApp.Core.Resources;

namespace Maris.ConsoleApp.Core;

/// <summary>
///  コマンドの基本機能を提供します。
/// </summary>
public abstract class CommandBase
{
    private ConsoleAppContext? context;

    /// <summary>
    ///  <see cref="CommandBase"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    protected CommandBase()
    {
    }

    /// <summary>
    ///  コマンドの名前を取得します。
    /// </summary>
    internal virtual string? CommandName => this.context?.CommandName;

    /// <summary>
    ///  コンソールアプリケーションの実行コンテキストを取得します。
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///  <list type="bullet">
    ///   <item>
    ///    <see cref="Context"/> が初期化されていません。
    ///    値を取得する前に <see cref="Initialize(ConsoleAppContext)"/> メソッドを呼び出してください。
    ///   </item>
    ///  </list>
    /// </exception>
    internal ConsoleAppContext Context
        => this.context ?? throw new InvalidOperationException(Messages.NotInitialized.Embed(nameof(this.Context)));

    /// <summary>
    ///  コマンドを初期化します。
    /// </summary>
    /// <param name="context">コンソールアプリケーションの実行コンテキスト。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="context"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///  <list type="bullet">
    ///   <item>指定したコンソールアプリケーションの実行コンテキストは、このコマンドにとって正しい値ではありません。</item>
    ///  </list>
    /// </exception>
    public void Initialize(ConsoleAppContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.ValidateContext(context);
    }

    /// <summary>
    ///  すべての入力パラメーターを検証します。
    ///  内部で <see cref="ValidateParameter"/> メソッドを呼び出します。
    /// </summary>
    /// <exception cref="InvalidParameterException">コマンドのパラメーターに入力エラーがあります。</exception>
    /// <exception cref="InvalidOperationException">
    ///  <list type="bullet">
    ///   <item>
    ///    <see cref="Context"/> が初期化されていません。
    ///    値を取得する前に <see cref="Initialize(ConsoleAppContext)"/> メソッドを呼び出してください。
    ///   </item>
    ///  </list>
    /// </exception>
    internal void ValidateAllParameter()
    {
        var validationContext = new ValidationContext(this.Context.Parameter);
        var validationResults = new Collection<ValidationResult>();
        if (!Validator.TryValidateObject(this.Context.Parameter, validationContext, validationResults, true))
        {
            throw new InvalidParameterException(validationResults);
        }

        try
        {
            this.ValidateParameter();
        }
        catch (Exception ex)
        {
            throw new InvalidParameterException(Messages.InvalidCommandParameter, ex);
        }
    }

    /// <summary>
    ///  入力パラメーターのカスタム検証ロジックを実行します。
    ///  入力値検証でエラーが発生した場合は、例外をスローしてください。
    /// </summary>
    internal abstract void ValidateParameter();

    /// <summary>
    ///  指定したコンソールアプリケーションの実行コンテキストが、このコマンドにとって正しいか検証します。
    /// </summary>
    /// <param name="context">コンソールアプリケーションの実行コンテキスト。</param>
    /// <exception cref="InvalidOperationException">
    ///  <list type="bullet">
    ///   <item>指定したコンソールアプリケーションの実行コンテキストは、このコマンドにとって正しい値ではありません。</item>
    ///  </list>
    /// </exception>
    internal virtual void ValidateContext(ConsoleAppContext context)
    {
    }
}
