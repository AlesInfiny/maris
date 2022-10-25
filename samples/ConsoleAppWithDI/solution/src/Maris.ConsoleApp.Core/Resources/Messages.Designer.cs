﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Maris.ConsoleApp.Core.Resources {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Maris.ConsoleApp.Core.Resources.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   {0} コマンドの非同期実行が {1} で完了しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string CommandExecutor_ExecutedAsyncCommand {
            get {
                return ResourceManager.GetString("CommandExecutor_ExecutedAsyncCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0} コマンドの同期実行が {1} で完了しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string CommandExecutor_ExecutedSyncCommand {
            get {
                return ResourceManager.GetString("CommandExecutor_ExecutedSyncCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0} コマンドの非同期実行を開始します。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string CommandExecutor_ExecutingAsyncCommand {
            get {
                return ResourceManager.GetString("CommandExecutor_ExecutingAsyncCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0} コマンドの同期実行を開始します。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string CommandExecutor_ExecutingSyncCommand {
            get {
                return ResourceManager.GetString("CommandExecutor_ExecutingSyncCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0} コマンドのパラメーターの入力値検証が完了しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string CommandExecutor_ValidatedParameter {
            get {
                return ResourceManager.GetString("CommandExecutor_ValidatedParameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0} コマンドのパラメーターの入力値検証を実行します。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string CommandExecutor_ValidatingParameter {
            get {
                return ResourceManager.GetString("CommandExecutor_ValidatingParameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   コマンドのパラメーターに入力エラーがあります。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string InvalidCommandParameter {
            get {
                return ResourceManager.GetString("InvalidCommandParameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   パラメーターの入力エラー情報詳細 : {0} 。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string InvalidCommandParameterDetails {
            get {
                return ResourceManager.GetString("InvalidCommandParameterDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   指定されたコマンドの型 {0} は {1} または {2} を実装していません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string InvalidCommandType {
            get {
                return ResourceManager.GetString("InvalidCommandType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   コンソールアプリケーションの実行コンテキストに設定されているパラメーター {0} のオブジェクトを、コマンドクラスの実装時に指定した型 {1} として取り扱うことができません。 {2} コマンドまたはそのパラメーターの実装を修正してください。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string InvalidParameterType {
            get {
                return ResourceManager.GetString("InvalidParameterType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0} クラスの {1} メソッドを呼び出しましたが null を取得しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string MethodReturnNull {
            get {
                return ResourceManager.GetString("MethodReturnNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   指定したパラメーターのクラス {0} に {1} 属性が追加されていません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string NoCustomAttributesAddedToParameterClass {
            get {
                return ResourceManager.GetString("NoCustomAttributesAddedToParameterClass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0} が初期化されていません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string NotInitialized {
            get {
                return ResourceManager.GetString("NotInitialized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   成功({0}) に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SuccessResult {
            get {
                return ResourceManager.GetString("SuccessResult", resourceCulture);
            }
        }
    }
}
