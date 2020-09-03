use Anviz_Data_Base;
/*===============================================================================================*/
/*===============================================================================================*/
/* Tablas. */
/*===============================================================================================*/
/*===============================================================================================*/

/*-----------------------------------------------------------------------------------------------*/
/* Información sobre la accion realizada. */
/*-----------------------------------------------------------------------------------------------*/
create table ActionInfo(
	IdAction int identity(1,1) not null,	/* PK */
	ResponsableAction int,					/* FK */
	TypeAction int,							/* 1 -> update; 2 -> delete; 3 -> rollBackDelete */
	DateAction datetime,

	constraint PK_Action primary key (IdAction),
	constraint FK_ResponsableAction foreign key (ResponsableAction) references UserProfile(UserId)
);

/*-----------------------------------------------------------------------------------------------*/
/* Log para los departamentos. */
/*-----------------------------------------------------------------------------------------------*/
create table DepartmentsBackup(
	Id int identity(1,1) not null,	/* PK */
	Id_Dept int not null,
	Nombre_Dept varchar(50),
	Name varchar(50),
	Correo varchar(150),
	ActionInfo int,					/* FK */

	constraint PK_Department primary key (Id),
	constraint FK_ActionInfoDepartment foreign key (ActionInfo) references ActionInfo(IdAction)
);

/*-----------------------------------------------------------------------------------------------*/
/* Log para los equipos. */
/*-----------------------------------------------------------------------------------------------*/
create table TeamsBackup(
	Id int identity(1,1) not null,	/* PK */
	Id_Dept int not null,
	DeptID int not null,
	Subdept varchar(50),
	SubEmail varchar(150),
	SubName varchar(50),
	ActionInfo int,					/* FK */

	constraint PK_Team primary key (Id),
	constraint FK_ActionInfoTeam foreign key (ActionInfo) references ActionInfo(IdAction)
);

/*-----------------------------------------------------------------------------------------------*/
/* Log para los empleados. */
/*-----------------------------------------------------------------------------------------------*/
create table EmployeesBackup(
	Id int identity(1,1) not null,	/* PK */
	Userid varchar(20) not null,
	Name varchar(50),
	Sex varchar(10),
	Pwd varchar(50),
	Deptid int not null,
	Nation varchar(50),
	Brithday smalldatetime,
	EmployDate smalldatetime,
	Telephone varchar(50),
	Duty varchar(50),
	NativePlace varchar(50),
	IDCard varchar(50),
	Address varchar(150),
	Mobile varchar(50),
	Educated varchar(50),
	Polity varchar(50),
	Specialty varchar(50),
	IsAtt bit,
	Isovertime bit,
	Isrest bit, 
	Remark varchar(250),
	MgFlag smallint,
	CardNum varchar(10),
	Picture image,
	UserFlag int,
	Groupid int,
	workdaylong int,

	ActionInfo int,					/* FK */

	constraint PK_Employee primary key (Id),
	constraint FK_ActionInfoEmployee foreign key (ActionInfo) references ActionInfo(IdAction)
);

/*-----------------------------------------------------------------------------------------------*/
/* Estado de envio de los reportes semanales. */
/*-----------------------------------------------------------------------------------------------*/
create table WeeklyReportsState(
	Id int identity(1,1) not null,		/* PK */
	StateReport int not null,			/* 0 -> no enviado; 1 -> enviado; 2 -> parcialmente enviado */
	PercentReports decimal not null,
	StartDate datetime,
	EndDate datetime,
	TypeError int not null,				/* 0 -> no error; 1 -> error en base de datos; 2 -> error al enviar email */

	constraint PK_WeeklyReportsState primary key (Id),
);

