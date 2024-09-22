using System.Xml;
using OxDAOEngine.ControlFactory;
using OxDAOEngine.Data;
using OxDAOEngine.Data.Filter;
using OxDAOEngine.Data.Links;
using OxDAOEngine.Data.Types;
using OxDAOEngine.XML;
using PlayStationGames.AccountEngine.Data;
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
        private Guid id = Guid.Empty;
        private string edition = string.Empty;
        private string series = string.Empty;
        private Guid owner = Guid.Empty;
        private PlatformType platformType = TypeHelper.DefaultValue<PlatformType>();
        private GameFormat format = TypeHelper.DefaultValue<GameFormat>();
        private Source sourceType = TypeHelper.DefaultValue<Source>();
        private GameRegion region = TypeHelper.DefaultValue<GameRegion>();
        private GameLanguage language = TypeHelper.DefaultValue<GameLanguage>();
        private ScreenView screenView = TypeHelper.DefaultValue<ScreenView>();
        private Difficult difficult = TypeHelper.DefaultValue<Difficult>();
        private CompleteTime completeTime = TypeHelper.DefaultValue<CompleteTime>();
        private TrophysetAccess trophysetAccess = TypeHelper.DefaultValue<TrophysetAccess>();
        private string code = string.Empty;
        private bool verified = false;
        private bool licensed = true;
        private string emulatorType = string.Empty;
        private string roms = string.Empty;
        private string genre = string.Empty;
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
        public readonly ListDAO<GameMode> GameModes = new() 
        { 
            XmlName = "Modes",
            SaveEmptyList = false
        };

        public readonly Platforms ReleasePlatforms = new() 
        { 
            XmlName = "ReleasePlatforms",
            SaveEmptyList = false
        };

        public readonly TrophyList EarnedTrophies = new() 
        { 
            XmlName = "EarnedTrophies",
            SaveEmptyList = false
        };

        public TrophyList AvailableTrophies = new() 
        { 
            XmlName = "AvailableTrophies",
            SaveEmptyList = false
        };

        public bool RelaterGameUpdateProcess { get; set; }

        public Guid Id
        {
            get => id;
            set => id = GuidValue(ModifyValue(GameField.Id, id, value));
        }

        public string EmulatorType
        {
            get => emulatorType;
            set => emulatorType = StringValue(ModifyValue(GameField.EmulatorType, emulatorType, value));
        }

        public string ROMs
        {
            get => roms;
            set => roms = StringValue(ModifyValue(GameField.EmulatorROMs, roms, value));
        }

        public string Genre
        {
            get => genre;
            set => genre = StringValue(ModifyValue(GameField.Genre, genre, value));
        }

        public ScreenView ScreenView
        {
            get => screenView;
            set => screenView = ModifyValue(GameField.ScreenView, screenView, value);
        }

        public Difficult Difficult
        {
            get => difficult;
            set => difficult = ModifyValue(GameField.Difficult, difficult, value);
        }
        public CompleteTime CompleteTime
        {
            get => completeTime;
            set => completeTime = ModifyValue(GameField.CompleteTime, completeTime, value);
        }

        public TrophysetAccess TrophysetAccess
        {
            get => trophysetAccess;
            set => trophysetAccess = ModifyValue(GameField.TrophysetAccess, trophysetAccess, value);
        }

        public string Developer
        {
            get => developer;
            set => developer = StringValue(ModifyValue(GameField.Developer, developer, value));
        }

        public string Publisher
        {
            get => publisher;
            set => publisher = StringValue(ModifyValue(GameField.Publisher, publisher, value));
        }

        public int Year
        {
            get => year;
            set => year = ModifyValue(GameField.Year, year, value);
        }

        public Pegi Pegi
        {
            get => pegi;
            set => pegi = ModifyValue(GameField.Pegi, pegi, value);
        }

        public int CriticScore
        {
            get => criticScore;
            set => criticScore = ModifyValue(GameField.CriticScore, criticScore, value);
        }

        public string OriginalName => 
            Name.Replace(" (Copy)", string.Empty);

        public string Edition
        {
            get => edition;
            set => edition = StringValue(ModifyValue(GameField.Edition, edition, value));
        }

        public string Series
        {
            get => series;
            set => series = StringValue(ModifyValue(GameField.Series, series, value));
        }

        public bool Verified
        {
            get => verified;
            set => verified = BoolValue(ModifyValue(GameField.Verified, verified, value));
        }

        public bool Licensed
        {
            get => licensed;
            set => licensed = BoolValue(ModifyValue(GameField.Licensed, licensed, value));
        }

        public Guid Owner
        {
            get => owner;
            set => owner = GuidValue(ModifyValue(GameField.Owner, owner, value));
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
                case GameField.Series:
                case GameField.Developer:
                case GameField.Publisher:
                case GameField.Genre:
                case GameField.EmulatorType:
                case GameField.EmulatorROMs:
                    return StringValue(this[field]).CompareTo(StringValue(y[field]));
                case GameField.Verified:
                case GameField.Licensed:
                    return BoolValue(this[field]).CompareTo(BoolValue(y[field]));
                case GameField.EarnedPlatinum:
                case GameField.AvailablePlatinum:
                case GameField.EarnedGold:
                case GameField.EarnedSilver:
                case GameField.EarnedBronze:
                case GameField.EarnedFromDLC:
                case GameField.EarnedNet:
                case GameField.AvailableGold:
                case GameField.AvailableSilver:
                case GameField.AvailableBronze:
                case GameField.AvailableFromDLC:
                case GameField.AvailableNet:
                case GameField.Year:
                case GameField.CriticScore:
                    return IntValue(this[field]).CompareTo(IntValue(y[field]));
            }

            if (y is Game yGame)
                return field switch
                {
                    GameField.Platform => platformType.CompareTo(yGame.PlatformType),
                    GameField.Format => format.CompareTo(yGame.Format),
                    GameField.TrophysetAccess => TrophysetAccess.CompareTo(yGame.TrophysetAccess),
                    GameField.Source => sourceType.CompareTo(yGame.SourceType),
                    GameField.Pegi => Pegi.CompareTo(yGame.Pegi),
                    GameField.ReleasePlatforms => ReleasePlatforms.CompareTo(yGame.ReleasePlatforms),
                    GameField.Difficult => Difficult.CompareTo(yGame.Difficult),
                    GameField.CompleteTime => CompleteTime.CompareTo(yGame.CompleteTime),
                    GameField.ScreenView => ScreenView.CompareTo(yGame.ScreenView),
                    GameField.Region => GameRegion.CompareTo(yGame.GameRegion),
                    GameField.Language => GameLanguage.CompareTo(yGame.GameLanguage),
                    GameField.GameModes => GameModes.CompareTo(yGame.GameModes),
                    GameField.Dlcs => Dlcs.CompareTo(yGame.Dlcs),
                    GameField.Tags => Tags.CompareTo(yGame.Tags),
                    GameField.Links => Links.CompareTo(yGame.Links),
                    GameField.Installations => Installations.CompareTo(yGame.Installations),
                    GameField.RelatedGames => RelatedGames.CompareTo(yGame.RelatedGames),
                    GameField.Status => new GameCalculations(this).GetGameStatus().CompareTo(new GameCalculations(yGame).GetGameStatus()),
                    GameField.Progress => new GameCalculations(this).GetGameProgress().CompareTo(new GameCalculations(yGame).GetGameProgress()),
                    GameField.EarnedPoints => new GameCalculations(this).GetEarnedPoints().CompareTo(new GameCalculations(yGame).GetEarnedPoints()),
                    GameField.EarnedPointsOld => new GameCalculations(this).GetEarnedOldPoints().CompareTo(new GameCalculations(yGame)),
                    _ => 0,
                };

            return base.CompareField(field, y);
        }

        public PlatformType PlatformType
        {
            get => platformType;
            set => platformType = ModifyValue(GameField.Platform, platformType, value);
        }

        public GameFormat Format
        {
            get => format;
            set => format = ModifyValue(GameField.Format, format, value);
        }

        public Source SourceType
        {
            get => sourceType;
            set => sourceType = ModifyValue(GameField.Source, sourceType, value);
        }

        public GameRegion GameRegion
        {
            get => region;
            set => region = ModifyValue(GameField.Region, region, value);
        }

        public GameLanguage GameLanguage
        {
            get => language;
            set => language = ModifyValue(GameField.Language, language, value);
        }

        public string Code
        {
            get => code;
            set => code = StringValue(ModifyValue(GameField.Code, code, value));
        }

        public Game() : base() => 
            GenerateId();

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
                case GameField.GameModes:
                    if (value is GameMode gameMode)
                        return new ListDAO<GameMode>()
                        {
                            gameMode
                        };
                    break;
                case GameField.Tags:
                    if (value is Tag tag)
                        return new ListDAO<Tag>()
                        {
                            tag
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
                GameField.EarnedPlatinum or
                GameField.Year or
                GameField.CriticScore =>
                    IntValue(value),
                _ => value,
            };
        }

        protected override void SetFieldValue(GameField field, object? value)
        {
            value = PrepareValueToSet(field, value);

            if (!fieldHelper.AlwaysUpdateFields.Contains(field) &&
                !CheckValueModified(this[field], value))
                return;

            base.SetFieldValue(field, value);

            switch (field)
            {
                case GameField.Id:
                    Id = GuidValue(value);
                    break;
                case GameField.Edition:
                    Edition = StringValue(value);
                    break;
                case GameField.Series:
                    Series = StringValue(value);
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
                case GameField.TrophysetAccess:
                    TrophysetAccess = TypeHelper.Value<TrophysetAccess>(value);
                    break;
                case GameField.EarnedPlatinum:
                    EarnedTrophies.Platinum = IntValue(value);
                    break;
                case GameField.EarnedGold:
                    EarnedTrophies.Gold = IntValue(value);
                    break;
                case GameField.EarnedSilver:
                    EarnedTrophies.Silver = IntValue(value);
                    break;
                case GameField.EarnedBronze:
                    EarnedTrophies.Bronze = IntValue(value);
                    break;
                case GameField.EarnedFromDLC:
                    EarnedTrophies.FromDLC = IntValue(value);
                    break;
                case GameField.EarnedNet:
                    EarnedTrophies.Net = IntValue(value);
                    break;
                case GameField.AvailablePlatinum:
                    AvailableTrophies.Platinum = IntValue(value);
                    break;
                case GameField.AvailableGold:
                    AvailableTrophies.Gold = IntValue(value);
                    break;
                case GameField.AvailableSilver:
                    AvailableTrophies.Silver = IntValue(value);
                    break;
                case GameField.AvailableBronze:
                    AvailableTrophies.Bronze = IntValue(value);
                    break;
                case GameField.AvailableFromDLC:
                    AvailableTrophies.FromDLC = IntValue(value);
                    break;
                case GameField.AvailableNet:
                    AvailableTrophies.Net = IntValue(value);
                    break;
                case GameField.Source:
                    SourceType = TypeHelper.Value<Source>(value);
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
                case GameField.Owner:
                    Owner = value is Account account ? account.Id : GuidValue(value);
                    break;
                case GameField.Difficult:
                    Difficult = TypeHelper.Value<Difficult>(value);
                    break;
                case GameField.CompleteTime:
                    CompleteTime = TypeHelper.Value<CompleteTime>(value);
                    break;
                case GameField.Genre:
                    Genre = StringValue(value);
                    break;
                case GameField.ScreenView:
                    ScreenView = TypeHelper.Value<ScreenView>(value);
                    break;
                case GameField.GameModes:
                    GameModes.CopyFrom((DAO?)value);
                    break;
                case GameField.Dlcs:
                    Dlcs.CopyFrom((DAO?)value);
                    break;
                case GameField.Tags:
                    Tags.CopyFrom((DAO?)value);
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
            }

            if (!RelaterGameUpdateProcess)
                switch (field)
                {
                    case GameField.Series:
                    case GameField.TrophysetAccess:
                    case GameField.EarnedPlatinum:
                    case GameField.EarnedGold:
                    case GameField.EarnedSilver:
                    case GameField.EarnedBronze:
                    case GameField.EarnedFromDLC:
                    case GameField.EarnedNet:
                    case GameField.AvailablePlatinum:
                    case GameField.AvailableGold:
                    case GameField.AvailableSilver:
                    case GameField.AvailableBronze:
                    case GameField.AvailableFromDLC:
                    case GameField.AvailableNet:
                    case GameField.Developer:
                    case GameField.Publisher:
                    case GameField.Year:
                    case GameField.Pegi:
                    case GameField.ReleasePlatforms:
                    case GameField.Difficult:
                    case GameField.CompleteTime:
                    case GameField.Genre:
                    case GameField.ScreenView:
                    case GameField.RelatedGames:
                        UpdateRelatedGames();
                        break;
                }

            Modified = true;
        }

        private static bool AvailableTrophyset(Game game) =>
            game.Owner != Guid.Empty
            && game.Licensed 
            && TypeHelper.Helper<GameFormatHelper>().AvailableTrophies(game.Format)
            && TypeHelper.Helper<PlatformTypeHelper>().PlatformWithTrophies(game.PlatformType);

        private void UpdateRelatedGames()
        {
            for (int i = 0; i < RelatedGames.Count; i++)
            {
                RelatedGame relatedGame = RelatedGames[i];
                Game? game = DataManager.Item<GameField, Game>(GameField.Id, relatedGame.GameId);

                if (game == null)
                    continue;

                if (relatedGame.SameTrophyset && (!AvailableTrophyset(this) || !AvailableTrophyset(game)))
                    relatedGame.SameTrophyset = false;

                UpdateRelatedGame(game, relatedGame.SameTrophyset);
            }
        }

        private void UpdateRelatedGame(Game game, bool sameTrophyset)
        {
            game.RelatedGames.Add(id, sameTrophyset);
            game.RelaterGameUpdateProcess = true;
            ReleasePlatforms.Add(game.PlatformType);

            if (sameTrophyset)
            {
                foreach (GameField field in fieldHelper.SameTrophysetFields)
                    game[field] = this[field];
            }

            foreach (GameField field in fieldHelper.SyncronizedRelatedFields)
                game[field] = this[field];


            if (ReleasePlatforms.Contains(game.PlatformType))
            {
                foreach (GameField field in fieldHelper.SyncronizedReleaseFields)
                    game[field] = this[field];

                if ((IntValue(game[GameField.Year]) == -1) || (IntValue(game[GameField.Year]) == 0))
                    game[GameField.Year] = this[GameField.Year];

                if (IntValue(game[GameField.CriticScore]) == -1)
                    game[GameField.CriticScore] = this[GameField.CriticScore];
            }

            game.RelaterGameUpdateProcess = false;
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
                GameField.Series => series,
                GameField.PlatformFamily => PlatformFamily.Sony,
                GameField.Platform => platformType,
                GameField.Format => format,
                GameField.TrophysetAccess => TrophysetAccess,
                GameField.EarnedPlatinum => EarnedTrophies.Platinum,
                GameField.EarnedGold => EarnedTrophies.Gold,
                GameField.EarnedSilver => EarnedTrophies.Silver,
                GameField.EarnedBronze => EarnedTrophies.Bronze,
                GameField.EarnedFromDLC => EarnedTrophies.FromDLC,
                GameField.EarnedNet => EarnedTrophies.Net,
                GameField.AvailablePlatinum => AvailableTrophies.Platinum,
                GameField.AvailableGold => AvailableTrophies.Gold,
                GameField.AvailableSilver => AvailableTrophies.Silver,
                GameField.AvailableBronze => AvailableTrophies.Bronze,
                GameField.AvailableFromDLC => AvailableTrophies.FromDLC,
                GameField.AvailableNet => AvailableTrophies.Net,
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
                GameField.Difficult => Difficult,
                GameField.CompleteTime => CompleteTime,
                GameField.Genre => Genre,
                GameField.ScreenView => ScreenView,
                GameField.GameModes => GameModes,
                GameField.Dlcs => Dlcs,
                GameField.Tags => Tags,
                GameField.Links => Links,
                GameField.RelatedGames => RelatedGames,
                GameField.EmulatorType => EmulatorType,
                GameField.EmulatorROMs => ROMs,
                GameField.Status => new GameCalculations(this).GetGameStatus(),
                GameField.Progress => new GameCalculations(this).GetGameProgress(),
                GameField.EarnedPoints => new GameCalculations(this).GetEarnedPoints(),
                GameField.EarnedPointsOld => new GameCalculations(this).GetEarnedOldPoints(),
                _ => new GameCardDecorator(this)[field],
            };

        public override void Clear()
        {
            if (State is
                DAOState.Creating or
                DAOState.Loading)
                return;

            base.Clear();
            Id = Guid.Empty;
            Edition = string.Empty;
            Series = string.Empty;
            Verified = false;
            Licensed = true;
            Owner = Guid.Empty;
            ROMs = string.Empty;
            EmulatorType = string.Empty;
            Genre = string.Empty;
            Developer = string.Empty;
            Publisher = string.Empty;
            Code = string.Empty;
            Year = GameConsts.Empty_Year;
            CriticScore = GameConsts.Empty_CriticScore;

            PlatformType = TypeHelper.DefaultValue<PlatformType>();
            Format = TypeHelper.Helper<GameFormatHelper>().DefaultFormat(PlatformType);
            ScreenView = TypeHelper.DefaultValue<ScreenView>();
            Difficult = TypeHelper.DefaultValue<Difficult>();
            CompleteTime = TypeHelper.DefaultValue<CompleteTime>();
            trophysetAccess = TypeHelper.DefaultValue<TrophysetAccess>();
            SourceType = TypeHelper.DefaultValue<Source>();
            GameRegion = TypeHelper.DefaultValue<GameRegion>();
            GameLanguage = TypeHelper.DefaultValue<GameLanguage>();
            Pegi = TypeHelper.DefaultValue<Pegi>();

            ReleasePlatforms.Clear();
            GameModes.Clear();
            Installations.Clear();
            Dlcs.Clear();
            Tags.Clear();
            Links.Clear();
            RelatedGames.Clear();
            AvailableTrophies.Clear();
            EarnedTrophies.Clear();
        }

        private void GenerateId() => Id = Guid.NewGuid();

        public override void Init()
        {
            GenerateId();
            AddListMember(GameField.GameModes, GameModes);
            AddListMember(GameField.Dlcs, Dlcs);
            AddListMember(GameField.Tags, Tags);
            AddListMember(GameField.Installations, Installations);
            AddListMember(GameField.Links, Links);
            AddListMember(GameField.RelatedGames, RelatedGames);
            AddMember(AvailableTrophies);
            AddMember(EarnedTrophies);
            AddListMember(GameField.ReleasePlatforms, ReleasePlatforms);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            base.SaveData(element, clearModified);
            XmlHelper.AppendElement(element, XmlConsts.Id, Id);

            if (Owner != Guid.Empty)
                XmlHelper.AppendElement(element, XmlConsts.Owner, Owner);

            XmlHelper.AppendElement(element, XmlConsts.Edition, Edition, true);
            XmlHelper.AppendElement(element, XmlConsts.Series, Series, true);

            if (SourceType != TypeHelper.DefaultValue<Source>())
                XmlHelper.AppendElement(element, XmlConsts.Source, SourceType);

            XmlHelper.AppendElement(element, XmlConsts.Region, GameRegion);
            XmlHelper.AppendElement(element, XmlConsts.Language, GameLanguage);
            XmlHelper.AppendElement(element, XmlConsts.Code, Code, true);
            XmlHelper.AppendElement(element, XmlConsts.Platform, PlatformType);
            XmlHelper.AppendElement(element, XmlConsts.Format, Format);
            XmlHelper.AppendElement(element, XmlConsts.Verified, Verified);
            XmlHelper.AppendElement(element, XmlConsts.Licensed, Licensed);
            XmlHelper.AppendElement(element, XmlConsts.EmulatorType, EmulatorType, true);
            XmlHelper.AppendElement(element, XmlConsts.ROMs, ROMs, true);
            XmlHelper.AppendElement(element, XmlConsts.Screen, ScreenView);
            XmlHelper.AppendElement(element, XmlConsts.Genre, Genre, true);
            XmlHelper.AppendElement(element, XmlConsts.Developer, Developer, true);
            XmlHelper.AppendElement(element, XmlConsts.Publisher, Publisher, true);
            XmlHelper.AppendElement(element, XmlConsts.Year, Year);
            XmlHelper.AppendElement(element, XmlConsts.PEGI, Pegi);

            if (CriticScore >= 0)
                XmlHelper.AppendElement(element, XmlConsts.CriticScore, CriticScore);

            if (TypeHelper.Helper<SourceHelper>().TrophysetSupport(SourceType) &&
                TypeHelper.Helper<PlatformTypeHelper>().PlatformWithTrophies(PlatformType))
            {
                XmlHelper.AppendElement(element, XmlConsts.TrophysetAccess, TrophysetAccess);
                XmlHelper.AppendElement(element, XmlConsts.Difficult, Difficult);
                XmlHelper.AppendElement(element, XmlConsts.CompleteTime, CompleteTime);
            }
        }

        protected override void LoadData(XmlElement element)
        {
            base.LoadData(element);
            id = XmlHelper.ValueGuid(element, XmlConsts.Id);
            if (id == Guid.Empty)
                GenerateId();

            edition = XmlHelper.Value(element, XmlConsts.Edition);
            series = XmlHelper.Value(element, XmlConsts.Series);
            platformType = XmlHelper.Value<PlatformType>(element, XmlConsts.Platform);
            format = XmlHelper.Value<GameFormat>(element, XmlConsts.Format);
            verified = XmlHelper.ValueBool(element, XmlConsts.Verified);
            licensed = (XmlHelper.Value(element, XmlConsts.Licensed) == string.Empty) 
                || XmlHelper.ValueBool(element, XmlConsts.Licensed);
            owner = XmlHelper.ValueGuid(element, XmlConsts.Owner);
            sourceType = XmlHelper.Value<Source>(element, XmlConsts.Source);
            region = XmlHelper.Value<GameRegion>(element, XmlConsts.Region);
            language = XmlHelper.Value<GameLanguage>(element, XmlConsts.Language);
            code = XmlHelper.Value(element, XmlConsts.Code);
            emulatorType = XmlHelper.Value(element, XmlConsts.EmulatorType);
            roms = XmlHelper.Value(element, XmlConsts.ROMs);
            screenView = XmlHelper.Value<ScreenView>(element, XmlConsts.Screen);
            genre = XmlHelper.Value(element, XmlConsts.Genre);
            difficult = XmlHelper.Value<Difficult>(element, XmlConsts.Difficult);
            completeTime = XmlHelper.Value<CompleteTime>(element, XmlConsts.CompleteTime);
            trophysetAccess = XmlHelper.Value<TrophysetAccess>(element, XmlConsts.TrophysetAccess);
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
            obj is Game otherGame
            && (base.Equals(obj)
                || (Id.Equals(otherGame.Id)
                    && Edition.Equals(otherGame.Edition)
                    && Series.Equals(otherGame.Series)
                    && PlatformType.Equals(otherGame.PlatformType)
                    && Format.Equals(otherGame.Format)
                    && Licensed.Equals(otherGame.Licensed)
                    && Owner.Equals(otherGame.Owner)
                    && Verified.Equals(otherGame.Verified)
                    && SourceType.Equals(otherGame.SourceType)
                    && EmulatorType.Equals(otherGame.EmulatorType) 
                    && ROMs.Equals(otherGame.ROMs)
                    && ScreenView.Equals(otherGame.ScreenView)
                    && Genre.Equals(otherGame.Genre)
                    && GameModes.Equals(otherGame.GameModes)
                    && Installations.Equals(otherGame.Installations)
                    && Difficult.Equals(otherGame.Difficult)
                    && CompleteTime.Equals(otherGame.CompleteTime)
                    && Dlcs.Equals(otherGame.Dlcs)
                    && Tags.Equals(otherGame.Tags)
                    && Links.Equals(otherGame.Links)
                    && RelatedGames.Equals(otherGame.RelatedGames))
                    && EarnedTrophies.Equals(otherGame.EarnedTrophies)
                    && AvailableTrophies.Equals(otherGame.AvailableTrophies)
                    && TrophysetAccess.Equals(otherGame.TrophysetAccess)
                    && Developer.Equals(otherGame.Developer)
                    && Publisher.Equals(otherGame.Publisher)
                    && Year.Equals(otherGame.Year)
                    && Pegi.Equals(otherGame.Pegi)
                    && CriticScore.Equals(otherGame.CriticScore)
                    && ReleasePlatforms.Equals(otherGame.ReleasePlatforms)
                    && GameRegion.Equals(otherGame.GameRegion)
                    && GameLanguage.Equals(otherGame.GameLanguage)
                    && Code.Equals(otherGame.Code)
                );

        public override int GetHashCode() => 
            id.GetHashCode();

        public override string FullTitle() =>
            $"{Name}  ({TypeHelper.ShortName(PlatformType)}, {TypeHelper.Name(SourceType)}, {TypeHelper.Name(GameRegion)})";

        public override object ParseCaldedValue(GameField field, string value) => 
            field switch
            {
                GameField.AvailablePlatinum or
                GameField.EarnedPlatinum or 
                GameField.Year or 
                GameField.Progress => 
                    int.Parse(value),
                GameField.Region => TypeHelper.Parse<GameRegion>(value),
                GameField.Language => TypeHelper.Parse<GameLanguage>(value),
                GameField.Platform => TypeHelper.Parse<PlatformType>(value),
                GameField.Format => TypeHelper.Parse<GameFormat>(value),
                GameField.Source => TypeHelper.Parse<Source>(value),
                GameField.Pegi => TypeHelper.Parse<Pegi>(value),
                GameField.Difficult => TypeHelper.Parse<Difficult>(value),
                GameField.CompleteTime => TypeHelper.Parse<CompleteTime>(value),
                GameField.ScreenView => TypeHelper.Parse<ScreenView>(value),
                GameField.Status => TypeHelper.Parse<Status>(value),
                _ => value,
            };

        public override bool IsCalcedField(GameField field) =>
            fieldHelper.CalcedFields.Contains(field);

        public static bool CheckUniqueTrophyset(Game game, RootListDAO<GameField, Game> list)
        {
            foreach (RelatedGame relGame in game.RelatedGames)
                if (relGame.SameTrophyset
                    && list.Contains(g => g.Id == relGame.GameId))
                    return false;

            return true;
        }

        public static IMatcher<ConsoleField> AvailableConsoleFilter(ControlBuilder<GameField, Game> gameControlBuilder)
        {
            Filter<ConsoleField, PSConsole> filter = new();

            bool licensed = gameControlBuilder.Value<bool>(GameField.Licensed);

            foreach (ConsoleGeneration generation in
                TypeHelper.Helper<PlatformTypeHelper>().Generations(
                    gameControlBuilder.Value<PlatformType>(GameField.Platform),
                    gameControlBuilder.Value<Source>(GameField.Source),
                    licensed)
                )
                filter.AddFilter(ConsoleField.Generation, generation, FilterConcat.OR);

            if (!licensed)
                filter.AddFilter(ConsoleField.Firmware, FirmwareType.Custom, FilterConcat.AND);

            return filter;
        }
    }
}