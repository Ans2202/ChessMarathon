set search_path to chess;
create Schema chess;

create table chess.Players (
    player_id serial primary key,
    first_name varchar(50) not null,
    last_name varchar(50) not null,
    country varchar(50) not null,
    current_world_ranking int unique not null,
    total_matches_played int not null default 0
);
select * from Players;

create table chess.Matches (
    match_id serial primary key,
    player1_id int not null,
    player2_id int not null,
    match_date date not null,
    match_level varchar(20) check(match_level in ('International','National')) not null,
    winner_id int default null,
    foreign key (player1_id) references chess.Players (player_id) on delete cascade on update cascade,
    foreign key (player2_id) references chess.Players (player_id) on delete cascade on update cascade,
    foreign key (winner_id) references chess.Players (player_id) on delete cascade on update cascade
);
select * from Matches;

create table chess.Sponsors(
    sponsor_id serial primary key,
    sponsor_name varchar(100) unique not null,
    industry varchar(50) not null,
    contact_email varchar(100) not null,
    contact_phone varchar(20) not null
);
select* from sponsors;

create table chess.Player_Sponsors(
    player_id int not null,
    sponsor_id int not null,
    sponsorship_amount numeric(10,2) not null,
    contract_start_date date not null,
    contract_end_date date not null,
    foreign key (player_id) references chess.Players (player_id) on delete cascade on update cascade,
    foreign key (sponsor_id) references chess.Sponsors (sponsor_id) on delete cascade on update cascade
);
select * from Player_Sponsors;

INSERT INTO Players (first_name, last_name, country, current_world_ranking, total_matches_played)
VALUES 
('Magnus', 'Carlsen', 'Norway', 1, 100),
('Fabiano', 'Caruana', 'USA', 2, 95),
('Ding', 'Liren', 'China', 3, 90),
('Ian', 'Nepomniachtchi', 'Russia', 4, 85),
('Wesley', 'So', 'USA', 5, 80),
('Anish', 'Giri', 'Netherlands', 6, 78),
('Hikaru', 'Nakamura', 'USA', 7, 75),
('Viswanathan', 'Anand', 'India', 8, 120),
('Teimour', 'Radjabov', 'Azerbaijan', 9, 70),
('Levon', 'Aronian', 'Armenia', 10, 72);

INSERT INTO Sponsors (sponsor_name, industry, contact_email, contact_phone)
VALUES 
('TechChess', 'Technology', 'contact@techchess.com', '123-456-7890'),
('MoveMaster', 'Gaming', 'info@movemaster.com', '234-567-8901'),
('ChessKing', 'Sports', 'support@chessking.com', '345-678-9012'),
('SmartMoves', 'AI', 'hello@smartmoves.ai', '456-789-0123'),
('GrandmasterFinance', 'Finance', 'contact@grandmasterfinance.com', '567-890-1234');

INSERT INTO Matches (player1_id, player2_id, match_date, match_level, winner_id)
VALUES 
(1, 2, '2024-08-01', 'International', 1),
(3, 4, '2024-08-02', 'International', 3),
(5, 6, '2024-08-03', 'National', 5),
(7, 8, '2024-08-04', 'International', 8),
(9, 10, '2024-08-05', 'National', 10),
(1, 3, '2024-08-06', 'International', 1),
(2, 4, '2024-08-07', 'National', 2),
(5, 7, '2024-08-08', 'International', 7),
(6, 8, '2024-08-09', 'National', 8),
(9, 1, '2024-08-10', 'International', 1);


INSERT INTO Player_Sponsors (player_id, sponsor_id, sponsorship_amount, contract_start_date, contract_end_date)
VALUES 
(1, 1, 500000.00, '2023-01-01', '2025-12-31'),
(2, 2, 300000.00, '2023-06-01', '2024-06-01'),
(3, 3, 400000.00, '2024-01-01', '2025-01-01'),
(4, 4, 350000.00, '2023-03-01', '2024-03-01'),
(5, 5, 450000.00, '2023-05-01', '2024-05-01'),
(6, 1, 250000.00, '2024-02-01', '2025-02-01'),
(7, 2, 200000.00, '2023-08-01', '2024-08-01'),
(8, 3, 600000.00, '2023-07-01', '2025-07-01'),
(9, 4, 150000.00, '2023-09-01', '2024-09-01'),
(10, 5, 300000.00, '2024-04-01', '2025-04-01');

--1
select * from matches 

--2
select * from Players where country ilike 'USA' order by current_world_ranking;

--3

SELECT concat(p.first_name ,' ', p.last_name) AS full_name,
COUNT(m.match_id) AS total_matches_played,
COUNT(CASE WHEN m.winner_id = p.player_id THEN 1 END) AS total_matches_won,
ROUND(COUNT(m.winner_id) * 100.0 / p.total_matches_played, 2) AS win_percentage
FROM chess.Players p
LEFT JOIN chess.Matches m
ON  p.player_id = m.player1_id OR p.player_id = m.player2_id
GROUP BY p.player_id, p.first_name, p.last_name
ORDER BY p.current_world_ranking;


--4
select concat(p.first_name,' ',p.last_name) as Full_Name,p.total_matches_played, count(m.winner_id) as TotalWinCount,
    ROUND(COUNT(m.winner_id) * 100.0 / p.total_matches_played, 2) AS win_percentage
    from players p
join matches m on p.player_id=m.winner_id
group by Full_Name,p.total_matches_played
having count(m.winner_id) > (select avg(win_count) from(
select count(winner_id) as win_count from Matches group by winner_id) as win_counts )
order by Full_Name