/*-----------------------------------------------------------------------------------------------*/
/* Detalles de envio de los reportes semanales. */
/*-----------------------------------------------------------------------------------------------*/
create table WeeklyReportsDetails(
	Id int identity(1,1) not null,		/* PK */
	TypeReport int not null,			/* 0 -> especial; 1 -> departamentos; 2 -> equipos; 3 -> empleados */
	IdReceptor int not null,			/* identificador de departamento, equipo, empleado o especial */
	StateReport int not null,			/* 0 -> no enviado; 1 -> enviado; */
	TypeError int not null,				/* 0 -> no error; 1 -> error en base de datos; 2 -> error al enviar email */
	IdWeeklyReportState int not null,	/* FK */

	constraint PK_WeeklyReportsDetails primary key (Id),
	constraint FK_ReportsStateReportsDetails foreign key (IdWeeklyReportState) references WeeklyReportsState(id),
);

/*-----------------------------------------------------------------------------------------------*/
/* Reportes especiales. */
/*-----------------------------------------------------------------------------------------------*/
create table SpecialReports(
	Id int identity(1,1) not null,	/* PK */
	idEmployee int,

	constraint PK_EspecialReport primary key (Id)
);

/*-----------------------------------------------------------------------------------------------*/
/* Correos de confirmacion reportes semanales. */
/*-----------------------------------------------------------------------------------------------*/
create table ConfirmEmails(
	Id int identity(1,1) not null,	/* PK */
	email varchar(50) not null,

	constraint PK_ConfirmEmail primary key (Id)
);

/*===============================================================================================*/
/*===============================================================================================*/
/* Procedimientos almacenados.*/
/*===============================================================================================*/
/*===============================================================================================*/

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: retorna todos los empleados en la base de datos.*/
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_AllEmployees
as
begin
	select Userinfo.Userid, UserInfo.Deptid, Userinfo.Name, Userinfo.Address
	from UserInfo
	order by Name asc
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: retorna todos los empleados que pertenecen a un equipo especifico.*/
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_TeamEmployees
	@Deptid int
as
begin
	select Userinfo.Userid, UserInfo.Deptid, Userinfo.Name, Userinfo.Address 
	from UserInfo
	where UserInfo.Deptid = @Deptid
	order by Name asc
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: busca y retorna un empleado especifico en la base de datos. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_Employee
	@Userid int
