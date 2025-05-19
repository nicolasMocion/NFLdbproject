


-- Teams
INSERT INTO Teams (Id, Name, StadiumId, CityId) VALUES
(1, 'Arizona Cardinals', 1, 1),
(2, 'Atlanta Falcons', 2, 2),
(3, 'Baltimore Ravens', 3, 3),
(4, 'Buffalo Bills', 4, 4),
(5, 'Carolina Panthers', 5, 5),
(6, 'Chicago Bears', 6, 6),
(7, 'Cincinnati Bengals', 7, 7),
(8, 'Cleveland Browns', 8, 8),
(9, 'Dallas Cowboys', 9, 9),
(10, 'Denver Broncos', 10, 10),
(11, 'Detroit Lions', 11, 11),
(12, 'Green Bay Packers', 12, 12),
(13, 'Houston Texans', 13, 13),
(14, 'Indianapolis Colts', 14, 14),
(15, 'Jacksonville Jaguars', 15, 15),
(16, 'Kansas City Chiefs', 16, 16),
(17, 'Las Vegas Raiders', 17, 17),
(18, 'Los Angeles Chargers', 18, 18),
(19, 'Miami Dolphins', 19, 19),
(20, 'Minnesota Vikings', 20, 20),
(21, 'New England Patriots', 21, 21),
(22, 'New Orleans Saints', 22, 22),
(23, 'New York Giants', 23, 23),
(24, 'New York Jets', 23, 23),
(25, 'Philadelphia Eagles', 24, 24),
(26, 'Pittsburgh Steelers', 25, 25),
(27, 'San Francisco 49ers', 26, 26),
(28, 'Seattle Seahawks', 27, 27),
(29, 'Tampa Bay Buccaneers', 28, 28),
(30, 'Tennessee Titans', 29, 30),
(31, 'Washington Commanders', 30, 29),
(32, 'Los Angeles Rams', 18, 18); -- Same as Chargers



