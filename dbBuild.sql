
create database PARKINGLOTDB
go

use PARKINGLOTDB
go

create table Company(
  Company_id int identity primary key,
  Company_name varchar(30) unique not null
)

create table Pricing(
  Pricing_id int identity primary key,
  Pricing_startHour decimal(5,2) not null,
  Pricing_endHour decimal(5,2),
  Pricing_priceOfInterval decimal(10,2) not null,
  DayType_id int not null,
  SpecificDay_id int,
)

create table Bill(
	Bill_id int identity primary key,
	Bill_licensePlate varchar(10) not null,
	Bill_enterDate datetime2 not null,
	Bill_exitDate datetime2,
	Bill_price decimal(10,2) default 0,
	Company_id int not null,
	Pricing_id int,
)

create table DayType(
DayType_id int identity primary key,
DayType_title varchar(20) not null,
Company_id int not null,
unique (Company_id, DayType_title)
)

create table SpecificDay(
SpecificDay_id int identity primary key,
SpecificDay_month int not null,
SpecificDay_day int not null,
DayType_id int not null,
)



alter table Pricing
	add foreign key (DayType_id) references DayType(DayType_id);
alter table Pricing
	add foreign key (SpecificDay_id) references SpecificDay(SpecificDay_id);

alter table Bill
	add foreign key (Company_id) references Company(Company_id);
alter table Bill
	add foreign key (Pricing_id) references Pricing(Pricing_id);

alter table DayType
	add foreign key (Company_id) references Company(Company_id);
alter table SpecificDay
	add foreign key (DayType_id) references DayType(DayType_id);