as
begin
	select Userinfo.Userid, UserInfo.Deptid, Userinfo.Name, Userinfo.Address 
	from UserInfo
	where UserInfo.Userid = @Userid
	order by Name asc
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion:	actualizar un empleado en la base de datos */
/*			y ejecutar el procedimiento almacenado SP_EmployeeBK */
/*			para almacenar una copia de seguridad del empleado. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_UpdateEmployee
	@Userid int,
	@Deptid int,
	@Name varchar(60),
	@Address varchar(60),
	@ResponsableAction int,
	@DateAction datetime
as
begin
	exec SP_EmployeeBK @ResponsableAction, 1 , @DateAction, @Userid
	update UserInfo
	set UserInfo.Deptid = @Deptid,
	UserInfo.Name = @Name,
	UserInfo.Address = @Address
	where UserInfo.Userid = @Userid
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion:	eliminar un empleado de la base de datos */
/*			y ejecutar el procedimiento almacenado SP_EmployeeBK */
/*			para almacenar una copia de seguridad del empleado. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_DeleteEmployee
	@Userid int,
	@ResponsableAction int,
	@DateAction datetime
as
begin
	exec SP_EmployeeBK @ResponsableAction, 2 , @DateAction, @Userid;
	delete UserInfo
	where UserInfo.Userid = @Userid
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: crea una copia de seguridad de un empleado. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_EmployeeBK
	@ResponsableAction int,
	@TypeAction int,
	@DateAction datetime,
	@Userid varchar(20)
as
begin
	insert into ActionInfo values (@ResponsableAction, @TypeAction, @DateAction)
	declare @ActionId int
	set @ActionId = SCOPE_IDENTITY()
	insert into EmployeesBackup
				select Userid,Name,Sex,Pwd,Deptid,Nation,Brithday,EmployDate,Telephone,Duty,NativePlace,IDCard,Address,Mobile,
						Educated,Polity,Specialty,IsAtt,Isovertime,Isrest,Remark,MgFlag,CardNum,Picture,UserFlag,Groupid,workdaylong, @ActionId
				from Userinfo where Userinfo.Userid = @Userid
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: crea una copia de seguridad de un departamento. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_DepartmentBK
	@ResponsableAction int,
	@TypeAction int,
	@DateAction datetime,
	@Id_Dept int,
	@Nombre_Dept varchar(50),
	@Name varchar(50),
	@Correo varchar(150)
as
begin
	insert into ActionInfo values (@ResponsableAction, @TypeAction, @DateAction)
	declare @ActionId int
	set @ActionId = SCOPE_IDENTITY()
	insert into DepartmentsBackup Values
				(@Id_Dept, @Nombre_Dept, @Name, @Correo, @ActionId)
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: crea una copia de seguridad de un equipo. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_TeamBK
	@ResponsableAction int,
	@TypeAction int,
	@DateAction datetime,
	@Id_Dept int,
	@DeptID int,
	@Subdept varchar(50),
	@SubEmail varchar(150),
	@SubName varchar(50)
as
begin
	insert into ActionInfo values (@ResponsableAction, @TypeAction, @DateAction)
	declare @ActionId int
	set @ActionId = SCOPE_IDENTITY()
	insert into TeamsBackup Values
				(@Id_Dept, @DeptID, @Subdept, @SubEmail, @SubName, @ActionId)
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: retorna todos los empleados en la bitacora de la base de datos.*/
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_AllEmployeesBK
as
begin
	select EmployeesBackup.Id, EmployeesBackup.Userid, EmployeesBackup.Deptid, EmployeesBackup.Name, EmployeesBackup.Address,
			ActionInfo.TypeAction, ActionInfo.DateAction,
			UserProfile.UserName
	from EmployeesBackup
	inner join ActionInfo on EmployeesBackup.ActionInfo = ActionInfo.IdAction
	inner join UserProfile on ActionInfo.ResponsableAction = UserProfile.UserId
	order by ActionInfo.DateAction desc
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: retorna todos los equipos en la bitacora de la base de datos.*/
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_AllTeamsBK
as
begin
	select TeamsBackup.Id, TeamsBackup.Id_Dept, TeamsBackup.DeptID, TeamsBackup.Subdept, TeamsBackup.SubEmail, TeamsBackup.SubName,
			ActionInfo.TypeAction, ActionInfo.DateAction,
			UserProfile.UserName
	from TeamsBackup 
	inner join ActionInfo on TeamsBackup.ActionInfo = ActionInfo.IdAction
	inner join UserProfile on ActionInfo.ResponsableAction = UserProfile.UserId
	order by ActionInfo.DateAction desc
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: retorna todos los departamentos en la bitacora de la base de datos.*/
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_AllDepartmetsBK
as
begin
	select DepartmentsBackup.Id, DepartmentsBackup.Id_Dept, DepartmentsBackup.Nombre_Dept, DepartmentsBackup.Name, DepartmentsBackup.Correo,
			ActionInfo.TypeAction, ActionInfo.DateAction,
			UserProfile.UserName
	from DepartmentsBackup 
	inner join ActionInfo on DepartmentsBackup.ActionInfo = ActionInfo.IdAction
	inner join UserProfile on ActionInfo.ResponsableAction = UserProfile.UserId
	order by ActionInfo.DateAction desc
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: busca y retorna un empleado especifico en la bitacora de la base de datos.*/
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_SearchEmployeeBK
	@Id int
as
begin
	select EmployeesBackup.Id, EmployeesBackup.Userid, EmployeesBackup.Deptid, EmployeesBackup.Name, EmployeesBackup.Address,
			ActionInfo.TypeAction, ActionInfo.DateAction,
			UserProfile.UserName
	from EmployeesBackup
	inner join ActionInfo on EmployeesBackup.ActionInfo = ActionInfo.IdAction
	inner join UserProfile on ActionInfo.ResponsableAction = UserProfile.UserId
	where EmployeesBackup.Id = @Id
	order by ActionInfo.DateAction desc
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: busca y retorna un equipo especifico en la bitacora de la base de datos.*/
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_SearchTeamBK
	@Id int
