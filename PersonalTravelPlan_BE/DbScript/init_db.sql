USE PersonalTravelPlan
-- DROP
DROP TABLE IF EXISTS JourneyPlace, Journey, Place, Country, Currency;

-- CURRENCY
CREATE TABLE [dbo].[Currency] (
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(100),
	PRIMARY KEY (Id)
);

SET IDENTITY_INSERT Currency ON;
INSERT INTO Currency([Id], [Name])
VALUES (1, 'VND'), (2, 'USD'), (3, 'EUR');
SET IDENTITY_INSERT Currency OFF;

-- COUNTRY
CREATE TABLE [dbo].[Country] (
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(100),
	[Code] NVARCHAR(5),
	PRIMARY KEY (Id)
)

SET IDENTITY_INSERT Country ON;
INSERT INTO Country([Id], [Name], [Code])
VALUES (1, 'Vietnam', 'vi'), (2, 'England', 'en'), (3, 'United State', 'us');
SET IDENTITY_INSERT Country OFF;

-- PLACE
CREATE TABLE [dbo].[Place] (
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(100),
	[CountryId] INT,
	PRIMARY KEY (Id)
)
ALTER TABLE Place ADD FOREIGN KEY ([CountryId]) REFERENCES Country([Id]);

SET IDENTITY_INSERT Place ON;
INSERT INTO Place([Id], [Name], [CountryId])
VALUES
(1, 'Ho Chi Minh City', 1), (2, 'Ha Noi', 1),
(3, 'London', 2), (4, 'Edinburgh', 2),
(5, 'New York City', 3), (6, 'San Francisco', 3);
SET IDENTITY_INSERT Place OFF;

-- JOURNEY
CREATE TABLE [dbo].[Journey] (
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Name] NVARCHAR(100),
	[Description] NVARCHAR(1024),
	[FromDate] Date,
	[ToDate] Date,
	[DurationDay] INT,
	[DurationNight] INT,
	[Amount] INT,
	[Status] NVARCHAR(100),
	[ImageUrl] NVARCHAR(500),
	[CountryId] INT,
	[CurrencyId] INT,
	PRIMARY KEY (Id)
)
ALTER TABLE Place ADD FOREIGN KEY ([CountryId]) REFERENCES Country([Id]);

SET IDENTITY_INSERT Journey ON;
INSERT INTO Journey([Id], [Name], [Description], [FromDate], [ToDate], [DurationDay], [DurationNight], [Amount], [Status], [ImageUrl], [CountryId], [CurrencyId])
VALUES
(	1,
	'Personalized Ha Noi Adventure: A Journey of Discovery',
	'Take a leisurely stroll around Hoan Kiem Lake to get acquainted with the city charm. Explore the Temple of Literature, known for its historical significance and beautiful architecture.',
	'2023-1-12',
	'2023-1-13',
	2,
	1,
	10000000,
	'Finished',
	NULL,
	1,
	1
),
(	2,
	'Exploring New York City and San Francisco',
	'Due to the distance between the cities, it is recommended to allocate at least 3-4 days for each city to fully experience their unique attractions and atmosphere. Additional travel time should be considered.',
	'2023-8-10',
	'2023-8-20',
	10,
	9,
	2000,
	'Planning',
	NULL,
	3,
	2
);
SET IDENTITY_INSERT Journey OFF;

-- PLACE JOURNEY
CREATE TABLE JourneyPlace(
	[JourneyId] INT NOT NULL,
	[PlaceId] INT NOT NULL,
	PRIMARY KEY (JourneyId, PlaceId)
);
ALTER TABLE JourneyPlace ADD FOREIGN KEY ([JourneyId]) REFERENCES Journey([Id]);
ALTER TABLE JourneyPlace ADD FOREIGN KEY ([PlaceId]) REFERENCES Place([Id]);

INSERT INTO JourneyPlace([JourneyId], [PlaceId])
VALUES
(1, 2), -- Ha Noi
(2, 5), (2, 6); -- New York City, San Francisco


