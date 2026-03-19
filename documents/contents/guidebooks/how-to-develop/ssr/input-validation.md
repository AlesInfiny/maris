---
title: 入力値検証の実装 （SSR 編）
description: SSR アプリケーション開発における 入力値検証の実装方法を解説します。
---

# 入力値検証 {#top}

ここでは、アプリケーションアーキテクチャで定義している入力値検査の実装方法を説明します。
入力値検査方針については [こちら](../../../app-architecture/server-side-rendering/global-function/validation-policy.md) を参照してください。

## 単項目チェックの実装 {#single-item-validation}

<!-- textlint-disable ja-technical-writing/sentence-length -->
Blazor Web アプリでは、 `EditForm` と属性（ `Attribute` ）ベースの検証を組み合わせることで、単項目チェックを実現します。
詳細は [フォーム検証 :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/forms/validation#form-validation){ target=_blank } および [データ注釈検証コンポーネントとカスタム検証 :material-open-in-new:](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/forms/validation#data-annotations-validator-component-and-custom-validation){ target=_blank } を参照してください。
<!-- textlint-enable ja-technical-writing/sentence-length -->

大まかな実装の流れを以下に示します。

1. 検証結果として表示するエラーメッセージを定義する。

    [メッセージ管理方針](../../../app-architecture/server-side-rendering/global-function/message-management-policy.md) の内容に従ってメッセージを作成します。

    ??? example "メッセージリソースの作成例"

        ![メッセージリソース](../../../images/guidebooks/how-to-develop/ssr/input-validation-message-resources.png){ loading=lazy }

1. ビューモデルを作成する。

    プロパティを持つビューモデルのクラスを作成します。プロパティは入力フォームの各項目となります。

    ??? example "ビューモデルの実装例"

        ```C#
        public class Student
        {
            public Guid Id { get; set; } = Guid.Empty;

            public string StudentNumber { get; set; } = string.Empty;

            public string FirstName { get; set; } = string.Empty;

            public string LastName { get; set; } = string.Empty;

            public int Age { get; set; }

            public int EnrollmentYear { get; set; }

            public int? GraduationYear { get; set; }
        }
        ```

1. ビューモデルの各プロパティに検証属性を付与することで、検証内容や表示するエラーメッセージを定義する。

    ??? example "ビューモデルに検証属性を付与した例"

        ```C#
        public class Student
        {
            public Guid Id { get; set; } = Guid.Empty;

            [Display(Name = "学生番号")]
            [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.RequiredToInput))]
            [StringLength(10, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.OverMaxStringLength))]
            public string StudentNumber { get; set; } = string.Empty;

            [Display(Name = "姓")]
            [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.RequiredToInput))]
            [StringLength(20, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.OverMaxStringLength))]
            public string FirstName { get; set; } = string.Empty;

            [Display(Name = "名")]
            [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.RequiredToInput))]
            [StringLength(20, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.OverMaxStringLength))]
            public string LastName { get; set; } = string.Empty;

            [Display(Name = "年齢")]
            [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.RequiredToInput))]
            [Range(0, 150, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.InvalidRange))]
            public int Age { get; set; }

            [Display(Name = "入学年")]
            [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.RequiredToInput))]
            [Range(1980, 2100, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.InvalidRange))]
            public int EnrollmentYear { get; set; }

            [Display(Name = "卒業年")]
            [Range(1980, 2100, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = nameof(Messages.InvalidRange))]
            public int? GraduationYear { get; set; }
        }
        ```

1. Razor コンポーネントに `EditForm` を用いて入力フォームを作成する。
1. `EditForm` にビューモデルをバインドする。

    ??? example "Razor コンポーネントに EditForm を追加した例"

        ```razor
        @page "/students/register"

        <PageTitle>RegisterStudent</PageTitle>

        <h1>学生登録</h1>

        <EditForm FormName="RegisterStudentForm" EditContext="@editContext" OnValidSubmit="HandleValidSubmit">
            <DataAnnotationsValidator />

            <div class="mb-3">
                <label class="form-label">学生番号</label>
                <InputText class="form-control" @bind-Value="model.StudentNumber" />
                <ValidationMessage For="() => model.StudentNumber" />
            </div>

            <div class="mb-3">
                <label class="form-label">姓</label>
                <InputText class="form-control" @bind-Value="model.FirstName" />
                <ValidationMessage For="() => model.FirstName" />
            </div>

            <div class="mb-3">
                <label class="form-label">名</label>
                <InputText class="form-control" @bind-Value="model.LastName" />
                <ValidationMessage For="() => model.LastName" />
            </div>

            <div class="mb-3">
                <label class="form-label">年齢</label>
                <InputNumber class="form-control" @bind-Value="model.Age" />
                <ValidationMessage For="() => model.Age" />
            </div>

            <div class="mb-3">
                <label class="form-label">入学年</label>
                <InputNumber class="form-control" @bind-Value="model.EnrollmentYear" />
                <ValidationMessage For="() => model.EnrollmentYear" />
            </div>

            <div class="mb-3">
                <label class="form-label">卒業年</label>
                <InputNumber class="form-control" @bind-Value="model.GraduationYear" />
                <ValidationMessage For="() => model.GraduationYear" />
            </div>

            <button type="submit" class="btn btn-primary">登録</button>

            <div class="mt-3">
                <ValidationSummary />
            </div>
        </EditForm>

        @code {
            private Student model = new();
            private EditContext editContext = default!;

            protected override void OnInitialized()
            {
                editContext = new EditContext(model);
            }

            private void HandleValidSubmit()
            {
                // ビジネスロジックの呼び出しなど
            }
        }
        ```

## 項目間チェック・複合チェック結果の ValidationSummary への反映 {#validation-summary}

項目間チェックは `OnValidSubmit` イベントハンドラー内で実装します。複合チェックは、アプリケーションコア層の業務ロジック内で実装します。
ここでは、これらの結果をビューの `ValidationSummary` に表示する方法を説明します。

1. `@code` で `ValidationMessageStore` の変数を宣言します。
1. `OnInitialized` または `OnInitializedAsync` メソッド内で上の変数を初期化します。
1. `OnValidSubmit` イベントハンドラーで以下の処理を追加します。
    1. `ValidationMessageStore` をクリアします。
    1. 項目間チェック・複合チェック結果のエラーメッセージを `ValidationMessageStore` に追加します。
    1. `EditContext.NotifyValidationStateChanged` メソッドを呼び出し、入力値検証の状態が変わったことを伝えます。

??? example "項目間チェック・複合チェックの結果を ValidationSummary へ反映する例"

    ```C# hl_lines="4 9 14 20 21 31 32"
    @code {
        private Student model = new();
        private EditContext editContext = default!;
        private ValidationMessageStore validationMessageStore = default!;

        protected override void OnInitialized()
        {
            editContext = new EditContext(model);
            validationMessageStore = new ValidationMessageStore(editContext);
        }

        private void HandleValidSubmit()
        {
            validationMessageStore.Clear();

            var studentEntity = ConvertToEntity(model);

            if(model.GraduationYear < model.EnrollmentYear) // 項目間チェックの実施
            {
                validationMessageStore.Add(new FieldIdentifier(model, string.Empty), "卒業年は入学年以降の年を入力してください。");
                editContext.NotifyValidationStateChanged();
                return;
            }

            try
            {
                // ビジネスロジックの呼び出し（複合チェックの実施）
            }
            catch (DuplicatedStudentNumberException ex)
            {
                validationMessageStore.Add(new FieldIdentifier(model, string.Empty), "同じ学籍番号の生徒がすでに存在します。");
                editContext.NotifyValidationStateChanged();
                return;
            }

            // 以降のコードは省略
        }
    }
    ```

## 入れ子になった ViewModel での入力値検証 {#validate-nested-view-models}