as
begin
	select TeamsBackup.Id, TeamsBackup.Id_Dept, TeamsBackup.DeptID, TeamsBackup.Subdept, TeamsBackup.SubEmail, TeamsBackup.SubName,
			ActionInfo.TypeAction, ActionInfo.DateAction,
			UserProfile.UserName
	from TeamsBackup 
	inner join ActionInfo on TeamsBackup.ActionInfo = ActionInfo.IdAction
	inner join UserProfile on ActionInfo.ResponsableAction = UserProfile.UserId
	where TeamsBackup.Id = @Id
	order by ActionInfo.DateAction desc;
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: busca y retorna un departamento especifico en la bitacora de la base de datos.*/
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_SearchDepartmentBK
	@Id int
as
begin
	select DepartmentsBackup.Id, DepartmentsBackup.Id_Dept, DepartmentsBackup.Nombre_Dept, DepartmentsBackup.Name, DepartmentsBackup.Correo,
			ActionInfo.TypeAction, ActionInfo.DateAction,
			UserProfile.UserName
	from DepartmentsBackup 
	inner join ActionInfo on DepartmentsBackup.ActionInfo = ActionInfo.IdAction
	inner join UserProfile on ActionInfo.ResponsableAction = UserProfile.UserId
	where DepartmentsBackup.Id = @Id
	order by ActionInfo.DateAction desc
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: busca y retorna el ultimo estado de un empleado en la bitacora de la base de datos*/
/*			1 = actualizado; 2 = eliminado */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_EndEstateEmployeeBK
	@UserId int
as
begin
	select Top 1 ActionInfo.TypeAction
	from EmployeesBackup
	inner join ActionInfo on EmployeesBackup.ActionInfo = ActionInfo.IdAction
	where EmployeesBackup.UserId = @UserId
	order by ActionInfo.DateAction desc;
end;


/*-----------------------------------------------------------------------------------------------*/
/* Funcion: busca y retorna el ultimo estado de un empleado en la bitacora de la base de datos*/
/*			1 = actualizado; 2 = eliminado */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_ExistDepartment
	@Deptid int
as
begin
	select COUNT(*)
	from Dept
	where Dept.Deptid = @Deptid
end;


/*-----------------------------------------------------------------------------------------------*/
/* Funcion: busca y retorna el ultimo estado de un equipo en la bitacora de la base de datos*/
/*			1 = actualizado; 2 = eliminado */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_EndEstateTeamBK
	@TeamId int
as
begin
	select Top 1 ActionInfo.TypeAction
	from TeamsBackup
	inner join ActionInfo on TeamsBackup.ActionInfo = ActionInfo.IdAction
	where TeamsBackup.DeptID = @TeamId
	order by ActionInfo.DateAction desc;
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: busca y retorna el ultimo estado de un departamento en la bitacora de la base de datos*/
/*			1 = actualizado; 2 = eliminado */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_EndEstateDepartmentBK
	@DeptId int
as
begin
	select Top 1 ActionInfo.TypeAction
	from DepartmentsBackup
	inner join ActionInfo on DepartmentsBackup.ActionInfo = ActionInfo.IdAction
	where DepartmentsBackup.Id_Dept = @DeptId
	order by ActionInfo.DateAction desc;
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: revertir la eliminacion de un empleado de la base de datos. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_RollBackEmployeeDeleted
	@ResponsableAction int,
	@DateAction datetime,
	@IdEmpBK int
as
begin
	insert into ActionInfo values (@ResponsableAction, 3, @DateAction)
	declare @ActionId int
	set @ActionId = SCOPE_IDENTITY()
	insert into EmployeesBackup
	select Userid,Name,null,null,Deptid,null,null,null,null,null,null,null,Address,null,
			null,null,null,null,null,null,null,null,null,null,null,null,null,@ActionId
	from EmployeesBackup where EmployeesBackup.Id = @IdEmpBK;
	insert into UserInfo
	select Userid,Name,Sex,Pwd,Deptid,Nation,Brithday,EmployDate,Telephone,Duty,NativePlace,IDCard,Address,Mobile,
			Educated,Polity,Specialty,IsAtt,Isovertime,Isrest,Remark,MgFlag,CardNum,Picture,UserFlag,Groupid,workdaylong
	from EmployeesBackup where EmployeesBackup.Id = @IdEmpBK
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: retornar todos los estados de envio de reportes semanales. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_AllWeeklyReportsState
as
begin
	select *
	from WeeklyReportsState
	order by WeeklyReportsState.StartDate desc;
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: retornar todos los detalles de un envio de reporte semanal. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_WeeklyReportState
	@IdWeeklyReportState int
