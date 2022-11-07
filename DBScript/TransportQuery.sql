----To creaate database
create database Transport
use Transport

--To create Admin table
create table AdminInfo(
UserName varchar(255)not null,
[password] varchar(50)not null)

--Insert values to Admin Table
Insert into AdminInfo values('Admin','Admin@1234')

--To create RouteInfo
create table RouteInfo(
RouteNum int identity(1,1) Primary key,
RouteName varchar(255) not null)

--To create StopInfo
create table StopInfo(
StopId int identity(1,1) Primary key,
StopName varchar(50) not null,
RouteNum int Foreign Key References RouteInfo(RouteNum) not null)

--To create VehicleInfo table
create table VehicleInfo(
VehicleId int identity(1,1) Primary Key,
VehicleNum varchar(30)not null,
RouteNum int Foreign Key References RouteInfo(RouteNum) not null,
Capacity int not null,
AvailableSeats int not null,
IsOperable bit not null)

--To create EmployeeInfo table
create table EmployeeInfo(
EmployeeId int identity(1,1) Primary Key,
[Name] varchar(255) not null,
Age int not null,
PhoneNumber varchar(10) not null,
RouteNum int Foreign Key References RouteInfo(RouteNum) not null,
VehicleId int Foreign Key References VehicleInfo(VehicleId) not null,
StopId int Foreign Key References StopInfo(StopId) not null)