- Cities
            INSERT INTO Cities (Id, Name, Population) VALUES
            (1, 'Glendale', 252381),       -- Arizona Cardinals
            (2, 'Atlanta', 498044),        -- Atlanta Falcons
            (3, 'Baltimore', 593490),      -- Baltimore Ravens
            (4, 'Orchard Park', 29000),    -- Buffalo Bills
            (5, 'Charlotte', 872498),      -- Carolina Panthers
            (6, 'Chicago', 2716000),       -- Chicago Bears
            (7, 'Cincinnati', 309317),     -- Cincinnati Bengals
            (8, 'Cleveland', 372624),      -- Cleveland Browns
            (9, 'Arlington', 398854),      -- Dallas Cowboys
            (10, 'Denver', 715522),        -- Denver Broncos
            (11, 'Detroit', 670031),       -- Detroit Lions
            (12, 'Green Bay', 104578),      -- Green Bay Packers
            (13, 'Houston', 2328000),       -- Houston Texans
            (14, 'Indianapolis', 876384),   -- Indianapolis Colts
            (15, 'Jacksonville', 892062),   -- Jacksonville Jaguars
            (16, 'Kansas City', 508090),    -- Kansas City Chiefs
            (17, 'Paradise', 28178),       -- Las Vegas Raiders
            (18, 'Inglewood', 109673),      -- Los Angeles Rams, Los Angeles Chargers
            (19, 'Miami Gardens', 110296),  -- Miami Dolphins
            (20, 'Minneapolis', 425403),    -- Minnesota Vikings
            (21, 'Foxborough', 5783),       -- New England Patriots
            (22, 'New Orleans', 383997),    -- New Orleans Saints
            (23, 'East Rutherford', 10038), -- New York Giants, New York Jets
            (24, 'Philadelphia', 1584200),  -- Philadelphia Eagles
            (25, 'Pittsburgh', 302407),    -- Pittsburgh Steelers
            (26, 'Santa Clara', 132679),    -- San Francisco 49ers
            (27, 'Seattle', 753675),        -- Seattle Seahawks
            (28, 'Tampa', 404636),          -- Tampa Bay Buccaneers
            (29, 'Landover', 46985),        -- Washington Commanders
            (30, 'Nashville', 715485);      -- Tennessee Titans (added the missing city)

            -- Stadiums (Non-Redundant)
            INSERT INTO Stadiums (Id, Name, Capacity, CityId) VALUES
            (1, 'State Farm Stadium', 63400, 1),        -- Glendale (Arizona)
            (2, 'Mercedes-Benz Stadium', 71000, 2),    -- Atlanta
            (3, 'M&T Bank Stadium', 71008, 3),          -- Baltimore
            (4, 'Highmark Stadium', 71608, 4),          -- Orchard Park (Buffalo)
            (5, 'Bank of America Stadium', 74900, 5),  -- Charlotte
            (6, 'Soldier Field', 61500, 6),             -- Chicago
            (7, 'Paycor Stadium', 65500, 7),            -- Cincinnati
            (8, 'FirstEnergy Stadium', 67834, 8),       -- Cleveland
            (9, 'AT&T Stadium', 80000, 9),               -- Arlington (Dallas)
            (10, 'Empower Field at Mile High', 76125, 10), -- Denver
            (11, 'Ford Field', 65000, 11),               -- Detroit
            (12, 'Lambeau Field', 81441, 12),            -- Green Bay
            (13, 'NRG Stadium', 72000, 13),              -- Houston
            (14, 'Lucas Oil Stadium', 67000, 14),        -- Indianapolis
            (15, 'TIAA Bank Field', 67400, 15),          -- Jacksonville
            (16, 'Arrowhead Stadium', 76416, 16),        -- Kansas City
            (17, 'Allegiant Stadium', 65000, 17),        -- Paradise (Las Vegas)
            (18, 'SoFi Stadium', 70000, 18),              -- Inglewood (Los Angeles - Rams & Chargers)
            (19, 'Hard Rock Stadium', 65000, 19),        -- Miami Gardens (Miami)
            (20, 'U.S. Bank Stadium', 66800, 20),        -- Minneapolis
            (21, 'Gillette Stadium', 65878, 21),         -- Foxborough (New England)
            (22, 'Caesars Superdome', 73208, 22),        -- New Orleans
            (23, 'MetLife Stadium', 82500, 23),          -- East Rutherford (New York - Jets & Giants)
            (24, 'Lincoln Financial Field', 69596, 24), -- Philadelphia
            (25, 'Heinz Field', 68400, 25),               -- Pittsburgh
            (26, 'Levi’s Stadium', 68500, 26),            -- Santa Clara (San Francisco)
            (27, 'Lumen Field', 68000, 27),                -- Seattle
            (28, 'Raymond James Stadium', 65890, 28),     -- Tampa
            (29, 'Nissan Stadium', 69000, 29),           -- Nashville (Tennessee Titans)
            (30, 'FedExField', 82000, 30);          -- Landover (Washington);
        

- Colleges

INSERT INTO Colleges (Id, Name, CityId) VALUES
(1, 'Glendale Community College', 1),
(2, 'Georgia State University', 2),
(3, 'Morgan State University', 3),
(4, 'SUNY Erie', 4),
(5, 'University of North Carolina at Charlotte', 5),
(6, 'University of Illinois at Chicago', 6),
(7, 'University of Cincinnati', 7),
(8, 'Cleveland State University', 8),
(9, 'University of Texas at Arlington', 9),
(10, 'University of Denver', 10),
(11, 'Wayne State University', 11),
(12, 'University of Wisconsin–Green Bay', 12),
(13, 'University of Houston', 13),
(14, 'IUPUI', 14),
(15, 'University of North Florida', 15),
(16, 'University of Missouri–Kansas City', 16),
(17, 'University of Nevada, Las Vegas', 17),
(18, 'University of Southern California', 18),
(19, 'Florida Memorial University', 19),
(20, 'University of Minnesota', 20),
(21, 'Bridgewater State University', 21),
(22, 'Tulane University', 22),
(23, 'Rutgers University – Newark', 23),
(24, 'Temple University', 24),
(25, 'University of Pittsburgh', 25),
(26, 'Santa Clara University', 26),
(27, 'University of Washington', 27),
(28, 'University of South Florida', 28),
(29, 'University of Maryland', 29),
(30, 'Vanderbilt University', 30);

