using System.Xml;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Data.Links;
using OxDAOEngine.Data.Types;
using OxDAOEngine.XML;
using PlayStationGames.AccountEngine.Data;
using PlayStationGames.AccountEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data;
using PlayStationGames.ConsoleEngine.Data.Fields;
using PlayStationGames.ConsoleEngine.Data.Types;
using PlayStationGames.GameEngine.Data.Decorator;
using PlayStationGames.GameEngine.Data.Fields;
using PlayStationGames.GameEngine.Data.Types;

namespace PlayStationGames.GameEngine.Data
{
    public class Game : RootDAO<GameField>
    {
        private Guid id = Guid.NewGuid();
        private string edition = string.Empty;
        private Guid owner = Guid.Empty;
        private PlatformType platformType = TypeHelper.DefaultValue<PlatformType>();
        private GameFormat format = TypeHelper.DefaultValue<GameFormat>();
        private Source sourceType = TypeHelper.DefaultValue<Source>();
        private GameRegion region = TypeHelper.DefaultValue<GameRegion>();
        private GameLanguage language = TypeHelper.DefaultValue<GameLanguage>();
        private ScreenView screenView = TypeHelper.DefaultValue<ScreenView>();
        private string code = string.Empty;
        private bool verified = false;
        private bool licensed = true;
        private string emulatorType = string.Empty;
        private string roms = string.Empty;
        private string genreName = string.Empty;
        private bool singlePlayer = true;
        private bool coachMultiplayer = false;
        private bool onlineMultiplayer = false;
        private string developer = string.Empty;
        private string publisher = string.Empty;
        private int year;
        private Pegi pegi = TypeHelper.DefaultValue<Pegi>();
        private int criticScore;
        public readonly ListDAO<Installation> Installations = new();
        public readonly ListDAO<DLC> Dlcs = new();
        public readonly ListDAO<Tag> Tags = new();
        public readonly Links<GameField> Links = new();
        public readonly RelatedGames RelatedGames = new();
        public readonly Trophyset Trophyset = new();

        public readonly ListDAO<Series> Serieses = new()
        {
            XmlName = "Serieses"
        };


        public readonly Platforms ReleasePlatforms = new() 
        { 
            XmlName = "ReleasePlatforms",
            SaveEmptyList = false
        };

        public Guid Id
        {
            get => id;
            set => ModifyValue(GameField.Id, id, value, n=> id = GuidValue(n));
        }

        public string EmulatorType
        {
            get => emulatorType;
            set => ModifyValue(GameField.EmulatorType, emulatorType, value, n => emulatorType = StringValue(n));
        }

        public string ROMs
        {
            get => roms;
            set => ModifyValue(GameField.EmulatorROMs, roms, value, n => roms = StringValue(n));
        }

        public string GenreName
        {
            get => genreName;
            set => ModifyValue(GameField.Genre, genreName, value,n => genreName = StringValue(n));
        }

        public bool SinglePlayer
        {
            get => singlePlayer;
            set => ModifyValue(GameField.SinglePlayer, singlePlayer, value, n => singlePlayer = BoolValue(n));
        }

        public bool CoachMultiplayer
        {
            get => coachMultiplayer;
            set => ModifyValue(GameField.CoachMultiplayer, coachMultiplayer, value, n => coachMultiplayer = BoolValue(n));
        }

        public bool OnlineMultiplayer
        {
            get => onlineMultiplayer;
            set => ModifyValue(GameField.OnlineMultiplayer, onlineMultiplayer, value, n => onlineMultiplayer = BoolValue(n));
        }

        public ScreenView ScreenView
        {
            get => screenView;
            set => ModifyValue(GameField.ScreenView, screenView, value, n => screenView = n);
        }

        public string Developer
        {
            get => developer;
            set => ModifyValue(GameField.Developer, developer, value, n => developer = StringValue(n));
        }

        public string Publisher
        {
            get => publisher;
            set => ModifyValue(GameField.Publisher, publisher, value, n => publisher = StringValue(n));
        }

        public int Year
        {
            get => year;
            set => ModifyValue(GameField.Year, year, value, n => year = n);
        }

