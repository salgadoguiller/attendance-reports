using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportsManagement.Models;
using DocumentFormat.OpenXml;
using ClosedXML.Excel;
using System.IO;

namespace ReportsManagement.Controllers
{
    public class StructureController : Controller
    {
        //
        // GET: /Structure/
        public ActionResult getStructures()
        {
            Conection conection = new Conection();

            try
            {
                List<Department> departments = conection.departments();
                List<Employee> employees = conection.employees().OrderBy(param => param.Name).ToList();
                string path = @"C:\\StructureLNO\\";
                string filename = "StructureLNO.xlsx";
                string worksheetName = "StructureLNO";
                createFolder(path);
                writeStructureInExcel(departments, employees, path + filename, worksheetName);
                download(filename, path);
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                // ViewData["error"] = true;
                ViewData["message"] = 3;
                ViewData["messageValue"] = e.Message;
            }

            return View();
        }

        public void createFolder(string path)
        {
            bool folderExists = Directory.Exists((path));
            if (!folderExists)
                Directory.CreateDirectory((path));
        }

        public void writeStructureInExcel(List<Department> departments, List<Employee> employees, string path, string worksheetName)
        {
            Conection conection = new Conection();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(worksheetName);

            worksheet.Cell("A1").Value = "Departamento";
            worksheet.Cell("B1").Value = "Nombre Responsable";
            worksheet.Cell("C1").Value = "Correo Responsable";
            worksheet.Cell("D1").Value = "Integrantes";
            worksheet.Cell("E1").Value = "Equipo";
            worksheet.Cell("F1").Value = "Nombre Responsable";
            worksheet.Cell("G1").Value = "Correo Responsable";
            worksheet.Cell("H1").Value = "Integrantes";
            worksheet.Cell("I1").Value = "SubEquipo";
            worksheet.Cell("J1").Value = "Nombre Responsable";
            worksheet.Cell("K1").Value = "Correo Responsable";
            worksheet.Cell("L1").Value = "Integrantes";

            int indexCell = 2;

            for (int index = 0; index < departments.Count; index++)
            {
                // Departments
                if (departments[index].SupDeptid == 1)
                {
                    bool deptHaveSubDivisions = false;
                    bool deptHaveEmployees = false;
                    DepartmentMongo departmentMongo;

                    try
                    {
                        departmentMongo = conection.searchDepartmentMongo(departments[index].Deptid);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }

                    worksheet.Cell("A" + indexCell).Value = departments[index].DeptName;
                    worksheet.Cell("B" + indexCell).Value = departmentMongo.Name[0];
                    worksheet.Cell("C" + indexCell).Value = departmentMongo.correo[0];

                    
                    // SubDepartments
                    for (int index2 = 0; index2 < departments.Count; index2++)
                    {
                        if (departments[index2].SupDeptid == departments[index].Deptid)
                        {
                            deptHaveSubDivisions = true;
                            bool teamHaveSubDivisions = false;
                            bool teamHaveEmployees = false;
                            worksheet.Cell("E" + indexCell).Value = departments[index2].DeptName;

                            GetReportMongo teamMongo = conection.searchTeamMongo(departments[index2].Deptid, departments[index2].SupDeptid);

                            worksheet.Cell("F" + indexCell).Value = teamMongo.subName;
                            worksheet.Cell("G" + indexCell).Value = teamMongo.subEmail;

                            // SubSubDepartments
                            for (int index3 = 0; index3 < departments.Count; index3++)
                            {
                                if (departments[index3].SupDeptid == departments[index2].Deptid)
                                {
                                    teamHaveSubDivisions = true;
                                    bool subTeamHaveEmployees = false;
                                    worksheet.Cell("I" + indexCell).Value = departments[index3].DeptName;

                                    GetReportMongo subteamMongo = conection.searchTeamMongo(departments[index3].Deptid, departments[index2].SupDeptid);

                                    worksheet.Cell("J" + indexCell).Value = subteamMongo.subName;
                                    worksheet.Cell("K" + indexCell).Value = subteamMongo.subEmail;

                                    List<Employee> employeesFilter3 = (from emp in employees 
                                                                      where emp.TeamId == departments[index3].Deptid 
                                                                      select emp ).ToList();

                                    foreach(Employee empTemp in employeesFilter3)
                                    {
                                        subTeamHaveEmployees = true;
                                        worksheet.Cell("L" + indexCell).Value = empTemp.Name;
                                        worksheet.Cell("H" + indexCell).Value = empTemp.Name;
                                        worksheet.Cell("D" + indexCell).Value = empTemp.Name;
                                        
                                        indexCell += 1;
                                    }

                                    if (!subTeamHaveEmployees)
                                    {
                                        indexCell += 1;
                                    }
                                }
                            }
                            List<Employee> employeesFilter2 = (from emp in employees
                                                              where emp.TeamId == departments[index2].Deptid
                                                              select emp).ToList();

                            foreach (Employee empTemp in employeesFilter2)
                            {
                                teamHaveEmployees = true;
                                worksheet.Cell("H" + indexCell).Value = empTemp.Name;
                                worksheet.Cell("D" + indexCell).Value = empTemp.Name;

                                indexCell += 1;
                            }

                            if (!teamHaveSubDivisions && !teamHaveEmployees)
                            {
                                indexCell += 1;
                            }
                        }
                    }
                    List<Employee> employeesFilter = (from emp in employees
                                                       where emp.TeamId == departments[index].Deptid
                                                       select emp).ToList();

                    foreach (Employee empTemp in employeesFilter)
                    {
                        deptHaveEmployees = true;

                        worksheet.Cell("D" + indexCell).Value = empTemp.Name;

                        indexCell += 1;
                    }

                    if (!deptHaveSubDivisions && !deptHaveEmployees)
                    {
                        indexCell += 1;
                    }
                }
            }

            worksheet.Columns().AdjustToContents();

            workbook.SaveAs(path);
        }

        // ------------------------------------------------------------------------------
        // Funcion: metodo encargado de descargar los reportes generados.
        // Parametros:
        //          filename    -> nombre del documento donde se genero el reporte.
        //          path        -> directorio donde se genero el reporte.
        // ------------------------------------------------------------------------------
        private void download(string filename, string path)
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "Attachment; filename=\"" + filename + "\"");
            Response.Flush();
            Response.TransmitFile(path + filename);
            Response.End();
        }
    }
}
