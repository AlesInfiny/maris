---
title: 入力値検証の実装 （SSR 編）
description: SSR アプリケーション開発における 入力値検証の実装方法を解説します。
---

# 入力値検証 {#top}

## Blazor Web アプリでの入力値検証の概要 {#about-validation-of-blazor-web-apps}

Blazor Web アプリでは、 `EditForm` と属性（ `Attribute` ）ベースの検証を組み合わせることで、入力値の検証を実現します。
大まかな実装の流れを以下に示します。

1. 検証結果として表示するエラーメッセージを定義する。

    [メッセージ管理方針](../../../app-architecture/server-side-rendering/global-function/message-management-policy.md) の内容に従ってメッセージを作成します。

    `Messages.RequiredToInput = "{0}は必須入力です。"`

1. ビューモデルを作成する。

    プロパティを持つビューモデルのクラスを作成します。プロパティは入力フォームの各項目となります。

    ??? example "ビューモデルの実装例"

        ```C#
        public class Blog
        {
            public Guid Id { get; set; } = Guid.Empty;
            public string Title { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
        }
        ```

1. ビューモデルの各プロパティに検証属性を付与することで、検証内容や表示するエラーメッセージを定義する。

    ??? example "ビューモデルに検証属性を付与した例"

        ```C#
        public class Blog
        {
            public Guid Id { get; set; } = Guid.Empty;

            [Display(Name = "タイトル")]
            [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.RequiredToInput))]
            public string Title { get; set; } = string.Empty;

            [Display(Name = "本文")]
            [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.RequiredToInput))]
            public string Content { get; set; } = string.Empty;
        }
        ```

1. Razor コンポーネントに `EditForm` を用いて入力フォームを作成する。
1. `EditForm` にビューモデルをバインドする。

    ??? example "Razor コンポーネントに EditForm を追加した例"

        ```razor
        @page "/blogs/create"
        @using BlazorApp1.ViewModels

        <PageTitle>BlogCreate</PageTitle>

        <h1>Create</h1>

        <EditForm FormName="BlogCreateForm" EditContext="@editContext" OnValidSubmit="HandleValidSubmit">
            <DataAnnotationsValidator />

            <div class="mb-3">
                <label class="form-label">タイトル</label>
                <InputText class="form-control" @bind-Value="model.Title" />
                <ValidationMessage For="() => model.Title" />
            </div>

            <div class="mb-3">
                <label class="form-label">本文</label>
                <InputTextArea class="form-control" rows="8" @bind-Value="model.Content" />
                <ValidationMessage For="() => model.Content" />
            </div>
            <div class="mb-3">
                <button type="submit" class="btn btn-primary">作成</button>
            </div>
            <div class="mb-3">
                <ValidationSummary />
            </div>
        </EditForm>

        @code {
            private EditContext editContext = default!;
            private Blog model = new();

            protected override void OnInitialized()
            {
                editContext = new EditContext(model);
            }

            private void HandleValidSubmit()
            {
                model.Id = Guid.NewGuid();

                // TODO: 保存処理（DB/API への登録など）をここに追加
            }
        }
        ```

## サーバーサイド検証結果の ValidationSummary への反映 {#validation-summary}

## 入れ子になった ViewModel での入力値検証 {#validate-nested-view-models}