        public Pegi Pegi
        {
            get => pegi;
            set => ModifyValue(GameField.Pegi, pegi, value, n => pegi = n);
        }

        public int CriticScore
        {
            get => criticScore;
            set => ModifyValue(GameField.CriticScore, criticScore, value, n => criticScore = n);
        }

        public string OriginalName => 
            Name.Replace(" (Copy)", string.Empty);

        public string Edition
        {
            get => edition;
            set => ModifyValue(GameField.Edition, edition, value, n => edition = StringValue(n));
        }

        public bool Verified
        {
            get => verified;
            set => ModifyValue(GameField.Verified, verified, value, n => verified = BoolValue(n));
        }

        public bool Licensed
        {
            get => licensed;
            set => ModifyValue(GameField.Licensed, licensed, value, n => licensed = BoolValue(n));
        }

        private string? oldAccountName;

        public Guid Owner
        {
            get => owner;
            set
            {
                Guid oldOwnerId = owner;

                ModifyValue(
                    GameField.Owner,
                    owner,
                    value,
                    n => owner = GuidValue(n),
                    oldAccountName
                );

                SetOldOwnerName(oldOwnerId);
            }
        }

        private void SetOldOwnerName(Guid oldOwnerId)
        {
            Account? account = DataManager.ListController<AccountField, Account>().Item(AccountField.Id, oldOwnerId);
            oldAccountName = account?.Name;
        }

        public override int CompareField(GameField field, IFieldMapping<GameField> y)
        {
            switch (field)
            {
                case GameField.Image:
                case GameField.Name:
                    return base.CompareField(field, y);
                case GameField.Id:
                case GameField.Owner:
                case GameField.Edition:
                case GameField.Genre:
                case GameField.Developer:
                case GameField.Publisher:
                case GameField.EmulatorType:
                case GameField.EmulatorROMs:
                    return StringValue(this[field]).CompareTo(StringValue(y[field]));
                case GameField.Verified:
                case GameField.Licensed:
                case GameField.SinglePlayer:
                case GameField.CoachMultiplayer:
                case GameField.OnlineMultiplayer:
                    return BoolValue(this[field]).CompareTo(BoolValue(y[field]));
                case GameField.AvailablePlatinum:
                case GameField.AvailableGold:
                case GameField.AvailableSilver:
                case GameField.AvailableBronze:
                case GameField.Year:
                case GameField.CriticScore:
                    return IntValue(this[field]).CompareTo(IntValue(y[field]));
            }

            if (y is Game yGame)
                return field switch
                {
                    GameField.Platform => platformType.CompareTo(yGame.PlatformType),
                    GameField.Format => format.CompareTo(yGame.Format),
                    GameField.TrophysetType => Trophyset.Type.CompareTo(yGame.Trophyset.Type),
                    GameField.Source => sourceType.CompareTo(yGame.SourceType),
                    GameField.Pegi => Pegi.CompareTo(yGame.Pegi),
                    GameField.ReleasePlatforms => ReleasePlatforms.CompareTo(yGame.ReleasePlatforms),
                    GameField.Difficult => Trophyset.Difficult.CompareTo(yGame.Trophyset.Difficult),
                    GameField.CompleteTime => Trophyset.CompleteTime.CompareTo(yGame.Trophyset.CompleteTime),
                    GameField.ScreenView => ScreenView.CompareTo(yGame.ScreenView),
                    GameField.Region => GameRegion.CompareTo(yGame.GameRegion),
                    GameField.Language => GameLanguage.CompareTo(yGame.GameLanguage),
                    GameField.Dlcs => Dlcs.CompareTo(yGame.Dlcs),
                    GameField.Tags => Tags.CompareTo(yGame.Tags),
                    GameField.Series => Serieses.CompareTo(yGame.Serieses),
                    GameField.Links => Links.CompareTo(yGame.Links),
                    GameField.Installations => Installations.CompareTo(yGame.Installations),
                    GameField.RelatedGames => RelatedGames.CompareTo(yGame.RelatedGames),
                    _ => 0,
                };

            return base.CompareField(field, y);
        }

        public PlatformType PlatformType
        {
            get => platformType;
            set => ModifyValue(GameField.Platform, platformType, value, n => platformType = n);
        }

