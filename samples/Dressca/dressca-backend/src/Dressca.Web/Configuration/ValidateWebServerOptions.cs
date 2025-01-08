using Microsoft.Extensions.Options;

namespace Dressca.Web.Configuration;

/// <summary>
///  <see cref="WebServerOptions"/> の入力値検証を行うための部分クラスです。
/// </summary>
[OptionsValidator]
public partial class ValidateWebServerOptions : IValidateOptions<WebServerOptions>
{
}