INSERT INTO Players (Id, Name, Position, Number, CollegeId, TeamId, HomeTownCityId) VALUES
(1, 'John Smith', 'QB', 12, 1, 1, 1),
(2, 'Michael Johnson', 'WR', 88, 2, 2, 2),
(3, 'David Brown', 'RB', 22, 3, 3, 3),
(4, 'Chris Davis', 'LB', 55, 4, 4, 4),
(5, 'James Wilson', 'CB', 24, 5, 5, 5),
(6, 'Robert Miller', 'TE', 85, 6, 6, 6),
(7, 'William Moore', 'QB', 7, 7, 7, 7),
(8, 'Daniel Taylor', 'WR', 17, 8, 8, 8),
(9, 'Joseph Anderson', 'RB', 28, 9, 9, 9),
(10, 'Mark Thomas', 'LB', 52, 10, 10, 10),

(11, 'Steven Jackson', 'CB', 21, 11, 11, 11),
(12, 'Paul White', 'TE', 81, 12, 12, 12),
(13, 'Kevin Harris', 'QB', 3, 13, 13, 13),
(14, 'Brian Martin', 'WR', 84, 14, 14, 14),
(15, 'George Lewis', 'RB', 33, 15, 15, 15),
(16, 'Edward Walker', 'LB', 50, 16, 16, 16),
(17, 'Jason Hall', 'CB', 25, 17, 17, 17),
(18, 'Timothy Allen', 'TE', 89, 18, 18, 18),
(19, 'Ryan Young', 'QB', 9, 19, 19, 19),
(20, 'Eric King', 'WR', 13, 20, 20, 20),

(21, 'Larry Scott', 'RB', 30, 21, 21, 21),
(22, 'Frank Green', 'LB', 56, 22, 22, 22),
(23, 'Justin Baker', 'CB', 27, 23, 23, 23),
(24, 'Aaron Gonzalez', 'TE', 88, 24, 24, 24),
(25, 'Sean Nelson', 'QB', 11, 25, 25, 25),
(26, 'Eric Carter', 'WR', 80, 26, 26, 26),
(27, 'Benjamin Mitchell', 'RB', 26, 27, 27, 27),
(28, 'Adam Perez', 'LB', 54, 28, 28, 28),
(29, 'Nathan Roberts', 'CB', 23, 29, 29, 29),
(30, 'Gregory Turner', 'TE', 87, 30, 30, 30),

(31, 'Patrick Collins', 'QB', 10, 1, 1, 1),
(32, 'Derek Edwards', 'WR', 82, 2, 2, 2),
(33, 'Kyle Morris', 'RB', 20, 3, 3, 3),
(34, 'Shawn Rivera', 'LB', 58, 4, 4, 4),
(35, 'Brandon Cook', 'CB', 29, 5, 5, 5),
(36, 'Jordan Morgan', 'TE', 84, 6, 6, 6),
(37, 'Austin Bell', 'QB', 14, 7, 7, 7),
(38, 'Carlos Diaz', 'WR', 81, 8, 8, 8),
(39, 'Victor Ramirez', 'RB', 24, 9, 9, 9),
(40, 'Dominic Bailey', 'LB', 57, 10, 10, 10),

