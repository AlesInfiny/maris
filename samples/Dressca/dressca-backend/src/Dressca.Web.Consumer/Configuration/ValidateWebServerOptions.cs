﻿using Microsoft.Extensions.Options;

namespace Dressca.Web.Consumer.Configuration;

/// <summary>
///  <see cref="WebServerOptions"/> の部分クラスです。
/// </summary>
[OptionsValidator]
public partial class ValidateWebServerOptions : IValidateOptions<WebServerOptions>
{
}