        public GameFormat Format
        {
            get => format;
            set => ModifyValue(GameField.Format, format, value, n => format = n);
        }

        public Source SourceType
        {
            get => sourceType;
            set => ModifyValue(GameField.Source, sourceType, value, n => sourceType = n);
        }

        public GameRegion GameRegion
        {
            get => region;
            set => ModifyValue(GameField.Region, region, value, n => region = n);
        }

        public GameLanguage GameLanguage
        {
            get => language;
            set => ModifyValue(GameField.Language, language, value, n => language = n);
        }

        public string Code
        {
            get => code;
            set => ModifyValue(GameField.Code, code, value, n => code = StringValue(n));
        }

        public Game() : base() { }

        private static object? PrepareValueToSet(GameField field, object? value)
        {
            switch (field)
            {
                case GameField.Dlcs:
                    if (value is DLC dlc)
                        return new ListDAO<DLC>()
                        {
                            dlc
                        };
                    break;
                case GameField.Links:
                    if (value is Link<GameField> link)
                        return new Links<GameField>()
                        {
                            link
                        };
                    break;
                case GameField.Installations:
                    if (value is Installation installation)
                        return new ListDAO<Installation>()
                        {
                            installation
                        };
                    break;
                case GameField.RelatedGames:
                    if (value is RelatedGame relatedGame)
                        return new RelatedGames()
                        {
                            relatedGame
                        };
                    break;
                case GameField.Tags:
                    if (value is Tag tag)
                        return new ListDAO<Tag>()
                        {
                            tag
                        };
                    break;
                case GameField.Series:
                    if (value is Series series)
                        return new ListDAO<Series>()
                        {
                            series
                        };
                    break;
                case GameField.ReleasePlatforms:
                    if (value is Platform platform)
                        return new Platforms()
                        {
                            platform
                        };
                    break;
            }

            ITypeHelper? helper = TypeHelper.FieldHelper<GameField>().GetHelper(field);

            if (helper != null)
                return helper.Value(value);

            return field switch
            {
                GameField.AvailablePlatinum or
                GameField.Year or
                GameField.CriticScore =>
                    IntValue(value),
                _ => value,
            };
        }