as
begin
	select *
	from WeeklyReportsDetails
	where WeeklyReportsDetails.IdWeeklyReportState = @IdWeeklyReportState
	order by WeeklyReportsDetails.TypeReport desc;
end;


/*-----------------------------------------------------------------------------------------------*/
/* Funcion: insertar estado del reporte semanal. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_InsertReportState
	@StateReport int,
	@PercentReports decimal,
	@StartDate datetime,
	@EndDate datetime,
	@TypeError int
as
begin
	insert into WeeklyReportsState values (@StateReport, @PercentReports, @StartDate, @EndDate, @TypeError)
	select SCOPE_IDENTITY()
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: insertar detalles del reporte semanal. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_InsertReportDetail
	@TypeReport int,
	@IdReceptor int,
	@StateReport int,
	@TypeError int,
	@IdWeeklyReportState int
as
begin
	insert into WeeklyReportsDetails values (@TypeReport, @IdReceptor, @StateReport, @TypeError, @IdWeeklyReportState)
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: retornar todos los departamentos y equipos de la empresa. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_AllDepartments
as
begin
	select *
	from Dept
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: retornar todos los subdepartamentos de un departamento de la empresa. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_SearchSubDepartments
	@Deptid int
as
begin
	select *
	from Dept
	where Dept.SupDeptid = @Deptid
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: retornar un departamento o equipo especifico. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_SearchDepartment
	@Deptid int
as
begin
	select *
	from Dept
	where Dept.Deptid = @Deptid
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion:	actualizar un depeartamento en la base de datos. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_UpdateDepartment
	@Deptid int,
	@DeptName varchar(50)
as
begin
	update Dept
	set Dept.DeptName = @DeptName
	where Dept.Deptid = @Deptid
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion:	actualizar estado de reporte semanal en la base de datos. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_UpdateStateReport
	@Id int,
	@StateReport int,
	@PercentReports decimal,
	@StartDate datetime,
	@EndDate datetime,
	@TypeError int
as
begin
	declare @Percent decimal
	if @StateReport = 1
		begin
			set @Percent = 100
		end
	else 
		begin
			declare @Total decimal
			declare @Sent decimal
			set @Total = (select count(WeeklyReportsDetails.Id)
						 from WeeklyReportsDetails
						 where WeeklyReportsDetails.IdWeeklyReportState = @Id)
			set @Sent = (select count(WeeklyReportsDetails.Id)
						from WeeklyReportsDetails
						where WeeklyReportsDetails.IdWeeklyReportState = @Id and WeeklyReportsDetails.StateReport = 1)
			set @Percent = (@Sent / @Total) * 100
		end
	update WeeklyReportsState
	set WeeklyReportsState.StateReport = @StateReport,
		WeeklyReportsState.PercentReports = @Percent,
		WeeklyReportsState.StartDate = @StartDate,
		WeeklyReportsState.EndDate = @EndDate,
		WeeklyReportsState.TypeError = @TypeError
	where WeeklyReportsState.Id = @Id
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion:	update detalles de reporte semanal en la base de datos. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_UpdateReportDetail
	@TypeReport int,
	@StateReport int,
	@TypeError int,
	@IdWeeklyReportState int,
	@IdReceptor int
as
begin
	update WeeklyReportsDetails
	set WeeklyReportsDetails.TypeReport = @TypeReport,
		WeeklyReportsDetails.StateReport = @StateReport,
		WeeklyReportsDetails.TypeError = @TypeError
	where WeeklyReportsDetails.IdReceptor = @IdReceptor 
		  and WeeklyReportsDetails.IdWeeklyReportState = @IdWeeklyReportState
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion:	retornar los empleados que reciben reportes especiales. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_SpecialReports
as
begin
	select Userinfo.Userid, UserInfo.Deptid, Userinfo.Name, Userinfo.Address 
	from UserInfo
	where UserInfo.Userid in (Select SpecialReports.idEmployee
							from SpecialReports)
	order by Name asc
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: insertar un nuevo empleado que recibira un reporte especial. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_InsertSpecialReport
	@IdEmployee int
as
begin
	insert into SpecialReports values (@IdEmployee)
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: eliminar reporte especial para un empleado. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_DeleteSpecialReport
	@IdEmployee int
as
begin
	delete SpecialReports 
	where SpecialReports.idEmployee = @IdEmployee
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: insertar departamento y retornar el identificador. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_InsertDepartment
	@DeptName varchar(50),
	@SupDeptid int
as
begin
	insert into Dept values (@DeptName, @SupDeptid)
	select SCOPE_IDENTITY()
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: eliminar departamento. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_DeleteDepartment
	@Deptid int
as
begin
	delete Dept
	where Dept.Deptid = @Deptid
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion:	retornar los correos que reciben las confirmaciones de envio de reportes semanales. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_ConfirmEmail
as
begin
	select *
	from ConfirmEmails
	order by ConfirmEmails.email asc
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: insertar un nuevo correo que recibe las confirmaciones de envio de reportes semanales. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_InsertConfirmEmail
	@Email varchar(50)
as
begin
	insert into ConfirmEmails values (@Email)
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: eliminar correo que recibe las confirmaciones de envio de reportes semanales. */
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_DeleteConfirmEmail
	@Id int
