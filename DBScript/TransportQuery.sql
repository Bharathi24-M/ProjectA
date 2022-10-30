----To creaate database
create database Transport
use Transport

--To create Admin table
create table AdminInfo(
UserName varchar(255)not null,
[password] varchar(50)not null)

--Insert values to Admin Table
Insert into AdminInfo values('Admin','Admin@1234')

--To create VehicleInfo table
create table VehicleInfo(
VehicleNum varchar(30)not null Primary key,
Capacity int not null,
AvailableSeats int not null,
IsOperable bit not null)

--To create RouteInfo
create table RouteInfo(
RouteNum int identity(1,1) Primary key,
RouteName varchar(255),
VehicleNum varchar(30) Foreign Key References VehicleInfo(VehicleNum) not null)

--To create StopInfo
create table StopInfo(
StopId int identity(1,1) Primary key,

RouteNum int Foreign Key References RouteInfo(RouteNum) not null,
StopName varchar(50)not null)

--To create EmployeeInfo table
create table EmployeeInfo(
EmployeeId int identity(1,1) Primary Key,
[Name] varchar(255)not null,
Age int,
PhoneNumber varchar(10)not null,
VehicleNum varchar(30) Foreign Key References VehicleInfo(VehicleNum) not null,
StopId int Foreign Key References StopInfo(StopId))