        protected override void SetFieldValue(GameField field, object? value)
        {
            value = PrepareValueToSet(field, value);
            base.SetFieldValue(field, value);

            switch (field)
            {
                case GameField.Id:
                    Id = GuidValue(value);
                    break;
                case GameField.Edition:
                    Edition = StringValue(value);
                    break;
                case GameField.CriticScore:
                    CriticScore = IntValue(value);
                    break;
                case GameField.Platform:
                    PlatformType = TypeHelper.Value<PlatformType>(value);
                    break;
                case GameField.Region:
                    GameRegion = TypeHelper.Value<GameRegion>(value);
                    break;
                case GameField.Language:
                    GameLanguage = TypeHelper.Value<GameLanguage>(value);
                    break;
                case GameField.Code:
                    Code = StringValue(value);
                    break;
                case GameField.Format:
                    Format = TypeHelper.Value<GameFormat>(value);
                    break;
                case GameField.TrophysetType:
                    Trophyset.Type = TypeHelper.Value<TrophysetType>(value);
                    break;
                case GameField.AvailablePlatinum:
                    Trophyset.Available.Platinum = IntValue(value);
                    break;
                case GameField.AvailableGold:
                    Trophyset.Available.Gold = IntValue(value);
                    break;
                case GameField.AvailableSilver:
                    Trophyset.Available.Silver = IntValue(value);
                    break;
                case GameField.AvailableBronze:
                    Trophyset.Available.Bronze = IntValue(value);
                    break;
                case GameField.Source:
                    SourceType = TypeHelper.Value<Source>(value);
                    break;
                case GameField.Genre:
                    GenreName = StringValue(value);
                    break;
                case GameField.Developer:
                    Developer = StringValue(value);
                    break;
                case GameField.Publisher:
                    Publisher = StringValue(value);
                    break;
                case GameField.Year:
                    Year = IntValue(value);
                    break;
                case GameField.Pegi:
                    Pegi = TypeHelper.Value<Pegi>(value);
                    break;
                case GameField.ReleasePlatforms:
                    ReleasePlatforms.CopyFrom((DAO?)value);

                    if (ReleasePlatforms.IsEmpty)
                        ReleasePlatforms.Add(PlatformType);
                    break;
                case GameField.Verified:
                    Verified = BoolValue(value);
                    break;
                case GameField.Licensed:
                    Licensed = BoolValue(value);
                    break;
                case GameField.SinglePlayer:
                    SinglePlayer = BoolValue(value);
                    break;
                case GameField.CoachMultiplayer:
                    CoachMultiplayer = BoolValue(value);
                    break;
                case GameField.OnlineMultiplayer:
                    OnlineMultiplayer = BoolValue(value);
                    break;
                case GameField.Owner:
                    Owner = value is Account account ? account.Id : GuidValue(value);
                    break;
                case GameField.Difficult:
                    Trophyset.Difficult = TypeHelper.Value<Difficult>(value);
                    break;
                case GameField.CompleteTime:
                    Trophyset.CompleteTime = TypeHelper.Value<CompleteTime>(value);
                    break;
                case GameField.ScreenView:
                    ScreenView = TypeHelper.Value<ScreenView>(value);
                    break;
                case GameField.Dlcs:
                    Dlcs.CopyFrom((DAO?)value);
                    break;
                case GameField.Tags:
                    Tags.CopyFrom((DAO?)value);
                    break;
                case GameField.Series:
                    Serieses.CopyFrom((DAO?)value);
                    break;
                case GameField.Links:
                    Links.CopyFrom((DAO?)value);
                    break;
                case GameField.Installations:
                    Installations.CopyFrom((DAO?)value);
                    break;
                case GameField.RelatedGames:
                    SetRelatedGames((RelatedGames?)value);
                    break;
                case GameField.EmulatorType:
                    EmulatorType = StringValue(value);
                    break;
                case GameField.EmulatorROMs:
                    ROMs = StringValue(value);
                    break;
                case GameField.Trophyset:
                    Trophyset.CopyFrom((DAO?)value);
                    break;
            }

            Modified = true;
        }

        private readonly GameFieldHelper fieldHelper = TypeHelper.Helper<GameFieldHelper>();

        private void SetRelatedGames(RelatedGames? value)
        {
            if (value != null)
                foreach (RelatedGame relatedGame in RelatedGames)
                {
                    Game? game = DataManager.Item<GameField, Game>(GameField.Id, relatedGame.GameId);

                    if (game != null && 
                        value.GetById(relatedGame.GameId) == null)
                        game.RelatedGames.Remove(id);
                }

            RelatedGames.CopyFrom(value);
        }

        protected override object? GetFieldValue(GameField field) => 
            field switch
            {
                GameField.Id => id,
                GameField.Name or GameField.Image => 
                    base.GetFieldValue(field),
                GameField.Edition => edition,
                GameField.PlatformFamily => PlatformFamily.Sony,
                GameField.Platform => platformType,
                GameField.Format => format,
                GameField.Trophyset => Trophyset,
                GameField.TrophysetType => Trophyset.Type,
                GameField.Difficult => Trophyset.Difficult,
                GameField.CompleteTime => Trophyset.CompleteTime,
                GameField.AvailablePlatinum => Trophyset.Available.Platinum,
                GameField.AvailableGold => Trophyset.Available.Gold,
                GameField.AvailableSilver => Trophyset.Available.Silver,
                GameField.AvailableBronze => Trophyset.Available.Bronze,
                GameField.Source => sourceType,
                GameField.Region => region,
                GameField.Language => language,
                GameField.Code => code,
                GameField.Developer => Developer,
                GameField.Publisher => Publisher,
                GameField.Year => Year,
                GameField.Pegi => Pegi,
                GameField.CriticScore => CriticScore,
                GameField.ReleasePlatforms => ReleasePlatforms,
                GameField.Verified => Verified,
                GameField.Owner => Owner,
                GameField.Licensed => Licensed,
                GameField.Installations => Installations,
                GameField.Genre => GenreName,
                GameField.ScreenView => ScreenView,
                GameField.SinglePlayer => SinglePlayer,
                GameField.CoachMultiplayer => CoachMultiplayer,
                GameField.OnlineMultiplayer => OnlineMultiplayer,
                GameField.Dlcs => Dlcs,
                GameField.Tags => Tags,
                GameField.Series => Serieses,
                GameField.Links => Links,
                GameField.RelatedGames => RelatedGames,
                GameField.EmulatorType => EmulatorType,
                GameField.EmulatorROMs => ROMs,
                _ => new GameCardDecorator(this)[field],
            };