as
begin
	delete ConfirmEmails 
	where ConfirmEmails.Id = @Id
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: retorna todos los usuarios del sistema.*/
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_AllUsers
as
begin
	select UserProfile.UserName, UserProfile.UserId
	from UserProfile
	order by UserProfile.UserName asc
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: retorna un usuarios del sistema.*/
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_SearchUsers
	@UserId int
as
begin
	select UserProfile.UserName, UserProfile.UserId
	from UserProfile
	where UserProfile.UserId = @UserId
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: eliminar un usuario del sistema.*/
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_DeleteUser
	@UserId int
as
begin
	delete
	from UserProfile
	where UserProfile.UserId = @UserId
	delete
	from webpages_Membership
	where webpages_Membership.UserId = @UserId
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: realizar marcaje de un empleado.*/
/*-----------------------------------------------------------------------------------------------*/
create procedure SP_Attendance
	@UserId int,
	@CheckTime datetime,
	@CheckType varchar(2),
	@SensorId varchar(10)
as
begin
	insert into Checkinout (Userid, CheckTime, CheckType, Sensorid) values (@UserId,@CheckTime,@CheckType,@SensorId);
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: */
/*-----------------------------------------------------------------------------------------------*/
create procedure GetEmployes
	@userid int,
	@begindate nvarchar(20)