(41, 'Ethan Murphy', 'CB', 22, 11, 11, 11),
(42, 'Sean Powell', 'TE', 83, 12, 12, 12),
(43, 'Joel Foster', 'QB', 4, 13, 13, 13),
(44, 'Dylan Simmons', 'WR', 86, 14, 14, 14),
(45, 'Corey Bryant', 'RB', 31, 15, 15, 15),
(46, 'Maxwell Jenkins', 'LB', 59, 16, 16, 16),
(47, 'Collin Dixon', 'CB', 20, 17, 17, 17),
(48, 'Isaac Hayes', 'TE', 90, 18, 18, 18),
(49, 'Joel Warren', 'QB', 6, 19, 19, 19),
(50, 'Brady Fields', 'WR', 85, 20, 20, 20);


INSERT INTO Coaches (Id, Name, Type, Position, TeamId) VALUES
(1, 'Mike Anderson', 'Offensive', 'Head Coach', 1),
(2, 'John Carter', 'Defensive', 'Defensive Coordinator', 1),
(3, 'Lisa Roberts', 'Offensive', 'Offensive Coordinator', 2),
(4, 'Tommy Lewis', 'Defensive', 'Linebackers Coach', 2),
(5, 'Samantha Green', 'Offensive', 'Quarterbacks Coach', 3),
(6, 'Eric Brown', 'Defensive', 'Defensive Backs Coach', 3),
(7, 'Rachel Turner', 'Offensive', 'Running Backs Coach', 4),
(8, 'David Smith', 'Defensive', 'Defensive Line Coach', 4),
(9, 'Brian Johnson', 'Offensive', 'Wide Receivers Coach', 5),
(10, 'Nancy Clark', 'Defensive', 'Linebackers Coach', 5),

(11, 'Jeffrey White', 'Offensive', 'Head Coach', 6),
(12, 'Patricia Hall', 'Defensive', 'Defensive Coordinator', 6),
(13, 'Greg Miller', 'Offensive', 'Offensive Coordinator', 7),
(14, 'Amanda Evans', 'Defensive', 'Defensive Backs Coach', 7),
(15, 'Kevin Walker', 'Offensive', 'Quarterbacks Coach', 8),
(16, 'Laura Adams', 'Defensive', 'Defensive Line Coach', 8),
(17, 'Steven Mitchell', 'Offensive', 'Running Backs Coach', 9),
(18, 'Monica Davis', 'Defensive', 'Linebackers Coach', 9),
(19, 'Daniel King', 'Offensive', 'Wide Receivers Coach', 10),
(20, 'Cynthia Lewis', 'Defensive', 'Defensive Coordinator', 10);


INSERT INTO Seasons (Id, Year) VALUES
(1, 1990),
(2, 1991),
(3, 1992),
(4, 1993),
(5, 1994),
(6, 1995),
(7, 1996),
(8, 1997),
(9, 1998),
(10, 1999),
(11, 2000),
(12, 2001),
(13, 2002),
(14, 2003),
(15, 2004),
(16, 2005),
(17, 2006),
(18, 2007),
(19, 2008),
(20, 2009),
(21, 2010),
(22, 2011),
(23, 2012),
(24, 2013),
(25, 2014),
(26, 2015),
(27, 2016),
(28, 2017),
(29, 2018),
(30, 2019),
(31, 2020),
(32, 2021),
(33, 2022),
(34, 2023);

INSERT INTO Titles (Id, Name, Description) VALUES
(1, 'Super Bowl Champion', 'Winner of the NFL Super Bowl, the league championship game'),
(2, 'AFC Champion', 'Winner of the American Football Conference (AFC) in the playoffs'),
(3, 'NFC Champion', 'Winner of the National Football Conference (NFC) in the playoffs'),
(4, 'AFC East Division Champion', 'Winner of the AFC East Division'),
(5, 'AFC North Division Champion', 'Winner of the AFC North Division'),
(6, 'AFC South Division Champion', 'Winner of the AFC South Division'),
(7, 'AFC West Division Champion', 'Winner of the AFC West Division'),
(8, 'NFC East Division Champion', 'Winner of the NFC East Division'),
(9, 'NFC North Division Champion', 'Winner of the NFC North Division'),
(10, 'NFC South Division Champion', 'Winner of the NFC South Division'),
(11, 'NFC West Division Champion', 'Winner of the NFC West Division');


