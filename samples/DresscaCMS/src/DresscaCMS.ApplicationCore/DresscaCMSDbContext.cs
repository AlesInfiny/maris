using Microsoft.EntityFrameworkCore;

namespace DresscaCMS.ApplicationCore
{
    public class DresscaCMSDbContext : DbContext
    {
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<AnnouncementContent> AnnouncementsContent { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ArgumentNullException.ThrowIfNull(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Dressca.CMS;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var id1 = new Guid("a2c651dc-e000-473e-b064-8a1e399f93a7");
            var id2 = new Guid("704945e2-ab1f-4a46-b853-d14f09e38ed4");
            var id3 = new Guid("c738b93c-06ef-44be-9bb8-87fe0c3f6151");
            var id4 = new Guid("5030af08-13b9-478f-96d3-77e68225ebec");
            var id5 = new Guid("8721cd7a-f8cb-4d00-b5c8-53ce5eeac062");
            var id6 = new Guid("da1cc949-385e-44b3-a7d1-3f489cf2347d");
            var id7 = new Guid("9eabc9d8-39af-472b-afbc-1f7e5ac61899");
            var id8 = new Guid("f46e232c-ce77-408d-924e-aeaff126a639");
            var id9 = new Guid("85355a45-24d1-4b1d-98cd-d073f2ccfa02");
            var id10 = new Guid("06d6b6c6-906c-4de8-a02a-fd0f448d9d2f");
            var id11 = new Guid("c948318a-44cf-4e3a-b4a2-33a382570283");
            var id12 = new Guid("450a2931-f066-4f64-920a-25d2804c4951");
            var id13 = new Guid("3ed8bba8-fe95-477c-89ac-0abb586affae");
            var id14 = new Guid("5ff8f1f1-29f2-4c55-ad12-90004bcb3003");
            var id15 = new Guid("c8180f90-fd55-4d81-9f1e-0946d5c3d75d");
            var id16 = new Guid("f83041bb-acb0-4e73-a87f-60646616b6a3");
            var id17 = new Guid("bf62d39d-338d-423e-9f49-e97605c8d157");
            var id18 = new Guid("01182620-90c4-498e-b0a5-1a8b740ff421");
            var id19 = new Guid("e16b13a2-8698-4bb1-97dc-2fc64448e2fd");
            var id20 = new Guid("cfdbc43c-fae1-4a43-9772-e3bef8c955dd");

            var date1 = new DateTimeOffset(2024, 9, 1, 2, 0, 0, TimeSpan.FromHours(9));
            var date2 = new DateTimeOffset(2024, 9, 15, 14, 30, 0, TimeSpan.FromHours(9));

            modelBuilder.Entity<Announcement>().HasData(
                new Announcement { Id = id1, Category = "メンテナンス", DisplayPriority = 3, PostDateTime = date1, ExpireDateTime = date2, IsDeleted = false },
                new Announcement { Id = id2, Category = "障害情報", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false },
                new Announcement { Id = id3, Category = "新機能・サービス", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false },
                new Announcement { Id = id4, Category = "イベント情報", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false, ExpireDateTime = date2 },
                new Announcement { Id = id5, Category = "会社情報", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false },
                new Announcement { Id = id6, Category = "会社情報", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false },
                new Announcement { Id = id7, Category = "メンテナンス", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false, ExpireDateTime = date2 },
                new Announcement { Id = id8, Category = "障害情報", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false },
                new Announcement { Id = id9, Category = "新機能・サービス", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false },
                new Announcement { Id = id10, Category = "イベント情報", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false, ExpireDateTime = date2 },
                new Announcement { Id = id11, Category = "会社情報", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false },
                new Announcement { Id = id12, Category = "会社情報", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false, ExpireDateTime = date2 },
                new Announcement { Id = id13, Category = "障害情報", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false },
                new Announcement { Id = id14, Category = "新機能・サービス", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false },
                new Announcement { Id = id15, Category = "イベント情報", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false, ExpireDateTime = date2 },
                new Announcement { Id = id16, Category = "重要なお知らせ", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false },
                new Announcement { Id = id17, Category = "メンテナンス", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false, ExpireDateTime = date2 },
                new Announcement { Id = id18, Category = "障害情報", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false },
                new Announcement { Id = id19, Category = "イベント情報", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false, ExpireDateTime = date2 },
                new Announcement { Id = id20, Category = "重要なお知らせ", DisplayPriority = 3, PostDateTime = date1, IsDeleted = false }
            );

            var acud1 = new Guid("a1e2b3c4-5d6f-4a7b-8c9d-0e1f2a3b4c5d");
            var acud2 = new Guid("b2c3d4e5-6f7a-4b8c-9d0e-1f2a3b4c5d6e");
            var acud3 = new Guid("c3d4e5f6-7a8b-4c9d-0e1f-2a3b4c5d6e7f");
            var acud4 = new Guid("d4e5f6a7-8b9c-4d0e-1f2a-3b4c5d6e7f8a");
            var acud5 = new Guid("e5f6a7b8-9c0d-4e1f-2a3b-4c5d6e7f8a9b");
            var acud6 = new Guid("f6a7b8c9-0d1e-4f2a-3b4c-5d6e7f8a9b0c");
            var acud7 = new Guid("a7b8c9d0-1e2f-4a3b-4c5d-6e7f8a9b0c1d");
            var acud8 = new Guid("b8c9d0e1-2f3a-4b4c-5d6e-7f8a9b0c1d2e");
            var acud9 = new Guid("c9d0e1f2-3a4b-4c5d-6e7f-8a9b0c1d2e3f");
            var acud10 = new Guid("d0e1f2a3-4b5c-4d6e-7f8a-9b0c1d2e3f4a");
            var acud11 = new Guid("e1f2a3b4-5c6d-4e7f-8a9b-0c1d2e3f4a5b");
            var acud12 = new Guid("f2a3b4c5-6d7e-4f8a-9b0c-1d2e3f4a5b6c");
            var acud13 = new Guid("a3b4c5d6-7e8f-4a9b-0c1d-2e3f4a5b6c7d");
            var acud14 = new Guid("b4c5d6e7-8f9a-4b0c-1d2e-3f4a5b6c7d8e");
            var acud15 = new Guid("c5d6e7f8-9a0b-4c1d-2e3f-4a5b6c7d8e9f");
            var acud16 = new Guid("d6e7f8a9-0b1c-4d2e-3f4a-5b6c7d8e9f0a");
            var acud17 = new Guid("e7f8a9b0-1c2d-4e3f-4a5b-6c7d8e9f0a1b");
            var acud18 = new Guid("f8a9b0c1-2d3e-4f4a-5b6c-7d8e9f0a1b2c");
            var acud19 = new Guid("a9b0c1d2-3e4f-4a5b-6c7d-8e9f0a1b2c3d");
            var acud20 = new Guid("b0c1d2e3-4f5a-4b6c-7d8e-9f0a1b2c3d4e");

            modelBuilder.Entity<AnnouncementContent>().HasData(
                new AnnouncementContent { Id = acud1, AnnouncementId = id1, LanguageCode = "jp", Title = "メンテナンスのお知らせ", Message = "9月25日午前2時から5時まで、システムメンテナンスのためサービスを一時停止いたします。ご利用のお客様にはご不便をおかけいたします。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud2, AnnouncementId = id2, LanguageCode = "jp", Title = "障害情報（ログイン）", Message = "現在、一部のユーザーにおいてログインできない障害が発生しております。原因を調査中であり、復旧まで今しばらくお待ちください。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud3, AnnouncementId = id3, LanguageCode = "jp", Title = "ダッシュボード機能追加のお知らせ", Message = "新しい会員向けダッシュボード機能を公開しました。利用状況やお知らせを一目で確認できるようになり、利便性が向上しました。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud4, AnnouncementId = id4, LanguageCode = "jp", Title = "オンラインセミナーのご案内", Message = "10月10日に開催されるオンラインセミナー「最新クラウド活用術」の参加受付を開始しました。ぜひお申し込みください。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud5, AnnouncementId = id5, LanguageCode = "jp", Title = "本社移転のお知らせ", Message = "本社オフィスは11月1日より新住所へ移転いたします。詳細な所在地やアクセス方法は会社概要ページをご確認ください。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud6, AnnouncementId = id6, LanguageCode = "jp", Title = "年末年始の営業について", Message = "年末年始休業期間は12月29日から1月3日までとなります。休業中のお問い合わせは1月4日以降に順次対応いたします。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud7, AnnouncementId = id7, LanguageCode = "jp", Title = "メンテナンスのお知らせ", Message = "定期メンテナンスを9月30日22時より実施いたします。作業中はログインや一部機能がご利用いただけませんので、あらかじめご了承ください。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud8, AnnouncementId = id8, LanguageCode = "jp", Title = "障害情報（メール送信）", Message = "メール送信システムに不具合が発生し、通知メールが届かない状況です。復旧対応を進めておりますのでご不便をおかけいたします。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud9, AnnouncementId = id9, LanguageCode = "jp", Title = "決済方法追加のお知らせ", Message = "本日よりオンライン決済にクレジットカード以外に電子マネー決済が追加されました。ぜひ新しい決済方法をご利用ください。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud10, AnnouncementId = id10, LanguageCode = "jp", Title = "秋のキャンペーンイベントのご案内", Message = "秋のキャンペーンイベントを10月1日より実施いたします。期間中は特別割引や限定コンテンツを用意しておりますのでご期待ください。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud11, AnnouncementId = id11, LanguageCode = "jp", Title = "2026年度新卒採用について", Message = "2026年度新卒採用エントリーの受付を開始しました。募集要項や応募方法の詳細は採用情報ページをご確認ください。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud12, AnnouncementId = id12, LanguageCode = "jp", Title = "中途採用募集について", Message = "中途採用にてシステムエンジニア職の募集を開始しました。経験者の方はぜひエントリーフォームよりご応募ください。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud13, AnnouncementId = id13, LanguageCode = "jp", Title = "障害情報（決済）", Message = "決済システムに遅延が発生しており、処理に時間がかかる状況です。復旧作業中のため、完了次第改めてお知らせいたします。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud14, AnnouncementId = id14, LanguageCode = "jp", Title = "プッシュ通知機能追加のお知らせ", Message = "スマートフォンアプリに「プッシュ通知」機能を追加しました。最新情報をリアルタイムでお届けしますのでぜひご活用ください。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud15, AnnouncementId = id15, LanguageCode = "jp", Title = "ユーザー交流イベントのご案内", Message = "当社主催のユーザー交流イベントを11月15日に東京本社で開催いたします。詳細や参加方法は後日お知らせいたします。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud16, AnnouncementId = id16, LanguageCode = "jp", Title = "当社をかたる不審なメールへのご注意", Message = "現在、当社を装った不審なメールが確認されております。添付ファイルの開封やURLのクリックは行わず、削除していただきますようお願いいたします。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud17, AnnouncementId = id17, LanguageCode = "jp", Title = "メンテナンスのお知らせ", Message = "サーバー保守作業のため、10月5日深夜にサービスが断続的にご利用いただけなくなります。ご理解とご協力をお願いいたします。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud18, AnnouncementId = id18, LanguageCode = "jp", Title = "障害情報（スマホアプリ）", Message = "スマートフォンアプリでのプッシュ通知が受信できない障害が発生しております。対応中ですので、復旧まで今しばらくお待ちください。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud19, AnnouncementId = id19, LanguageCode = "jp", Title = "冬の感謝フェアーのご案内", Message = "12月1日より「冬の感謝フェア」を開催いたします。限定グッズの配布や特典をご用意しておりますので、ぜひご参加ください。", LinkedUrl = "" },
                new AnnouncementContent { Id = acud20, AnnouncementId = id20, LanguageCode = "jp", Title = "天候不良に伴う配送遅延について", Message = "現在の天候不良の影響により、一部地域で商品のお届けに遅延が発生しております。ご利用のお客様にはご迷惑をおかけしますが、何卒ご了承ください。", LinkedUrl = "" }
            );

        }
    }
}
