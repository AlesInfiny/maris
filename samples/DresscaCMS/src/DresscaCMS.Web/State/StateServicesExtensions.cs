using System.Diagnostics.CodeAnalysis;
using DresscaCMS.Web.State;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///  揮発性のステート管理サービスの拡張メソッドを提供します。
/// </summary>
[SuppressMessage(
    category: "StyleCop.CSharp.ReadabilityRules",
    checkId: "SA1101:PrefixLocalCallsWithThis",
    Justification = "StyleCop bug. see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3954")]
public static class StateServicesExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        ///  揮発性ステートストアのインメモリ実装を DI コンテナーに登録します。
        /// </summary>
        /// <returns>追加済みのサービスコレクション。</returns>
        public IServiceCollection AddInMemoryConsumableStateStore()
        {
            services.AddScoped<IConsumableStateStore, InMemoryConsumableStateStore>();
            return services;
        }
    }
}
