using System.Xml;
using OxXMLEngine.ControlFactory;
using OxXMLEngine.Data;
using OxXMLEngine.Data.Filter;
using OxXMLEngine.Data.Types;
using OxXMLEngine.XML;
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
        private string imageBase64 = string.Empty;
        private Bitmap? image = null;
        private string name = string.Empty;
        private string edition = string.Empty;
        private string series = string.Empty;
        private PlatformType platformType = default;
        private GameFormat format = default;
        private Source sourceType = default;
        private GameRegion region = default;
        private GameLanguage language = default;
        private string code = string.Empty;
        private bool verified = false;
        private bool licensed = true;
        private bool favorite = false;
        private bool trophysetTODO = false;
        private string emulatorType = string.Empty;
        private string roms = string.Empty;
        private string genre = string.Empty;
        private ScreenView screenView = default;
        private Difficult difficult;
        private CompleteTime completeTime;
        private TrophysetAccessibility trophysetAccessibility;
        private string developer = string.Empty;
        private string publisher = string.Empty;
        private int year;
        private Pegi pegi;
        private int criticScore;
        public readonly ListDAO<Installation> Installations = new();
        public readonly ListDAO<DLC> Dlcs = new();
        public readonly ListDAO<Link> Links = new();
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
            set => id = GuidValue(ModifyField(id, value));
        }

        public string EmulatorType
        {
            get => emulatorType;
            set => emulatorType = StringValue(ModifyField(emulatorType, value));
        }

        public string ROMs
        {
            get => roms;
            set => roms = StringValue(ModifyField(roms, value));
        }

        public string Genre
        {
            get => genre;
            set => genre = StringValue(ModifyField(genre, value));
        }

        public ScreenView ScreenView
        {
            get => screenView;
            set => screenView = ModifyField(screenView, value);
        }

        public Difficult Difficult
        {
            get => difficult;
            set => difficult = ModifyField(difficult, value);
        }
        public CompleteTime CompleteTime
        {
            get => completeTime;
            set => completeTime = ModifyField(completeTime, value);
        }

        public TrophysetAccessibility TrophysetAccessibility
        {
            get => trophysetAccessibility;
            set => trophysetAccessibility = ModifyField(trophysetAccessibility, value);
        }

        public string Developer
        {
            get => developer;
            set => developer = StringValue(ModifyField(developer, value));
        }

        public string Publisher
        {
            get => publisher;
            set => publisher = StringValue(ModifyField(publisher, value));
        }

        public int Year
        {
            get => year;
            set => year = ModifyField(year, value);
        }

        public Pegi Pegi
        {
            get => pegi;
            set => pegi = ModifyField(pegi, value);
        }

        public int CriticScore
        {
            get => criticScore;
            set => criticScore = ModifyField(criticScore, value);
        }

        public string Name
        {
            get => name;
            set => name = StringValue(ModifyField(name, value));
        }

        public Bitmap? Image
        {
            get => image;
            set => image = ModifyField(image, value);
        }

        public string Edition
        {
            get => edition;
            set => edition = StringValue(ModifyField(edition, value));
        }

        public string Series
        {
            get => series;
            set => series = StringValue(ModifyField(series, value));
        }

        public bool Verified
        {
            get => verified;
            set => verified = BoolValue(ModifyField(verified, value));
        }

        public bool Licensed
        {
            get => licensed;
            set => licensed = BoolValue(ModifyField(licensed, value));
        }

        public bool Favorite
        {
            get => favorite;
            set => favorite = BoolValue(ModifyField(favorite, value));
        }

        public bool TrophysetTODO
        {
            get => trophysetTODO;
            set => trophysetTODO = BoolValue(ModifyField(trophysetTODO, value));
        }

        public override int CompareField(GameField field, IFieldMapping<GameField> y)
        {
            switch (field)
            {
                case GameField.Id:
                case GameField.Name:
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
                case GameField.Favorite:
                case GameField.TrophysetTODO:
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
                    GameField.TrophysetAccess => TrophysetAccessibility.CompareTo(yGame.TrophysetAccessibility),
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
                    GameField.Links => Links.CompareTo(yGame.Links),
                    GameField.Installations => Links.CompareTo(yGame.Links),
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
            set => platformType = ModifyField(platformType, value);
        }

        public GameFormat Format
        {
            get => format;
            set => format = ModifyField(format, value);
        }

        public Source SourceType
        {
            get => sourceType;
            set => sourceType = ModifyField(sourceType, value);
        }

        public GameRegion GameRegion
        {
            get => region;
            set => region = ModifyField(region, value);
        }

        public GameLanguage GameLanguage
        {
            get => language;
            set => language = ModifyField(language, value);
        }

        public string Code
        {
            get => code;
            set => code = StringValue(ModifyField(code, value));
        }

        public Game() : base() =>
            GenerateGuid();

        private static object? PrepareValueToSet(GameField field, object? value) =>
            field switch
            {
                GameField.AvailablePlatinum or 
                GameField.EarnedPlatinum or 
                GameField.Year => 
                    IntValue(value),
                GameField.Platform => 
                    TypeHelper.Value<PlatformType>(value),
                GameField.Format => 
                    TypeHelper.Value<GameFormat>(value),
                GameField.Source => 
                    TypeHelper.Value<Source>(value),
                GameField.Pegi => 
                    TypeHelper.Value<Pegi>(value),
                GameField.Difficult => 
                    TypeHelper.Value<Difficult>(value),
                GameField.CompleteTime => 
                    TypeHelper.Value<CompleteTime>(value),
                GameField.ScreenView => 
                    TypeHelper.Value<ScreenView>(value),
                _ => value,
            };

        protected override void SetFieldValue(GameField field, object? value)
        {
            value = PrepareValueToSet(field, value);

            if (!fieldHelper.AlwaysUpdateFields.Contains(field) &&
                !CheckValueModified(this[field], value))
                return;

            switch (field)
            {
                case GameField.Id:
                    Id = GuidValue(value);
                    break;
                case GameField.Name:
                    Name = StringValue(value);
                    break;
                case GameField.Image:
                    Image = (Bitmap?)value;
                    imageBase64 = string.Empty;
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
                    TrophysetAccessibility = TypeHelper.Value<TrophysetAccessibility>(value);
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
                case GameField.Favorite:
                    Favorite = BoolValue(value);
                    break;
                case GameField.TrophysetTODO:
                    TrophysetTODO = BoolValue(value);
                    break;
                case GameField.Licensed:
                    Licensed = BoolValue(value);
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
            game.Licensed 
            && TypeHelper.Helper<GameFormatHelper>().AvailableTrophies(game.Format)
            && TypeHelper.Helper<PlatformTypeHelper>().IsPSNPlatform(game.PlatformType);

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
                GameField.Name => name,
                GameField.Image => image,
                GameField.Edition => edition,
                GameField.Series => series,
                GameField.PlatformFamily => PlatformFamily.Sony,
                GameField.Platform => platformType,
                GameField.Format => format,
                GameField.TrophysetAccess => TrophysetAccessibility,
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
                GameField.Licensed => Licensed,
                GameField.Favorite => Favorite,
                GameField.TrophysetTODO => TrophysetTODO,
                GameField.Installations => Installations,
                GameField.Difficult => Difficult,
                GameField.CompleteTime => CompleteTime,
                GameField.Genre => Genre,
                GameField.ScreenView => ScreenView,
                GameField.GameModes => GameModes,
                GameField.Dlcs => Dlcs,
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
            Id = Guid.Empty;
            Name = string.Empty;
            Image = null;
            imageBase64 = string.Empty;
            Edition = string.Empty;
            Series = string.Empty;
            PlatformType = TypeHelper.DefaultValue<PlatformType>();
            Format = TypeHelper.Helper<GameFormatHelper>().DefaultFormat(PlatformType);
            Verified = false;
            Licensed = true;
            Favorite = false;
            TrophysetTODO = false;
            ROMs = string.Empty;
            EmulatorType = string.Empty;
            Genre = string.Empty;
            ScreenView = TypeHelper.DefaultValue<ScreenView>();
            Difficult = TypeHelper.DefaultValue<Difficult>();
            CompleteTime = TypeHelper.DefaultValue<CompleteTime>();
            trophysetAccessibility = TypeHelper.EmptyValue<TrophysetAccessibility>();
            Developer = string.Empty;
            Publisher = string.Empty;
            Year = GameConsts.Empty_Year;
            Pegi = TypeHelper.EmptyValue<Pegi>();
            CriticScore = GameConsts.Empty_CriticScore;
            ReleasePlatforms.Clear();

            SourceType = Source.PSN;
            GameRegion = default;
            GameLanguage = default;
            Code = string.Empty;
            GameModes.Clear();
            Installations.Clear();
            Dlcs.Clear();
            Links.Clear();
            RelatedGames.Clear();
            AvailableTrophies.Clear();
            EarnedTrophies.Clear();
        }

        private void GenerateGuid() => 
            Id = Guid.NewGuid();

        public override void Init()
        {
            GenerateGuid();
            AddMember(GameModes);
            AddMember(Dlcs);
            AddMember(Installations);
            AddMember(Links);
            AddMember(RelatedGames);
            AddMember(AvailableTrophies);
            AddMember(EarnedTrophies);
            AddMember(ReleasePlatforms);
        }

        protected override void SaveData(XmlElement element, bool clearModified = true)
        {
            XmlHelper.AppendElement(element, XmlConsts.Id, Id);
            XmlHelper.AppendElement(element, XmlConsts.Name, Name);

            if (imageBase64 == string.Empty)
            {
                if (Image != null)
                    XmlHelper.AppendElement(element, XmlConsts.Image, Image);
            }
            else XmlHelper.AppendElement(element, XmlConsts.Image, imageBase64);

            XmlHelper.AppendElement(element, XmlConsts.Edition, Edition, true);
            XmlHelper.AppendElement(element, XmlConsts.Series, Series, true);
            XmlHelper.AppendElement(element, XmlConsts.Source, SourceType);
            XmlHelper.AppendElement(element, XmlConsts.Region, GameRegion);
            XmlHelper.AppendElement(element, XmlConsts.Language, GameLanguage);
            XmlHelper.AppendElement(element, XmlConsts.Code, Code, true);
            XmlHelper.AppendElement(element, XmlConsts.Platform, PlatformType);
            XmlHelper.AppendElement(element, XmlConsts.Format, Format);
            XmlHelper.AppendElement(element, XmlConsts.Verified, Verified);
            XmlHelper.AppendElement(element, XmlConsts.Licensed, Licensed);
            XmlHelper.AppendElement(element, XmlConsts.Favorite, Favorite);
            XmlHelper.AppendElement(element, XmlConsts.TrophysetTODO, TrophysetTODO);
            XmlHelper.AppendElement(element, XmlConsts.EmulatorType, EmulatorType, true);
            XmlHelper.AppendElement(element, XmlConsts.ROMs, ROMs, true);
            XmlHelper.AppendElement(element, XmlConsts.Screen, ScreenView);
            XmlHelper.AppendElement(element, XmlConsts.Difficult, Difficult);
            XmlHelper.AppendElement(element, XmlConsts.CompleteTime, CompleteTime);
            XmlHelper.AppendElement(element, XmlConsts.Genre, Genre, true);
            XmlHelper.AppendElement(element, XmlConsts.TrophysetAccess, TrophysetAccessibility);
            XmlHelper.AppendElement(element, XmlConsts.Developer, Developer, true);
            XmlHelper.AppendElement(element, XmlConsts.Publisher, Publisher, true);
            XmlHelper.AppendElement(element, XmlConsts.Year, Year);
            XmlHelper.AppendElement(element, XmlConsts.PEGI, Pegi);
            XmlHelper.AppendElement(element, XmlConsts.CriticScore, CriticScore);
        }

        protected override void LoadData(XmlElement element)
        {
            id = XmlHelper.ValueGuid(element, XmlConsts.Id);
            if (id == Guid.Empty)
                GenerateGuid();

            Name = XmlHelper.Value(element, XmlConsts.Name);
            imageBase64 = XmlHelper.Value(element, XmlConsts.Image);
            Image = XmlHelper.ValueBitmap(element, XmlConsts.Image);
            Edition = XmlHelper.Value(element, XmlConsts.Edition);
            Series = XmlHelper.Value(element, XmlConsts.Series);
            PlatformType = XmlHelper.Value<PlatformType>(element, XmlConsts.Platform);
            Format = XmlHelper.Value<GameFormat>(element, XmlConsts.Format);
            Verified = XmlHelper.ValueBool(element, XmlConsts.Verified);

            Licensed = (XmlHelper.Value(element, XmlConsts.Licensed) == string.Empty) 
                || XmlHelper.ValueBool(element, XmlConsts.Licensed);

            Favorite = XmlHelper.ValueBool(element, XmlConsts.Favorite);
            TrophysetTODO = XmlHelper.ValueBool(element, XmlConsts.TrophysetTODO);
            SourceType = XmlHelper.Value<Source>(element, XmlConsts.Source);
            GameRegion = XmlHelper.Value<GameRegion>(element, XmlConsts.Region);
            GameLanguage = XmlHelper.Value<GameLanguage>(element, XmlConsts.Language);
            Code = XmlHelper.Value(element, XmlConsts.Code);
            EmulatorType = XmlHelper.Value(element, XmlConsts.EmulatorType);
            ROMs = XmlHelper.Value(element, XmlConsts.ROMs);
            ScreenView = XmlHelper.Value<ScreenView>(element, XmlConsts.Screen);
            Genre = XmlHelper.Value(element, XmlConsts.Genre);
            Difficult = XmlHelper.Value<Difficult>(element, XmlConsts.Difficult);
            CompleteTime = XmlHelper.Value<CompleteTime>(element, XmlConsts.CompleteTime);
            trophysetAccessibility = XmlHelper.Value<TrophysetAccessibility>(element, XmlConsts.TrophysetAccess);
            Developer = XmlHelper.Value(element, XmlConsts.Developer);
            Publisher = XmlHelper.Value(element, XmlConsts.Publisher);
            Year = XmlHelper.ValueInt(element, XmlConsts.Year);
            Pegi = XmlHelper.Value<Pegi>(element, XmlConsts.PEGI);
            CriticScore = XmlHelper.ValueInt(element, XmlConsts.CriticScore);
        }

        public override string ToString() => 
            Name;

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
                    && ((Image == null && otherGame.Image == null) 
                        || Image != null && Image.Equals(otherGame.Image))
                    && Name.Equals(otherGame.Name)
                    && Edition.Equals(otherGame.Edition)
                    && Series.Equals(otherGame.Series)
                    && PlatformType.Equals(otherGame.PlatformType)
                    && Format.Equals(otherGame.Format)
                    && Licensed.Equals(otherGame.Licensed)
                    && Favorite.Equals(otherGame.Favorite)
                    && TrophysetTODO.Equals(otherGame.TrophysetTODO)
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
                    && Links.Equals(otherGame.Links)
                    && RelatedGames.Equals(otherGame.RelatedGames))
                    && EarnedTrophies.Equals(otherGame.EarnedTrophies)
                    && AvailableTrophies.Equals(otherGame.AvailableTrophies)
                    && TrophysetAccessibility.Equals(otherGame.TrophysetAccessibility)
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

        public static IMatcher<PSConsole> AvailableConsoleFilter(ControlBuilder<GameField, Game> gameControlBuilder)
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