create table Coffee
(
    CoffeeId int identity(1,1) not null,
	CoffeeName nvarchar(max) not null
)
go

alter table Coffee
add constraint pk_CoffeeId primary key(CoffeeId)
go

create table Rating
(
	RatingId int identity(1,1) not null,
	CoffeeId int not null,
	Comment nvarchar(max) null,
	RatingValue int not null
)
go

alter table Rating
add constraint pl_RatingId primary key (RatingId)
go

alter table Rating
add constraint fk_coffee_id foreign key(CoffeeId)
references Coffee(CoffeeId)

insert into Coffee(CoffeeName)
values('Blue Mountain')
go

insert into Coffee(CoffeeName)
values('Blue Tokai')
go