INSERT INTO NFLGames (Id, GameDate, HomeScore, AwayScore, SeasonId, AwayTeamId, HomeTeamId) VALUES
(1, '2023-09-10 13:00:00', 24, 17, 1, 2, 5),
(2, '2023-09-17 13:00:00', 31, 21, 1, 9, 1),
(3, '2023-09-24 13:00:00', 27, 30, 1, 12, 3),
(4, '2023-10-01 13:00:00', 14, 28, 1, 7, 4),
(5, '2023-10-08 13:00:00', 21, 19, 1, 10, 6),
(6, '2023-10-15 13:00:00', 34, 24, 1, 8, 2),
(7, '2023-10-22 13:00:00', 17, 23, 1, 14, 11),
(8, '2023-10-29 13:00:00', 20, 27, 1, 16, 13),
(9, '2023-11-05 13:00:00', 19, 22, 1, 15, 7),
(10, '2023-11-12 13:00:00', 23, 14, 1, 5, 10),

(11, '2023-11-19 13:00:00', 27, 25, 1, 18, 12),
(12, '2023-11-26 13:00:00', 30, 16, 1, 4, 8),
(13, '2023-12-03 13:00:00', 21, 18, 1, 22, 17),
(14, '2023-12-10 13:00:00', 24, 29, 1, 6, 20),
(15, '2023-12-17 13:00:00', 28, 26, 1, 19, 14),
(16, '2023-12-24 13:00:00', 17, 20, 1, 1, 23),
(17, '2023-12-31 13:00:00', 22, 21, 1, 21, 15),
(18, '2024-01-07 13:00:00', 14, 24, 1, 3, 9),
(19, '2024-01-14 13:00:00', 26, 19, 1, 11, 18),
(20, '2024-01-21 13:00:00', 33, 31, 1, 13, 22),

(21, '2024-01-28 13:00:00', 20, 23, 1, 24, 16),
(22, '2024-02-04 13:00:00', 29, 25, 1, 17, 1),
(23, '2024-02-11 13:00:00', 18, 30, 1, 20, 7),
(24, '2024-02-18 13:00:00', 21, 22, 1, 25, 12),
(25, '2024-02-25 13:00:00', 27, 28, 1, 8, 6),
(26, '2024-03-03 13:00:00', 15, 17, 1, 19, 23),
(27, '2024-03-10 13:00:00', 24, 21, 1, 30, 26),
(28, '2024-03-17 13:00:00', 29, 22, 1, 31, 27),
(29, '2024-03-24 13:00:00', 18, 24, 1, 32, 28),
(30, '2024-03-31 13:00:00', 23, 19, 1, 1, 10),

(31, '2024-04-07 13:00:00', 31, 20, 1, 9, 5),
(32, '2024-04-14 13:00:00', 22, 24, 1, 6, 11),
(33, '2024-04-21 13:00:00', 27, 28, 1, 2, 14),
(34, '2024-04-28 13:00:00', 19, 17, 1, 4, 7),
(35, '2024-05-05 13:00:00', 30, 26, 1, 15, 13),
(36, '2024-05-12 13:00:00', 28, 30, 1, 12, 20),
(37, '2024-05-19 13:00:00', 25, 27, 1, 21, 18),
(38, '2024-05-26 13:00:00', 22, 19, 1, 8, 3),
(39, '2024-06-02 13:00:00', 24, 23, 1, 10, 22),
(40, '2024-06-09 13:00:00', 31, 29, 1, 7, 26),