        public override void Clear()
        {
            if (State is
                DAOState.Creating or
                DAOState.Loading)
                return;

            base.Clear();
            id = Guid.NewGuid();
            edition = string.Empty;
            verified = false;
            licensed = true;
            owner = Guid.Empty;
            roms = string.Empty;
            emulatorType = string.Empty;
            genreName = string.Empty;
            singlePlayer = true;
            coachMultiplayer = false;
            onlineMultiplayer = false;
            developer = string.Empty;
            publisher = string.Empty;
            code = string.Empty;
            year = GameConsts.Empty_Year;
            criticScore = GameConsts.Empty_CriticScore;

            platformType = TypeHelper.DefaultValue<PlatformType>();
            format = TypeHelper.Helper<GameFormatHelper>().DefaultFormat(PlatformType);
            screenView = TypeHelper.DefaultValue<ScreenView>();
            sourceType = TypeHelper.DefaultValue<Source>();
            region = TypeHelper.DefaultValue<GameRegion>();
            language = TypeHelper.DefaultValue<GameLanguage>();
            pegi = TypeHelper.DefaultValue<Pegi>();
            
            ReleasePlatforms.Clear();
            Trophyset.Clear();
            Installations.Clear();
            Dlcs.Clear();
            Tags.Clear();
            Serieses.Clear();
            Links.Clear();
            RelatedGames.Clear();
        }

        public override void Init()
        {
            AddMember(Trophyset);
            AddListMember(GameField.Dlcs, Dlcs);
            AddListMember(GameField.Tags, Tags);
            AddListMember(GameField.Series, Serieses);
            AddListMember(GameField.Installations, Installations);
            AddListMember(GameField.Links, Links);
            AddListMember(GameField.RelatedGames, RelatedGames);
            AddListMember(GameField.ReleasePlatforms, ReleasePlatforms);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            base.SaveData(element, clearModified);
            XmlHelper.AppendElement(element, XmlConsts.Id, Id);

            if (Owner != Guid.Empty)
                XmlHelper.AppendElement(element, XmlConsts.Owner, Owner);

            XmlHelper.AppendElement(element, XmlConsts.Edition, Edition, true);

            if (SourceType != TypeHelper.DefaultValue<Source>())
                XmlHelper.AppendElement(element, XmlConsts.Source, SourceType);

            XmlHelper.AppendElement(element, XmlConsts.Genre, GenreName, true);
            XmlHelper.AppendElement(element, XmlConsts.Screen, ScreenView);

            if (SinglePlayer)
                XmlHelper.AppendElement(element, XmlConsts.SinglePlayer, SinglePlayer);

            if (CoachMultiplayer)
                XmlHelper.AppendElement(element, XmlConsts.CoachMultiplayer, CoachMultiplayer);

            if (OnlineMultiplayer)
                XmlHelper.AppendElement(element, XmlConsts.OnlineMultiplayer, OnlineMultiplayer);

            XmlHelper.AppendElement(element, XmlConsts.Region, GameRegion);
            XmlHelper.AppendElement(element, XmlConsts.Language, GameLanguage);
            XmlHelper.AppendElement(element, XmlConsts.Code, Code, true);
            XmlHelper.AppendElement(element, XmlConsts.Platform, PlatformType);
            XmlHelper.AppendElement(element, XmlConsts.Format, Format);

            if (Verified)
                XmlHelper.AppendElement(element, XmlConsts.Verified, Verified);

            XmlHelper.AppendElement(element, XmlConsts.Licensed, Licensed);
            XmlHelper.AppendElement(element, XmlConsts.EmulatorType, EmulatorType, true);
            XmlHelper.AppendElement(element, XmlConsts.ROMs, ROMs, true);
            XmlHelper.AppendElement(element, XmlConsts.Screen, ScreenView);
            XmlHelper.AppendElement(element, XmlConsts.Developer, Developer, true);
            XmlHelper.AppendElement(element, XmlConsts.Publisher, Publisher, true);
            XmlHelper.AppendElement(element, XmlConsts.Year, Year);
            XmlHelper.AppendElement(element, XmlConsts.PEGI, Pegi);

            if (CriticScore >= 0)
                XmlHelper.AppendElement(element, XmlConsts.CriticScore, CriticScore);
        }

