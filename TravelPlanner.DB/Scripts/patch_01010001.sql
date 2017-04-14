if OBJECT_ID('Users', 'U') is null
begin
	create table Users
	(
		Id bigint primary key identity(1, 1),
		Name nvarchar(128) not null,
		Email nvarchar(32) not null unique,
		Password char(60) not null,
		Type tinyint not null
	)

	-- Password = 1111
	insert into Users (Name, Email, Type, Password)
	values ('User',    'user@test.com',    1, '$2a$10$I44hzoGZ0I61GpfL97yPWu1bpATfIirh9PMVJsNOr7MO9hVNbRwyG'),
		   ('Manager', 'manager@test.com', 2, '$2a$10$I44hzoGZ0I61GpfL97yPWu1bpATfIirh9PMVJsNOr7MO9hVNbRwyG'),
		   ('Admin',   'admin@test.com',   3, '$2a$10$I44hzoGZ0I61GpfL97yPWu1bpATfIirh9PMVJsNOr7MO9hVNbRwyG')	
end
go

if OBJECT_ID('Trips', 'U') is null
begin
	create table Trips
	(
		Id bigint primary key identity(1, 1),
		UserId bigint not null foreign key references Users(Id),
		Destination nvarchar(128) not null,
		StartDate date not null,
		EndDate date not null,
		Comment nvarchar(256) null
	)

	create index IX_Trips_UserId on Trips(UserId)
end
go