as
begin
	declare @m date = cast(@begindate as date)
	declare @t date = dateadd(day,1,cast(@begindate as date))
	declare @w date = dateadd(day,2,cast(@begindate as date))
	declare @th date = dateadd(day,3,cast(@begindate as date))
	declare @f date = dateadd(day,4,cast(@begindate as date))
	declare @s date = dateadd(day,5,cast(@begindate as date))
	declare @sn date = dateadd(day,6,cast(@begindate as date))

	select userid, name,
			dbo.coming_in(Userid,@m) 'Hora de LLegada Lunes',
			dbo.sensor_in(Userid,@m) 'Lugar de Marcaje Llegada Lunes', 
			dbo.turn_off(Userid,@m) 'Hora de salida Lunes' ,
			dbo.sensor_out(Userid,@m) 'Lugar de Marcaje Salida Lunes',
			dbo.worked_hours(userid,@m) 'Monday',

			dbo.coming_in(Userid,@t) 'Hora de LLegada Martes',
			dbo.sensor_in(Userid,@t) 'Lugar de Marcaje Llegada Martes', 
			dbo.turn_off(Userid,@t) 'Hora de salida Martes' ,
			dbo.sensor_out(Userid,@t) 'Lugar de Marcaje Salida Martes', 
			dbo.worked_hours(userid,@t) 'Tuesday',

			dbo.coming_in(Userid,@w) 'Hora de LLegada Miercoles',
			dbo.sensor_in(Userid,@w) 'Lugar de Marcaje Llegada Miercoles',
			dbo.turn_off(Userid,@w) 'Hora de salida Miercoles' ,
			dbo.sensor_out(Userid,@w) 'Lugar de Marcaje Salida Miercoles', 
			dbo.worked_hours(userid,@w) 'Wednsday',

			dbo.coming_in(Userid,@th) 'Hora de LLegada Jueves',
			dbo.sensor_in(Userid,@th) 'Lugar de Marcaje Llegada Jueves', 
			dbo.turn_off(Userid,@th) 'Hora de salida Jueves' ,
			dbo.sensor_out(Userid,@th) 'Lugar de Marcaje Salida Jueves', 
			dbo.worked_hours(userid,@th) 'Thursday',

			dbo.coming_in(Userid,@f) 'Hora de LLegada Viernes',
			dbo.sensor_in(Userid,@f) 'Lugar de Marcaje Llegada Viernes',
			dbo.turn_off(Userid,@f) 'Hora de salida Viernes' ,
			dbo.sensor_out(Userid,@f) 'Lugar de Marcaje Salida Viernes',   
			dbo.worked_hours(userid,@f) 'Friday',

			dbo.coming_in(Userid,@s) 'Hora de LLegada Sabado',
			dbo.sensor_in(Userid,@s) 'Lugar de Marcaje Llegada Sabado',
			dbo.turn_off(Userid,@s) 'Hora de salida Sabado' ,
			dbo.sensor_in(Userid,@s) 'Lugar de Marcaje Salida Sabado',
			dbo.worked_hours(userid,@s) 'Saturday',

			dbo.coming_in(Userid,@sn) 'Hora de LLegada Domingo',
			dbo.sensor_in(Userid,@sn) 'Lugar de Marcaje Llegada Domingo',
			dbo.turn_off(Userid,@sn) 'Hora de salida Domingo' ,
			dbo.sensor_in(Userid,@sn) 'Lugar de Marcaje Salida Domingo',  
			dbo.worked_hours(userid,@sn) 'Sunday'
	from Userinfo 
	where Userid = @userid
	order by Userid desc
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: */
/*-----------------------------------------------------------------------------------------------*/
create procedure sp_matrix_weekdays
	@idDepto int,
	@begindate nvarchar(20)
