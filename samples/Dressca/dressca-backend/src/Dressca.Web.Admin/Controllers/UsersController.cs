﻿using Dressca.ApplicationCore.Authorization;
using Dressca.Web.Admin.Dto.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Dressca.Web.Admin.Controllers;

/// <summary>
/// ユーザーの情報にアクセスするコントローラーです。
/// </summary>
[Route("api/users")]
[ApiController]
[Produces("application/json")]
public class UsersController : Controller
{
    private readonly IUserStore userStore;

    /// <summary>
    ///   <see cref="UsersController"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="userStore">ユーザー情報のストア。</param>
    /// <exception cref="ArgumentNullException">
    ///  <list type="bullet">
    ///   <item><paramref name="userStore"/> が <see langword="null"/> です。</item>
    ///  </list>
    /// </exception>
    public UsersController(IUserStore userStore)
    {
        this.userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
    }

    /// <summary>
    /// ログイン中のユーザーの情報を取得します。
    /// </summary>
    /// <response code="200">成功。</response>
    /// <returns>ユーザーの情報。</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetLoginUserResponse), StatusCodes.Status200OK)]
    [OpenApiOperation("getLoginUser")]
    [Authorize]
    public GetLoginUserResponse GetLoginUser()
    {
        return new GetLoginUserResponse
        {
            UserName = this.userStore.LoginUserName,
            Roles = this.userStore.LoginUserRoles,
        };
    }
}
