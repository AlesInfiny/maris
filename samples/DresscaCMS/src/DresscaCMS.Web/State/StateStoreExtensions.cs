using DresscaCMS.Web.State;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///  ステート管理サービスの拡張メソッドを提供します。
/// </summary>
public static class StateStoreExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        ///  ステートストアのインメモリ実装を DI コンテナーに登録します。
        /// </summary>
        /// <returns>追加済みのサービスコレクション。</returns>
        public IServiceCollection AddInMemoryStateStore()
        {
            services.AddScoped<IStateStore, InMemoryStateStore>();
            return services;
        }
    }
}