        protected override void LoadData(XmlElement element)
        {
            base.LoadData(element);
            id = XmlHelper.ValueGuid(element, XmlConsts.Id, true);
            edition = XmlHelper.Value(element, XmlConsts.Edition);
            platformType = XmlHelper.Value<PlatformType>(element, XmlConsts.Platform);
            format = XmlHelper.Value<GameFormat>(element, XmlConsts.Format);
            verified = XmlHelper.ValueBool(element, XmlConsts.Verified);
            licensed = (XmlHelper.Value(element, XmlConsts.Licensed) == string.Empty) 
                || XmlHelper.ValueBool(element, XmlConsts.Licensed);
            owner = XmlHelper.ValueGuid(element, XmlConsts.Owner);
            SetOldOwnerName(owner);
            sourceType = XmlHelper.Value<Source>(element, XmlConsts.Source);
            region = XmlHelper.Value<GameRegion>(element, XmlConsts.Region);
            language = XmlHelper.Value<GameLanguage>(element, XmlConsts.Language);
            code = XmlHelper.Value(element, XmlConsts.Code);
            emulatorType = XmlHelper.Value(element, XmlConsts.EmulatorType);
            roms = XmlHelper.Value(element, XmlConsts.ROMs);
            screenView = XmlHelper.Value<ScreenView>(element, XmlConsts.Screen);
            genreName = XmlHelper.Value(element, XmlConsts.Genre);
            singlePlayer = XmlHelper.ValueBool(element, XmlConsts.SinglePlayer);
            coachMultiplayer = XmlHelper.ValueBool(element, XmlConsts.CoachMultiplayer);
            onlineMultiplayer = XmlHelper.ValueBool(element, XmlConsts.OnlineMultiplayer);
            developer = XmlHelper.Value(element, XmlConsts.Developer);
            publisher = XmlHelper.Value(element, XmlConsts.Publisher);
            year = XmlHelper.ValueInt(element, XmlConsts.Year);
            pegi = XmlHelper.Value<Pegi>(element, XmlConsts.PEGI);
            criticScore = XmlHelper.ValueInt(element, XmlConsts.CriticScore);
        }

        public override int CompareTo(DAO? other)
        {
            if (Equals(other))
                return 0;

            if (other == null)
                return 1;

            Game otherGame = (Game)other;

            int result = Name.CompareTo(otherGame.Name);

            if (result != 0)
                return result;

            if (PlatformType > otherGame.PlatformType)
                return 1;
                
            if (PlatformType < otherGame.PlatformType)
                return -1;

            if (SourceType > otherGame.SourceType)
                return 1;
                
            if (SourceType < otherGame.SourceType)
                return -1;

            if (GameRegion > otherGame.GameRegion)
                return 1;

            if (GameRegion < otherGame.GameRegion)
                return -1;

            return 0;
        }