as
begin
	declare @m date = cast(@begindate as date)
	declare @t date = dateadd(day,1,cast(@begindate as date))
	declare @w date = dateadd(day,2,cast(@begindate as date))
	declare @th date = dateadd(day,3,cast(@begindate as date))
	declare @f date = dateadd(day,4,cast(@begindate as date))
	declare @s date = dateadd(day,5,cast(@begindate as date))
	declare @sn date = dateadd(day,6,cast(@begindate as date))

	select userid, name,
						dbo.coming_in(Userid,@m) 'Hora de LLegada Lunes',
			dbo.sensor_in(Userid,@m) 'Lugar de Marcaje Llegada Lunes', 
			dbo.turn_off(Userid,@m) 'Hora de salida Lunes' ,
			dbo.sensor_out(Userid,@m) 'Lugar de Marcaje Salida Lunes',
			dbo.worked_hours(userid,@m) 'Monday',

			dbo.coming_in(Userid,@t) 'Hora de LLegada Martes',
			dbo.sensor_in(Userid,@t) 'Lugar de Marcaje Llegada Martes', 
			dbo.turn_off(Userid,@t) 'Hora de salida Martes' ,
			dbo.sensor_out(Userid,@t) 'Lugar de Marcaje Salida Martes', 
			dbo.worked_hours(userid,@t) 'Tuesday',

			dbo.coming_in(Userid,@w) 'Hora de LLegada Miercoles',
			dbo.sensor_in(Userid,@w) 'Lugar de Marcaje Llegada Miercoles',
			dbo.turn_off(Userid,@w) 'Hora de salida Miercoles' ,
			dbo.sensor_out(Userid,@w) 'Lugar de Marcaje Salida Miercoles', 
			dbo.worked_hours(userid,@w) 'Wednsday',

			dbo.coming_in(Userid,@th) 'Hora de LLegada Jueves',
			dbo.sensor_in(Userid,@th) 'Lugar de Marcaje Llegada Jueves', 
			dbo.turn_off(Userid,@th) 'Hora de salida Jueves' ,
			dbo.sensor_out(Userid,@th) 'Lugar de Marcaje Salida Jueves', 
			dbo.worked_hours(userid,@th) 'Thursday',

			dbo.coming_in(Userid,@f) 'Hora de LLegada Viernes',
			dbo.sensor_in(Userid,@f) 'Lugar de Marcaje Llegada Viernes',
			dbo.turn_off(Userid,@f) 'Hora de salida Viernes' ,
			dbo.sensor_out(Userid,@f) 'Lugar de Marcaje Salida Viernes',   
			dbo.worked_hours(userid,@f) 'Friday',

			dbo.coming_in(Userid,@s) 'Hora de LLegada Sabado',
			dbo.sensor_in(Userid,@s) 'Lugar de Marcaje Llegada Sabado',
			dbo.turn_off(Userid,@s) 'Hora de salida Sabado' ,
			dbo.sensor_in(Userid,@s) 'Lugar de Marcaje Salida Sabado',
			dbo.worked_hours(userid,@s) 'Saturday',

			dbo.coming_in(Userid,@sn) 'Hora de LLegada Domingo',
			dbo.sensor_in(Userid,@sn) 'Lugar de Marcaje Llegada Domingo',
			dbo.turn_off(Userid,@sn) 'Hora de salida Domingo' ,
			dbo.sensor_in(Userid,@sn) 'Lugar de Marcaje Salida Domingo',  
			dbo.worked_hours(userid,@sn) 'Sunday'
	from Userinfo 
	where Deptid = @idDepto
	order by Userid desc
end;

/*===============================================================================================*/
/* Funciones.*/
/*===============================================================================================*/

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: */
/*-----------------------------------------------------------------------------------------------*/
create function turn_off
(
     @id int,
	 @date date
)
returns datetime
as
begin
	return (select max((CheckTime))as horaEntrada from Checkinout
			where  cast(checkTime as date) = @date and Userid = @id)

end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: */
/*-----------------------------------------------------------------------------------------------*/
create function worked_hours
(
	@id int,
	@date date
)
returns decimal(18,2)
as
begin
	return (select cast(DATEDIFF(minute,min(CheckTime),max(CheckTime))/60.0 as decimal(18,2)) from Checkinout 
			where userid = @id 
			and cast(checkTime as date) = @date )
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: */
/*-----------------------------------------------------------------------------------------------*/
create function sensor_out
( 
     @id int,
	 @date date
)
returns integer
as
begin
	return (SELECT C.Sensorid
		FROM Checkinout C
		WHERE C.Userid = @id  AND cast(C.CheckTime as datetime) = [dbo].[turn_off](@id,@date) )
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: */
/*-----------------------------------------------------------------------------------------------*/
create function sensor_in
( 
     @id int,
	 @date date
)
returns integer
as
begin
	return (SELECT C.Sensorid
		FROM Checkinout C
		WHERE C.Userid = @id  AND cast(C.CheckTime as datetime) = [dbo].[coming_in](@id,@date) )

end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: */
/*-----------------------------------------------------------------------------------------------*/
create function I_and_OUT
( 
     @id int,
	 @date date
)
returns date
as
begin
	return (select min((CheckTime))as horaEntrada from Checkinout
			where  cast(checkTime as date) = @date and Userid = @id)
end;

/*-----------------------------------------------------------------------------------------------*/
/* Funcion: */
/*-----------------------------------------------------------------------------------------------*/
create function coming_in
( 
     @id int,
	 @date date
)
returns datetime
as
begin
	return (select min((CheckTime))as horaEntrada from Checkinout
			where  cast(checkTime as date) =@date  and Userid = @id)

end;