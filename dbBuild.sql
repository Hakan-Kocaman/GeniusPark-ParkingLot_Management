use master
go

create database PARKINGLOTDB
go

use PARKINGLOTDB
go

create table Users(
	Users_id int identity primary key,
	Users_name varchar(20) unique not null,
	Users_password varchar(20) not null,
	Roles_id int not null, --
	Company_id int not null
)

create table Roles(  --
	Roles_id int identity primary key,
	Roles_title varchar(20) unique not null
)

create table Parkinglot( --
	Parkinglot_id int identity primary key,
	Parkinglot_name varchar(30) not null,
	Company_id int not null,
	unique (Parkinglot_name,Company_id),
)

create table Subscription( --
	Subscription_id int identity primary key,
	Subscription_coveringPercentage decimal(5,2) not null,
	Subscription_duration int,
	Company_id int not null,
)

create table SubscriptedVehicle( --
	SubscriptedVehicle_id int identity primary key,
	SubscriptedVehicle_licensePlate varchar(10) not null,
	SubscriptedVehicle_startDate datetime2 not null,
	SubscriptedVehicle_endDate datetime2,
	Subscription_id int not null,
	unique (SubscriptedVehicle_licensePlate, Subscription_id)
)

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
	Users_id int not null,
	Pricing_id int,
	Subscription_id int, --
	Parkinglot_id int, --
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
Company_id int not null,
DayType_id int not null,
)





alter table Users
	add foreign key (Company_id) references Company(Company_id);
alter table Users
	add foreign key (Roles_id) references Roles(Roles_id);

alter table Parkinglot
	add foreign key (Company_id) references Company(Company_id);

alter table Subscription
	add foreign key (Company_id) references Company(Company_id);

alter table SubscriptedVehicle
	add foreign key (Subscription_id) references Subscription(Subscription_id);

alter table Pricing
	add foreign key (DayType_id) references DayType(DayType_id);
alter table Pricing
	add foreign key (SpecificDay_id) references SpecificDay(SpecificDay_id);

alter table Bill
	add foreign key (Company_id) references Company(Company_id);
alter table Bill
	add foreign key (Pricing_id) references Pricing(Pricing_id);
alter table Bill
	add foreign key (Users_id) references Users(Users_id);

alter table DayType
	add foreign key (Company_id) references Company(Company_id);
alter table SpecificDay
	add foreign key (DayType_id) references DayType(DayType_id);
alter table SpecificDay
	add foreign key (Company_id) references Company(Company_id);