(41, '2024-06-16 13:00:00', 20, 25, 1, 5, 29),
(42, '2024-06-23 13:00:00', 18, 21, 1, 14, 17),
(43, '2024-06-30 13:00:00', 26, 28, 1, 3, 19),
(44, '2024-07-07 13:00:00', 29, 22, 1, 24, 12),
(45, '2024-07-14 13:00:00', 23, 20, 1, 16, 4),
(46, '2024-07-21 13:00:00', 25, 27, 1, 2, 30),
(47, '2024-07-28 13:00:00', 21, 18, 1, 31, 9),
(48, '2024-08-04 13:00:00', 19, 24, 1, 6, 1),
(49, '2024-08-11 13:00:00', 27, 30, 1, 18, 13),
(50, '2024-08-18 13:00:00', 30, 25, 1, 22, 7),

(51, '2024-08-25 13:00:00', 16, 20, 1, 11, 5),
(52, '2024-09-01 13:00:00', 24, 22, 1, 9, 17),
(53, '2024-09-08 13:00:00', 28, 30, 1, 3, 10),
(54, '2024-09-15 13:00:00', 21, 19, 1, 26, 15),
(55, '2024-09-22 13:00:00', 29, 24, 1, 20, 2),
(56, '2024-09-29 13:00:00', 25, 23, 1, 13, 23),
(57, '2024-10-06 13:00:00', 22, 18, 1, 7, 31),
(58, '2024-10-13 13:00:00', 19, 26, 1, 1, 28),
(59, '2024-10-20 13:00:00', 30, 21, 1, 12, 24),
(60, '2024-10-27 13:00:00', 27, 25, 1, 5, 20),

(61, '2024-11-03 13:00:00', 23, 18, 1, 18, 16),
(62, '2024-11-10 13:00:00', 26, 29, 1, 11, 22),
(63, '2024-11-17 13:00:00', 24, 27, 1, 9, 15),
(64, '2024-11-24 13:00:00', 21, 23, 1, 4, 19),
(65, '2024-12-01 13:00:00', 20, 22, 1, 14, 6),
(66, '2024-12-08 13:00:00', 25, 28, 1, 7, 3),
(67, '2024-12-15 13:00:00', 27, 29, 1, 2, 10),
(68, '2024-12-22 13:00:00', 23, 24, 1, 16, 21),
(69, '2024-12-29 13:00:00', 18, 20, 1, 13, 30),
(70, '2025-01-05 13:00:00', 26, 22, 1, 28, 8);


INSERT INTO PlayerStats (Id, PlayerId, Yards, Touchdowns, Interceptions) VALUES
(1, 1, 3500, 25, 12),
(2, 2, 1100, 12, 1),
(3, 3, 950, 9, 0),
(4, 4, 100, 2, 1),
(5, 5, 50, 1, 0),
(6, 6, 600, 6, 0),
(7, 7, 2900, 18, 10),
(8, 8, 980, 10, 1),
(9, 9, 890, 8, 0),
(10, 10, 150, 3, 2),

(11, 11, 30, 0, 1),
(12, 12, 520, 5, 0),
(13, 13, 3100, 22, 11),
(14, 14, 1050, 11, 2),
(15, 15, 880, 7, 0),
(16, 16, 90, 2, 1),
(17, 17, 40, 1, 1),
(18, 18, 610, 6, 0),
(19, 19, 2800, 20, 9),
(20, 20, 970, 9, 1),

(21, 21, 840, 6, 0),
(22, 22, 110, 3, 2),
(23, 23, 25, 1, 1),
(24, 24, 670, 7, 0),
(25, 25, 3300, 21, 10),
(26, 26, 1020, 10, 1),
(27, 27, 810, 5, 0),
(28, 28, 140, 4, 2),
(29, 29, 20, 0, 1),
(30, 30, 620, 6, 0),

