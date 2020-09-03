﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReportsManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class Anviz_Data_BaseEntities : DbContext
    {
        public Anviz_Data_BaseEntities()
            : base("name=Anviz_Data_BaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<GetEmployes_Result> GetEmployes(Nullable<int> userid, string begindate)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("userid", userid) :
                new ObjectParameter("userid", typeof(int));
    
            var begindateParameter = begindate != null ?
                new ObjectParameter("begindate", begindate) :
                new ObjectParameter("begindate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetEmployes_Result>("GetEmployes", useridParameter, begindateParameter);
        }
    
        public virtual ObjectResult<SP_AllDepartments_Result> SP_AllDepartments()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_AllDepartments_Result>("SP_AllDepartments");
        }
    
        public virtual ObjectResult<SP_AllDepartmetsBK_Result> SP_AllDepartmetsBK()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_AllDepartmetsBK_Result>("SP_AllDepartmetsBK");
        }
    
        public virtual ObjectResult<SP_AllEmployees_Result> SP_AllEmployees()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_AllEmployees_Result>("SP_AllEmployees");
        }
    
        public virtual ObjectResult<SP_AllEmployeesBK_Result> SP_AllEmployeesBK()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_AllEmployeesBK_Result>("SP_AllEmployeesBK");
        }
    
        public virtual ObjectResult<SP_AllTeamsBK_Result> SP_AllTeamsBK()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_AllTeamsBK_Result>("SP_AllTeamsBK");
        }
    
        public virtual ObjectResult<SP_AllUsers_Result> SP_AllUsers()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_AllUsers_Result>("SP_AllUsers");
        }
    
        public virtual ObjectResult<SP_AllWeeklyReportsState_Result> SP_AllWeeklyReportsState()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_AllWeeklyReportsState_Result>("SP_AllWeeklyReportsState");
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int SP_Attendance(Nullable<int> userId, Nullable<System.DateTime> checkTime, string checkType, string sensorId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var checkTimeParameter = checkTime.HasValue ?
                new ObjectParameter("CheckTime", checkTime) :
                new ObjectParameter("CheckTime", typeof(System.DateTime));
    
            var checkTypeParameter = checkType != null ?
                new ObjectParameter("CheckType", checkType) :
                new ObjectParameter("CheckType", typeof(string));
    
            var sensorIdParameter = sensorId != null ?
                new ObjectParameter("SensorId", sensorId) :
                new ObjectParameter("SensorId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_Attendance", userIdParameter, checkTimeParameter, checkTypeParameter, sensorIdParameter);
        }
    
        public virtual ObjectResult<SP_ConfirmEmail_Result> SP_ConfirmEmail()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_ConfirmEmail_Result>("SP_ConfirmEmail");
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int SP_DeleteConfirmEmail(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_DeleteConfirmEmail", idParameter);
        }
    
        public virtual int SP_DeleteDepartment(Nullable<int> deptid)
        {
            var deptidParameter = deptid.HasValue ?
                new ObjectParameter("Deptid", deptid) :
                new ObjectParameter("Deptid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_DeleteDepartment", deptidParameter);
        }
    
        public virtual int SP_DeleteEmployee(Nullable<int> userid, Nullable<int> responsableAction, Nullable<System.DateTime> dateAction)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("Userid", userid) :
                new ObjectParameter("Userid", typeof(int));
    
            var responsableActionParameter = responsableAction.HasValue ?
                new ObjectParameter("ResponsableAction", responsableAction) :
                new ObjectParameter("ResponsableAction", typeof(int));
    
            var dateActionParameter = dateAction.HasValue ?
                new ObjectParameter("DateAction", dateAction) :
                new ObjectParameter("DateAction", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_DeleteEmployee", useridParameter, responsableActionParameter, dateActionParameter);
        }
    
        public virtual int SP_DeleteSpecialReport(Nullable<int> idEmployee)
        {
            var idEmployeeParameter = idEmployee.HasValue ?
                new ObjectParameter("IdEmployee", idEmployee) :
                new ObjectParameter("IdEmployee", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_DeleteSpecialReport", idEmployeeParameter);
        }
    
        public virtual int SP_DeleteUser(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_DeleteUser", userIdParameter);
        }
    
        public virtual int SP_DepartmentBK(Nullable<int> responsableAction, Nullable<int> typeAction, Nullable<System.DateTime> dateAction, Nullable<int> id_Dept, string nombre_Dept, string name, string correo)
        {
            var responsableActionParameter = responsableAction.HasValue ?
                new ObjectParameter("ResponsableAction", responsableAction) :
                new ObjectParameter("ResponsableAction", typeof(int));
    
            var typeActionParameter = typeAction.HasValue ?
                new ObjectParameter("TypeAction", typeAction) :
                new ObjectParameter("TypeAction", typeof(int));
    
            var dateActionParameter = dateAction.HasValue ?
                new ObjectParameter("DateAction", dateAction) :
                new ObjectParameter("DateAction", typeof(System.DateTime));
    
            var id_DeptParameter = id_Dept.HasValue ?
                new ObjectParameter("Id_Dept", id_Dept) :
                new ObjectParameter("Id_Dept", typeof(int));
    
            var nombre_DeptParameter = nombre_Dept != null ?
                new ObjectParameter("Nombre_Dept", nombre_Dept) :
                new ObjectParameter("Nombre_Dept", typeof(string));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var correoParameter = correo != null ?
                new ObjectParameter("Correo", correo) :
                new ObjectParameter("Correo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_DepartmentBK", responsableActionParameter, typeActionParameter, dateActionParameter, id_DeptParameter, nombre_DeptParameter, nameParameter, correoParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<SP_Employee_Result> SP_Employee(Nullable<int> userid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("Userid", userid) :
                new ObjectParameter("Userid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Employee_Result>("SP_Employee", useridParameter);
        }
    
        public virtual int SP_EmployeeBK(Nullable<int> responsableAction, Nullable<int> typeAction, Nullable<System.DateTime> dateAction, string userid)
        {
            var responsableActionParameter = responsableAction.HasValue ?
                new ObjectParameter("ResponsableAction", responsableAction) :
                new ObjectParameter("ResponsableAction", typeof(int));
    
            var typeActionParameter = typeAction.HasValue ?
                new ObjectParameter("TypeAction", typeAction) :
                new ObjectParameter("TypeAction", typeof(int));
    
            var dateActionParameter = dateAction.HasValue ?
                new ObjectParameter("DateAction", dateAction) :
                new ObjectParameter("DateAction", typeof(System.DateTime));
    
            var useridParameter = userid != null ?
                new ObjectParameter("Userid", userid) :
                new ObjectParameter("Userid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_EmployeeBK", responsableActionParameter, typeActionParameter, dateActionParameter, useridParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> SP_EndEstateDepartmentBK(Nullable<int> deptId)
        {
            var deptIdParameter = deptId.HasValue ?
                new ObjectParameter("DeptId", deptId) :
                new ObjectParameter("DeptId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_EndEstateDepartmentBK", deptIdParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> SP_EndEstateEmployeeBK(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_EndEstateEmployeeBK", userIdParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> SP_EndEstateTeamBK(Nullable<int> teamId)
        {
            var teamIdParameter = teamId.HasValue ?
                new ObjectParameter("TeamId", teamId) :
                new ObjectParameter("TeamId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_EndEstateTeamBK", teamIdParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> SP_ExistDepartment(Nullable<int> deptid)
        {
            var deptidParameter = deptid.HasValue ?
                new ObjectParameter("Deptid", deptid) :
                new ObjectParameter("Deptid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_ExistDepartment", deptidParameter);
        }
    
        public virtual int sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int SP_InsertConfirmEmail(string email)
        {
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_InsertConfirmEmail", emailParameter);
        }
    
        public virtual ObjectResult<Nullable<decimal>> SP_InsertDepartment(string deptName, Nullable<int> supDeptid)
        {
            var deptNameParameter = deptName != null ?
                new ObjectParameter("DeptName", deptName) :
                new ObjectParameter("DeptName", typeof(string));
    
            var supDeptidParameter = supDeptid.HasValue ?
                new ObjectParameter("SupDeptid", supDeptid) :
                new ObjectParameter("SupDeptid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("SP_InsertDepartment", deptNameParameter, supDeptidParameter);
        }
    
        public virtual int SP_InsertReportDetail(Nullable<int> typeReport, Nullable<int> idReceptor, Nullable<int> stateReport, Nullable<int> typeError, Nullable<int> idWeeklyReportState)
        {
            var typeReportParameter = typeReport.HasValue ?
                new ObjectParameter("TypeReport", typeReport) :
                new ObjectParameter("TypeReport", typeof(int));
    
            var idReceptorParameter = idReceptor.HasValue ?
                new ObjectParameter("IdReceptor", idReceptor) :
                new ObjectParameter("IdReceptor", typeof(int));
    
            var stateReportParameter = stateReport.HasValue ?
                new ObjectParameter("StateReport", stateReport) :
                new ObjectParameter("StateReport", typeof(int));
    
            var typeErrorParameter = typeError.HasValue ?
                new ObjectParameter("TypeError", typeError) :
                new ObjectParameter("TypeError", typeof(int));
    
            var idWeeklyReportStateParameter = idWeeklyReportState.HasValue ?
                new ObjectParameter("IdWeeklyReportState", idWeeklyReportState) :
                new ObjectParameter("IdWeeklyReportState", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_InsertReportDetail", typeReportParameter, idReceptorParameter, stateReportParameter, typeErrorParameter, idWeeklyReportStateParameter);
        }
    
        public virtual ObjectResult<Nullable<decimal>> SP_InsertReportState(Nullable<int> stateReport, Nullable<decimal> percentReports, Nullable<System.DateTime> startDate, Nullable<System.DateTime> endDate, Nullable<int> typeError)
        {
            var stateReportParameter = stateReport.HasValue ?
                new ObjectParameter("StateReport", stateReport) :
                new ObjectParameter("StateReport", typeof(int));
    
            var percentReportsParameter = percentReports.HasValue ?
                new ObjectParameter("PercentReports", percentReports) :
                new ObjectParameter("PercentReports", typeof(decimal));
    
            var startDateParameter = startDate.HasValue ?
                new ObjectParameter("StartDate", startDate) :
                new ObjectParameter("StartDate", typeof(System.DateTime));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("EndDate", endDate) :
                new ObjectParameter("EndDate", typeof(System.DateTime));
    
            var typeErrorParameter = typeError.HasValue ?
                new ObjectParameter("TypeError", typeError) :
                new ObjectParameter("TypeError", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("SP_InsertReportState", stateReportParameter, percentReportsParameter, startDateParameter, endDateParameter, typeErrorParameter);
        }
    
        public virtual int SP_InsertSpecialReport(Nullable<int> idEmployee)
        {
            var idEmployeeParameter = idEmployee.HasValue ?
                new ObjectParameter("IdEmployee", idEmployee) :
                new ObjectParameter("IdEmployee", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_InsertSpecialReport", idEmployeeParameter);
        }
    
        public virtual ObjectResult<sp_matrix_weekdays_Result> sp_matrix_weekdays(Nullable<int> idDepto, string begindate)
        {
            var idDeptoParameter = idDepto.HasValue ?
                new ObjectParameter("idDepto", idDepto) :
                new ObjectParameter("idDepto", typeof(int));
    
            var begindateParameter = begindate != null ?
                new ObjectParameter("begindate", begindate) :
                new ObjectParameter("begindate", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_matrix_weekdays_Result>("sp_matrix_weekdays", idDeptoParameter, begindateParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int SP_RollBackEmployeeDeleted(Nullable<int> responsableAction, Nullable<System.DateTime> dateAction, Nullable<int> idEmpBK)
        {
            var responsableActionParameter = responsableAction.HasValue ?
                new ObjectParameter("ResponsableAction", responsableAction) :
                new ObjectParameter("ResponsableAction", typeof(int));
    
            var dateActionParameter = dateAction.HasValue ?
                new ObjectParameter("DateAction", dateAction) :
                new ObjectParameter("DateAction", typeof(System.DateTime));
    
            var idEmpBKParameter = idEmpBK.HasValue ?
                new ObjectParameter("IdEmpBK", idEmpBK) :
                new ObjectParameter("IdEmpBK", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_RollBackEmployeeDeleted", responsableActionParameter, dateActionParameter, idEmpBKParameter);
        }
    
        public virtual ObjectResult<SP_SearchDepartment_Result> SP_SearchDepartment(Nullable<int> deptid)
        {
            var deptidParameter = deptid.HasValue ?
                new ObjectParameter("Deptid", deptid) :
                new ObjectParameter("Deptid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SearchDepartment_Result>("SP_SearchDepartment", deptidParameter);
        }
    
        public virtual ObjectResult<SP_SearchDepartmentBK_Result> SP_SearchDepartmentBK(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SearchDepartmentBK_Result>("SP_SearchDepartmentBK", idParameter);
        }
    
        public virtual ObjectResult<SP_SearchEmployeeBK_Result> SP_SearchEmployeeBK(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SearchEmployeeBK_Result>("SP_SearchEmployeeBK", idParameter);
        }
    
        public virtual ObjectResult<SP_SearchSubDepartments_Result> SP_SearchSubDepartments(Nullable<int> deptid)
        {
            var deptidParameter = deptid.HasValue ?
                new ObjectParameter("Deptid", deptid) :
                new ObjectParameter("Deptid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SearchSubDepartments_Result>("SP_SearchSubDepartments", deptidParameter);
        }
    
        public virtual ObjectResult<SP_SearchTeamBK_Result> SP_SearchTeamBK(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SearchTeamBK_Result>("SP_SearchTeamBK", idParameter);
        }
    
        public virtual ObjectResult<SP_SearchUsers_Result> SP_SearchUsers(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SearchUsers_Result>("SP_SearchUsers", userIdParameter);
        }
    
        public virtual ObjectResult<SP_SpecialReports_Result> SP_SpecialReports()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_SpecialReports_Result>("SP_SpecialReports");
        }
    
        public virtual int SP_TeamBK(Nullable<int> responsableAction, Nullable<int> typeAction, Nullable<System.DateTime> dateAction, Nullable<int> id_Dept, Nullable<int> deptID, string subdept, string subEmail, string subName)
        {
            var responsableActionParameter = responsableAction.HasValue ?
                new ObjectParameter("ResponsableAction", responsableAction) :
                new ObjectParameter("ResponsableAction", typeof(int));
    
            var typeActionParameter = typeAction.HasValue ?
                new ObjectParameter("TypeAction", typeAction) :
                new ObjectParameter("TypeAction", typeof(int));
    
            var dateActionParameter = dateAction.HasValue ?
                new ObjectParameter("DateAction", dateAction) :
                new ObjectParameter("DateAction", typeof(System.DateTime));
    
            var id_DeptParameter = id_Dept.HasValue ?
                new ObjectParameter("Id_Dept", id_Dept) :
                new ObjectParameter("Id_Dept", typeof(int));
    
            var deptIDParameter = deptID.HasValue ?
                new ObjectParameter("DeptID", deptID) :
                new ObjectParameter("DeptID", typeof(int));
    
            var subdeptParameter = subdept != null ?
                new ObjectParameter("Subdept", subdept) :
                new ObjectParameter("Subdept", typeof(string));
    
            var subEmailParameter = subEmail != null ?
                new ObjectParameter("SubEmail", subEmail) :
                new ObjectParameter("SubEmail", typeof(string));
    
            var subNameParameter = subName != null ?
                new ObjectParameter("SubName", subName) :
                new ObjectParameter("SubName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_TeamBK", responsableActionParameter, typeActionParameter, dateActionParameter, id_DeptParameter, deptIDParameter, subdeptParameter, subEmailParameter, subNameParameter);
        }
    
        public virtual ObjectResult<SP_TeamEmployees_Result> SP_TeamEmployees(Nullable<int> deptid)
        {
            var deptidParameter = deptid.HasValue ?
                new ObjectParameter("Deptid", deptid) :
                new ObjectParameter("Deptid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_TeamEmployees_Result>("SP_TeamEmployees", deptidParameter);
        }
    
        public virtual int SP_UpdateDepartment(Nullable<int> deptid, string deptName)
        {
            var deptidParameter = deptid.HasValue ?
                new ObjectParameter("Deptid", deptid) :
                new ObjectParameter("Deptid", typeof(int));
    
            var deptNameParameter = deptName != null ?
                new ObjectParameter("DeptName", deptName) :
                new ObjectParameter("DeptName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_UpdateDepartment", deptidParameter, deptNameParameter);
        }
    
        public virtual int SP_UpdateEmployee(Nullable<int> userid, Nullable<int> deptid, string name, string address, Nullable<int> responsableAction, Nullable<System.DateTime> dateAction)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("Userid", userid) :
                new ObjectParameter("Userid", typeof(int));
    
            var deptidParameter = deptid.HasValue ?
                new ObjectParameter("Deptid", deptid) :
                new ObjectParameter("Deptid", typeof(int));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var addressParameter = address != null ?
                new ObjectParameter("Address", address) :
                new ObjectParameter("Address", typeof(string));
    
            var responsableActionParameter = responsableAction.HasValue ?
                new ObjectParameter("ResponsableAction", responsableAction) :
                new ObjectParameter("ResponsableAction", typeof(int));
    
            var dateActionParameter = dateAction.HasValue ?
                new ObjectParameter("DateAction", dateAction) :
                new ObjectParameter("DateAction", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_UpdateEmployee", useridParameter, deptidParameter, nameParameter, addressParameter, responsableActionParameter, dateActionParameter);
        }
    
        public virtual int SP_UpdateReportDetail(Nullable<int> typeReport, Nullable<int> stateReport, Nullable<int> typeError, Nullable<int> idWeeklyReportState, Nullable<int> idReceptor)
        {
            var typeReportParameter = typeReport.HasValue ?
                new ObjectParameter("TypeReport", typeReport) :
                new ObjectParameter("TypeReport", typeof(int));
    
            var stateReportParameter = stateReport.HasValue ?
                new ObjectParameter("StateReport", stateReport) :
                new ObjectParameter("StateReport", typeof(int));
    
            var typeErrorParameter = typeError.HasValue ?
                new ObjectParameter("TypeError", typeError) :
                new ObjectParameter("TypeError", typeof(int));
    
            var idWeeklyReportStateParameter = idWeeklyReportState.HasValue ?
                new ObjectParameter("IdWeeklyReportState", idWeeklyReportState) :
                new ObjectParameter("IdWeeklyReportState", typeof(int));
    
            var idReceptorParameter = idReceptor.HasValue ?
                new ObjectParameter("IdReceptor", idReceptor) :
                new ObjectParameter("IdReceptor", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_UpdateReportDetail", typeReportParameter, stateReportParameter, typeErrorParameter, idWeeklyReportStateParameter, idReceptorParameter);
        }
    
        public virtual int SP_UpdateStateReport(Nullable<int> id, Nullable<int> stateReport, Nullable<decimal> percentReports, Nullable<System.DateTime> startDate, Nullable<System.DateTime> endDate, Nullable<int> typeError)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var stateReportParameter = stateReport.HasValue ?
                new ObjectParameter("StateReport", stateReport) :
                new ObjectParameter("StateReport", typeof(int));
    
            var percentReportsParameter = percentReports.HasValue ?
                new ObjectParameter("PercentReports", percentReports) :
                new ObjectParameter("PercentReports", typeof(decimal));
    
            var startDateParameter = startDate.HasValue ?
                new ObjectParameter("StartDate", startDate) :
                new ObjectParameter("StartDate", typeof(System.DateTime));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("EndDate", endDate) :
                new ObjectParameter("EndDate", typeof(System.DateTime));
    
            var typeErrorParameter = typeError.HasValue ?
                new ObjectParameter("TypeError", typeError) :
                new ObjectParameter("TypeError", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_UpdateStateReport", idParameter, stateReportParameter, percentReportsParameter, startDateParameter, endDateParameter, typeErrorParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual ObjectResult<SP_WeeklyReportState_Result> SP_WeeklyReportState(Nullable<int> idWeeklyReportState)
        {
            var idWeeklyReportStateParameter = idWeeklyReportState.HasValue ?
                new ObjectParameter("IdWeeklyReportState", idWeeklyReportState) :
                new ObjectParameter("IdWeeklyReportState", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_WeeklyReportState_Result>("SP_WeeklyReportState", idWeeklyReportStateParameter);
        }
    }
}