        public override bool Equals(object? obj) => 
            base.Equals(obj)
            && obj is Game otherGame
            && Id.Equals(otherGame.Id)
            && Edition.Equals(otherGame.Edition)
            && PlatformType.Equals(otherGame.PlatformType)
            && Format.Equals(otherGame.Format)
            && Licensed.Equals(otherGame.Licensed)
            && Owner.Equals(otherGame.Owner)
            && Verified.Equals(otherGame.Verified)
            && SourceType.Equals(otherGame.SourceType)
            && EmulatorType.Equals(otherGame.EmulatorType)
            && ROMs.Equals(otherGame.ROMs)
            && ScreenView.Equals(otherGame.ScreenView)
            && GenreName.Equals(otherGame.GenreName)
            && SinglePlayer.Equals(otherGame.SinglePlayer)
            && CoachMultiplayer.Equals(otherGame.CoachMultiplayer)
            && OnlineMultiplayer.Equals(otherGame.OnlineMultiplayer)
            && Trophyset.Equals(otherGame.Trophyset)
            && Installations.Equals(otherGame.Installations)
            && Dlcs.Equals(otherGame.Dlcs)
            && Tags.Equals(otherGame.Tags)
            && Serieses.Equals(otherGame.Serieses)
            && Links.Equals(otherGame.Links)
            && RelatedGames.Equals(otherGame.RelatedGames)
            && Developer.Equals(otherGame.Developer)
            && Publisher.Equals(otherGame.Publisher)
            && Year.Equals(otherGame.Year)
            && Pegi.Equals(otherGame.Pegi)
            && CriticScore.Equals(otherGame.CriticScore)
            && ReleasePlatforms.Equals(otherGame.ReleasePlatforms)
            && GameRegion.Equals(otherGame.GameRegion)
            && GameLanguage.Equals(otherGame.GameLanguage)
            && Code.Equals(otherGame.Code);

        public override int GetHashCode() => 
            id.GetHashCode();

        public override string FullTitle() =>
            $"{Name}  ({TypeHelper.ShortName(PlatformType)}, {TypeHelper.Name(SourceType)}, {TypeHelper.Name(GameRegion)})";

        public override object ParseCaldedValue(GameField field, string value) => 
            field switch
            {
                GameField.AvailablePlatinum or
                GameField.Year => 
                    int.Parse(value),
                GameField.Region or
                GameField.Language or
                GameField.Platform or
                GameField.Format or
                GameField.Source or
                GameField.Pegi or
                GameField.Difficult or
                GameField.CompleteTime or
                GameField.ScreenView =>
                    FieldHelper.GetHelper(field)!.Parse(value),
                _ => value,
            };

        public override bool IsCalcedField(GameField field) =>
            fieldHelper.CalcedFields.Contains(field);

        public static bool CheckUniqueTrophyset(Game game, RootListDAO<GameField, Game> list)
        {
            //TODO: need implementation
            return false;
        }

        public static IMatcher<ConsoleField> AvailableConsoleFilter(ControlBuilder<GameField, Game> gameControlBuilder)
        {
            Filter<ConsoleField, PSConsole> filter = new();

            bool licensed = gameControlBuilder.Value<bool>(GameField.Licensed);

            foreach (ConsoleGeneration generation in
                platformTypeHelper.Generations(
                    gameControlBuilder.Value<PlatformType>(GameField.Platform),
                    gameControlBuilder.Value<Source>(GameField.Source),
                    licensed)
                )
                filter.AddFilter(ConsoleField.Generation, generation, FilterConcat.OR);

            if (licensed)
            {
                if (sourceHelper.IsPSN(gameControlBuilder[GameField.Source].EnumValue<Source>()))
                    filter.AddFilter(
                        ConsoleField.Accounts,
                        FilterOperation.Contains,
                        new ConsoleAccounts() 
                        { 
                            new ConsoleAccount() 
                            { 
                                Id = gameControlBuilder[GameField.Owner].GuidValue 
                            } 
                        },
                        FilterConcat.AND
                    );
            }
            else
                filter.AddFilter(ConsoleField.Firmware, FirmwareType.Custom, FilterConcat.AND);

            return filter;
        }

        private static readonly PlatformTypeHelper platformTypeHelper = TypeHelper.Helper<PlatformTypeHelper>();
        private static readonly GameFormatHelper formatHelper = TypeHelper.Helper<GameFormatHelper>();
        private static readonly SourceHelper sourceHelper = TypeHelper.Helper<SourceHelper>();

        public bool AccountAvailable => 
            Licensed
                ? platformTypeHelper.IsPSNPlatform(PlatformType)
                    && sourceHelper.IsPSN(SourceType)
                : PlatformType == PlatformType.PSVita
                    && SourceType == Source.PKGj;

        public bool TrophysetAvailable =>
            formatHelper.AvailableTrophies(Format)
            && platformTypeHelper.PlatformWithTrophies(PlatformType);

        public override Color BaseColor => sourceHelper.BaseColor(SourceType);
    }
}