(31, 31, 2750, 17, 8),
(32, 32, 990, 11, 1),
(33, 33, 860, 6, 0),
(34, 34, 130, 3, 2),
(35, 35, 35, 0, 1),
(36, 36, 590, 5, 0),
(37, 37, 2950, 19, 9),
(38, 38, 1080, 12, 1),
(39, 39, 870, 7, 0),
(40, 40, 120, 2, 1),

(41, 41, 45, 1, 1),
(42, 42, 630, 6, 0),
(43, 43, 3150, 23, 11),
(44, 44, 1010, 10, 2),
(45, 45, 890, 8, 0),
(46, 46, 125, 3, 2),
(47, 47, 28, 1, 1),
(48, 48, 600, 5, 0),
(49, 49, 2900, 20, 10),
(50, 50, 960, 9, 1);

INSERT INTO PlayerTeamHistory (PlayerId, TeamId, StartDate, EndDate) VALUES
(1, 1, '2018-08-01', '2022-01-01'),
(2, 2, '2020-07-15', '2024-01-01'),
(3, 3, '2019-09-01', '2024-01-01'),
(4, 4, '2017-08-01', '2020-12-31'),
(4, 10, '2021-01-15', '2024-01-01'),
(5, 5, '2016-09-01', '2024-01-01'),
(6, 6, '2020-07-01', '2024-01-01'),
(7, 7, '2021-09-01', '2024-01-01'),
(8, 8, '2020-08-01', '2022-12-31'),
(8, 3, '2023-01-01', '2024-01-01'),

(9, 9, '2022-07-01', '2024-01-01'),
(10, 10, '2019-09-01', '2023-01-01'),
(11, 11, '2018-08-01', '2022-01-01'),
(12, 12, '2021-07-01', '2024-01-01'),
(13, 13, '2020-08-15', '2024-01-01'),
(14, 14, '2017-09-01', '2021-12-31'),
(14, 6, '2022-01-01', '2024-01-01'),
(15, 15, '2023-06-01', '2024-01-01'),
(16, 16, '2019-08-01', '2022-12-31'),
(16, 1, '2023-01-01', '2024-01-01'),

(17, 17, '2020-08-01', '2024-01-01'),
(18, 18, '2018-07-15', '2024-01-01'),
(19, 19, '2019-08-01', '2023-01-01'),
(20, 20, '2022-09-01', '2024-01-01'),
(21, 21, '2021-08-01', '2024-01-01'),
(22, 22, '2016-09-01', '2024-01-01'),
(23, 23, '2018-09-01', '2024-01-01'),
(24, 24, '2020-07-01', '2023-07-01'),
(24, 9, '2023-07-02', '2024-01-01'),
(25, 25, '2019-08-01', '2024-01-01'),

(26, 26, '2020-08-01', '2024-01-01'),
(27, 27, '2023-09-01', '2024-01-01'),
(28, 28, '2022-09-01', '2024-01-01'),
(29, 29, '2018-07-01', '2024-01-01'),
(30, 30, '2019-09-01', '2022-12-31'),
(30, 15, '2023-01-01', '2024-01-01'),
(31, 1, '2020-08-01', '2024-01-01'),
(32, 2, '2018-08-01', '2024-01-01'),
(33, 3, '2019-09-01', '2024-01-01'),
(34, 4, '2021-07-01', '2024-01-01'),

(35, 5, '2020-08-01', '2024-01-01'),
(36, 6, '2022-07-01', '2024-01-01'),
(37, 7, '2021-08-01', '2024-01-01'),
(38, 8, '2020-08-01', '2023-07-01'),
(38, 20, '2023-07-02', '2024-01-01'),
(39, 9, '2019-09-01', '2024-01-01'),
(40, 10, '2016-09-01', '2022-12-31'),
(41, 11, '2018-08-01', '2024-01-01'),
(42, 12, '2022-07-01', '2024-01-01'),
(43, 13, '2020-08-01', '2024-01-01'),

(44, 14, '2020-09-01', '2024-01-01'),
(45, 15, '2023-06-01', '2024-01-01'),
(46, 16, '2018-08-01', '2022-12-31'),
(46, 22, '2023-01-01', '2024-01-01'),
(47, 17, '2021-09-01', '2024-01-01'),
(48, 18, '2020-07-01', '2024-01-01'),
(49, 19, '2019-08-01', '2023-07-01'),
(49, 5, '2023-07-02', '2024-01-01'),
(50, 20, '2022-08-01', '2024-01-01');

-- Adding multiteam history to 10 players
INSERT INTO PlayerTeamHistory (PlayerId, TeamId, StartDate, EndDate) VALUES
(2, 5, '2024-02-01', '2025-01-01'),
(7, 14, '2024-01-15', '2025-01-01'),
(10, 4, '2023-02-01', '2025-01-01'),
(12, 18, '2024-01-01', '2025-01-01'),
(17, 2, '2024-03-01', '2025-01-01'),
(21, 1, '2024-01-01', '2025-01-01'),
(25, 7, '2024-02-15', '2025-01-01'),
(29, 10, '2024-01-01', '2025-01-01'),
(31, 20, '2024-01-01', '2025-01-01'),
(43, 6, '2024-01-15', '2025-01-01');


INSERT INTO CoachTeamHistory (CoachId, TeamId, StartDate, EndDate) VALUES
(1, 1, '2023-01-01', '2025-01-01'),
(2, 2, '2023-01-01', '2025-01-01'),
(3, 3, '2023-01-01', '2025-01-01'),
(4, 4, '2023-01-01', '2025-01-01'),
(5, 5, '2023-01-01', '2025-01-01'),
(6, 6, '2023-01-01', '2025-01-01'),
(7, 7, '2023-01-01', '2025-01-01'),
(8, 8, '2023-01-01', '2025-01-01'),
(9, 9, '2023-01-01', '2025-01-01'),
(10, 10, '2023-01-01', '2025-01-01'),
(11, 11, '2023-01-01', '2025-01-01'),
(12, 12, '2023-01-01', '2025-01-01'),
(13, 13, '2023-01-01', '2025-01-01'),
(14, 14, '2023-01-01', '2025-01-01'),
(15, 15, '2023-01-01', '2025-01-01'),
(16, 16, '2023-01-01', '2025-01-01'),
(17, 17, '2023-01-01', '2025-01-01'),
(18, 18, '2023-01-01', '2025-01-01'),
(19, 19, '2023-01-01', '2025-01-01'),
(20, 20, '2023-01-01', '2025-01-01');

INSERT INTO CoachTeamHistory (CoachId, TeamId, StartDate, EndDate) VALUES
(1, 11, '2020-01-01', '2022-12-31'),
(4, 13, '2021-01-01', '2022-12-31'),
(7, 2,  '2022-01-01', '2023-01-01'),
(10, 18, '2021-06-01', '2022-12-01'),
(13, 5, '2020-08-01', '2022-12-31');

INSERT INTO TeamTitles (TeamId, TitleId, YearWon) VALUES
(1, 1, 2010),
(1, 3, 2015),
(2, 2, 2012),
(3, 1, 2011),
(4, 4, 2018),
(5, 5, 2014),
(6, 6, 2020),
(7, 7, 2013),
(8, 2, 2016),
(9, 3, 2019),
(10, 8, 2021),
(11, 9, 2009),
(12, 10, 2017),
(13, 1, 2012),
(14, 2, 2014),
(15, 5, 2016),
(16, 4, 2019),
(17, 7, 2020),
(18, 6, 2018),
(19, 9, 2015),
(20, 10, 2013),
(21, 3, 2011),
(22, 1, 2017),
(23, 5, 2019),
(24, 2, 2020),
(25, 6, 2014),
(26, 8, 2012),
(27, 4, 2016),
(28, 7, 2018),
(29, 9, 2021);