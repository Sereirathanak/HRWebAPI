using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HREmployeeController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HREmployeeController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }


        [HttpPost("structure")]
        public async Task<ActionResult<IEnumerable<ReturnEmployeeName>>> GetStructure(HRCustomerFilter filter)
        {

            var listEmployee = _context.employee.Include(e => e.ccfemployeeJoinInfo)
                                        .Include(e => e.imageProfile)
                                        .Include(e => e.ccfuser)
                                        .AsQueryable();


            if (filter.checkBranch == true && filter.checkDepartment == true && filter.checkPosition == true && filter.checkIncharge == true
                && filter.checkEcard == true && filter.search != "" && filter.search != null)
            {

                var listEmployess = listEmployee
                                                .Where(e => e.elevel == int.Parse(filter.search))
                                                .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                .Take(filter.pageSize)
                                                .AsQueryable()
                                                .ToList();

                return Ok(listEmployess);
            }
            else
            {
                return Ok(listEmployee);

            }

            return Ok(listEmployee);
        }

        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<ReturnEmployeeName>>> GetName(HRCustomerFilter filter)
        {
            var listEmployee = _context.employee.Include(e => e.ccfemployeeJoinInfo)
                                      .Include(e => e.imageProfile)
                                      .Include(e => e.ccfuser)
                                      .OrderByDescending(lr => lr.rdate)
                                    .Where(e => e.ccfemployeeJoinInfo.sup == filter.search)
                                    .AsQueryable()
                                    .Skip((filter.pageNumber - 1) * filter.pageSize)
                                    .Take(filter.pageSize)
                                    .ToList();

         
            return Ok(listEmployee);
        }


        [HttpPost("approvalrequest")]
        public async Task<ActionResult<IEnumerable<ReturnEmployeeName>>> GetRequestApprover(HRCustomerFilter filter)
        {
            var listEmployee = _context.employee.Include(e => e.ccfemployeeJoinInfo)
                                      .Include(e => e.imageProfile)
                                      .Include(e => e.ccfuser)
                                      .OrderByDescending(lr => lr.rdate)
                                    .Where(e => e.u5 != "")
                                    .AsQueryable()
                                    .Skip((filter.pageNumber - 1) * filter.pageSize)
                                    .Take(filter.pageSize)
                                    .ToList();


            return Ok(listEmployee);
        }

        //Get All List 
        [HttpPost("all")]
        public async Task<ActionResult<IEnumerable<HREmployee>>> GetAll(HRCustomerFilter filter)
        {

            var listEmployee = _context.employee.Include(e => e.ccfemployeeJoinInfo)
                                        .Include(e => e.imageProfile)
                                        .Include(e => e.ccfuser)
                                        .AsQueryable();


            int totallistEmployee = listEmployee.Count();

            //var departmentCode = _context.department.Where(e => e.depid == listEmployee.);


            //seleted branch and department and position and incharge
            if (filter.checkBranch == false && filter.checkDepartment == false && filter.checkPosition == false && filter.checkIncharge == false &&
               filter.checkEcard == false && filter.search == "" )
            {
                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                             .OrderByDescending(lr => lr.rdate)
                                                             .AsQueryable()
                                                             .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                             .Take(filter.pageSize)
                                                             .ToList();
                return Ok(listEmployess);

            }

            //list all 
            if (filter.checkBranch == true && filter.checkDepartment == true && filter.checkPosition == true && filter.checkIncharge == true
               && filter.checkEcard == true && filter.search == "" && filter.listall == true)
            {
                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .AsQueryable()
                                                            //.Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            //.Take(filter.pageSize)
                                                            .ToList();
                return Ok(listEmployess);
            }

            if (filter.checkBranch == true && filter.checkDepartment == true && filter.checkPosition == true && filter.checkIncharge == true
               && filter.checkEcard == true && filter.search == "" && filter.listall == false)
            {
                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                return Ok(listEmployess);
            }


            if (filter.checkBranch == true && filter.checkDepartment == true && filter.checkPosition == true && filter.checkIncharge == true
               && filter.checkEcard == true && filter.search == "" && filter.listall == null)
            {
                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                return Ok(listEmployess);
            }
            //seleted branch and department and position and incharge
            if (filter.checkBranch == true && filter.checkDepartment == true && filter.checkPosition == true && filter.checkIncharge == true
                && filter.checkEcard == true  && filter.search != "" && filter.search != null && filter.listall == true)
            {

                var branchCode = _context.hrBranchClass.SingleOrDefault(e => e.braname.ToLower().Contains(filter.search.ToLower()));

                var departmentCode = _context.department
                    .SingleOrDefault(e => e.depname.ToLower().Contains(filter.search.ToLower()));

                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                             .OrderByDescending(lr => lr.rdate)
                                                             .AsQueryable()
                                                             //.Skip((filter.pageNumber - 1) * filter.pageSize)
                                                             //.Take(filter.pageSize)
                                                             .ToList();
                var employeeCard = _context.employee.Where(e => e.ecard.Contains(filter.search));


                if (filter.status != null && filter.status !="")
                {

                    listEmployess = listEmployee.Where(lr => lr.estatus == filter.status)
                                                          .OrderByDescending(lr => lr.rdate)
                                                          .Where(e => e.ecard.Contains(filter.search))
                                                          .AsQueryable()
                                                          //.Skip((filter.pageNumber - 1) * filter.pageSize)
                                                          //.Take(filter.pageSize)
                                                          .ToList();

                    return Ok(listEmployess);
                }


                if (employeeCard != null && employeeCard.Count() > 0)
                {

                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                          .OrderByDescending(lr => lr.rdate)
                                                          .Where(e => e.ecard.Contains(filter.search))
                                                          .AsQueryable()
                                                          //.Skip((filter.pageNumber - 1) * filter.pageSize)
                                                          //.Take(filter.pageSize)
                                                          .ToList();

                    return Ok(listEmployess);
                }

                var findEmployeeLevel = _context.employee.Where(e => e.elevel.ToString().Contains(filter.search));

                if (findEmployeeLevel != null && findEmployeeLevel.Count() != 0)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                          .OrderByDescending(lr => lr.rdate)
                                                          .Where(e => e.elevel.ToString().Contains(filter.search))
                                                          .AsQueryable()
                                                          //.Skip((filter.pageNumber - 1) * filter.pageSize)
                                                          //.Take(filter.pageSize)
                                                          .ToList();

                    return Ok(listEmployess);
                }

                var findEmployeeIDuser = _context.employeeJoinInfo.Where(e => e.sup == filter.search);

                if (findEmployeeIDuser != null && findEmployeeIDuser.Count() > 0)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                          .OrderByDescending(lr => lr.rdate)
                                                          .Where(e => e.ccfemployeeJoinInfo.sup == filter.search)
                                                          .AsQueryable()
                                                          //.Skip((filter.pageNumber - 1) * filter.pageSize)
                                                          //.Take(filter.pageSize)
                                                          .ToList();

                    return Ok(listEmployess);
                }


                var employeeCode = _context.employee.Where(e => e.dname.ToLower().Contains(filter.search.ToLower()));


                if (employeeCode != null && employeeCode.Count()> 0 )
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.dname.ToLower().Contains(filter.search.ToLower()))
                                                            .AsQueryable()
                                                            //.Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            //.Take(filter.pageSize)
                                                            .ToList();

                    return Ok(listEmployess);
                }

                if (branchCode != null&& branchCode.braid !="")
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.site == branchCode.braid)
                                                            .AsQueryable()
                                                            //.Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            //.Take(filter.pageSize)
                                                            .ToList();

                    return Ok(listEmployess);
                }

                if (departmentCode != null && departmentCode.depid != "")
                {

                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.dep == departmentCode.depid)
                                                            .AsQueryable()
                                                            //.Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            //.Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }

                var filterPosition = _context.position.SingleOrDefault(e => e.pos.ToLower().Contains(filter.search.ToLower()));

                if (filterPosition != null && filterPosition.posid != "")
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.pos == filterPosition.posid)
                                                            .AsQueryable()
                                                            //.Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            //.Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }

                var filterIncharge = listEmployee.Where(lr => lr.estatus == "A")
                                                           .OrderByDescending(lr => lr.rdate)
                                                           .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                           .AsQueryable()
                                                           //.Skip((filter.pageNumber - 1) * filter.pageSize)
                                                           //.Take(filter.pageSize)
                                                           .ToList();
                if (filterIncharge != null && filterIncharge.Count()>0)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                            .AsQueryable()
                                                            //.Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            //.Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }

                return Ok(listEmployess);
            }

            //seleted branch and department and position and incharge list all
            if (filter.checkBranch == true && filter.checkDepartment == true && filter.checkPosition == true && filter.checkIncharge == true
                && filter.checkEcard == true && filter.search != "" && filter.search != null && filter.listall == null)
            {

                var branchCode = _context.hrBranchClass.SingleOrDefault(e => e.braname.ToLower().Contains(filter.search.ToLower()));

                var departmentCode = _context.department
                    .SingleOrDefault(e => e.depname.ToLower().Contains(filter.search.ToLower()));

                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                             .OrderByDescending(lr => lr.rdate)
                                                             .AsQueryable()
                                                             //.Skip((filter.pageNumber - 1) * filter.pageSize)
                                                             //.Take(filter.pageSize)
                                                             .ToList();
                var employeeCard = _context.employee.Where(e => e.ecard.Contains(filter.search));

                if (employeeCard != null && employeeCard.Count() > 0)
                {

                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                          .OrderByDescending(lr => lr.rdate)
                                                          .Where(e => e.ecard.Contains(filter.search))
                                                          .AsQueryable()
                                                          .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                          .Take(filter.pageSize)
                                                          .ToList();

                    return Ok(listEmployess);
                }

                var findEmployeeLevel = _context.employee.Where(e => e.elevel.ToString().Contains(filter.search));

                if (findEmployeeLevel != null && findEmployeeLevel.Count() != 0)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                          .OrderByDescending(lr => lr.rdate)
                                                          .Where(e => e.elevel.ToString().Contains(filter.search))
                                                          .AsQueryable()
                                                          .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                          .Take(filter.pageSize)
                                                          .ToList();

                    return Ok(listEmployess);
                }

                var findEmployeeIDuser = _context.employeeJoinInfo.Where(e => e.sup == filter.search);

                if (findEmployeeIDuser != null && findEmployeeIDuser.Count() > 0)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                          .OrderByDescending(lr => lr.rdate)
                                                          .Where(e => e.ccfemployeeJoinInfo.sup == filter.search)
                                                          .AsQueryable()
                                                          .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                          .Take(filter.pageSize)
                                                          .ToList();

                    return Ok(listEmployess);
                }


                var employeeCode = _context.employee.Where(e => e.dname.ToLower().Contains(filter.search.ToLower()));


                if (employeeCode != null && employeeCode.Count() > 0)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.dname.ToLower().Contains(filter.search.ToLower()))
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();

                    return Ok(listEmployess);
                }

                if (branchCode != null && branchCode.braid != "")
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.site == branchCode.braid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();

                    return Ok(listEmployess);
                }

                if (departmentCode != null && departmentCode.depid != "")
                {

                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.dep == departmentCode.depid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }

                var filterPosition = _context.position.SingleOrDefault(e => e.pos.ToLower().Contains(filter.search.ToLower()));

                if (filterPosition != null && filterPosition.posid != "")
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.pos == filterPosition.posid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }

                var filterIncharge = listEmployee.Where(lr => lr.estatus == "A")
                                                           .OrderByDescending(lr => lr.rdate)
                                                           .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                           .AsQueryable()
                                                           .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                           .Take(filter.pageSize)
                                                           .ToList();
                if (filterIncharge != null && filterIncharge.Count() > 0)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }

                return Ok(listEmployess);
            }

            //seleted branch and department and position and incharge list all
            if (filter.checkBranch == true && filter.checkDepartment == true && filter.checkPosition == true && filter.checkIncharge == true
                && filter.checkEcard == true && filter.search != "" && filter.search != null && filter.listall == false)
            {

                var branchCode = _context.hrBranchClass.SingleOrDefault(e => e.braname.ToLower().Contains(filter.search.ToLower()));

                var departmentCode = _context.department
                    .SingleOrDefault(e => e.depname.ToLower().Contains(filter.search.ToLower()));

                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                             .OrderByDescending(lr => lr.rdate)
                                                             .AsQueryable()
                                                             //.Skip((filter.pageNumber - 1) * filter.pageSize)
                                                             //.Take(filter.pageSize)
                                                             .ToList();
                var employeeCard = _context.employee.Where(e => e.ecard.Contains(filter.search));

                if (employeeCard != null && employeeCard.Count() > 0)
                {

                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                          .OrderByDescending(lr => lr.rdate)
                                                          .Where(e => e.ecard.Contains(filter.search))
                                                          .AsQueryable()
                                                          .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                          .Take(filter.pageSize)
                                                          .ToList();

                    return Ok(listEmployess);
                }

                var findEmployeeLevel = _context.employee.Where(e => e.elevel.ToString().Contains(filter.search));

                if (findEmployeeLevel != null && findEmployeeLevel.Count() != 0)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                          .OrderByDescending(lr => lr.rdate)
                                                          .Where(e => e.elevel.ToString().Contains(filter.search))
                                                          .AsQueryable()
                                                          .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                          .Take(filter.pageSize)
                                                          .ToList();

                    return Ok(listEmployess);
                }

                var findEmployeeIDuser = _context.employeeJoinInfo.Where(e => e.sup == filter.search);

                if (findEmployeeIDuser != null && findEmployeeIDuser.Count() > 0)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                          .OrderByDescending(lr => lr.rdate)
                                                          .Where(e => e.ccfemployeeJoinInfo.sup == filter.search)
                                                          .AsQueryable()
                                                          .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                          .Take(filter.pageSize)
                                                          .ToList();

                    return Ok(listEmployess);
                }


                var employeeCode = _context.employee.Where(e => e.dname.ToLower().Contains(filter.search.ToLower()));


                if (employeeCode != null && employeeCode.Count() > 0)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.dname.ToLower().Contains(filter.search.ToLower()))
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();

                    return Ok(listEmployess);
                }

                if (branchCode != null && branchCode.braid != "")
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.site == branchCode.braid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();

                    return Ok(listEmployess);
                }

                if (departmentCode != null && departmentCode.depid != "")
                {

                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.dep == departmentCode.depid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }

                var filterPosition = _context.position.SingleOrDefault(e => e.pos.ToLower().Contains(filter.search.ToLower()));

                if (filterPosition != null && filterPosition.posid != "")
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.pos == filterPosition.posid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }

                var filterIncharge = listEmployee.Where(lr => lr.estatus == "A")
                                                           .OrderByDescending(lr => lr.rdate)
                                                           .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                           .AsQueryable()
                                                           .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                           .Take(filter.pageSize)
                                                           .ToList();
                if (filterIncharge != null && filterIncharge.Count() > 0)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }

                return Ok(listEmployess);
            }

            //seleted branch and position and incharge
            if (filter.checkBranch == true && filter.checkPosition == true && filter.checkIncharge == true
                && filter.search != "" && filter.search != null)
            {
                
                var branchCode = _context.hrBranchClass.SingleOrDefault(e => e.braname.ToLower().Contains(filter.search.ToLower()));

                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                          .OrderByDescending(lr => lr.rdate)
                                                          .AsQueryable()
                                                          .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                          .Take(filter.pageSize)
                                                          .ToList();
                if (branchCode != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.site == branchCode.braid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();

                    return Ok(listEmployess);
                }

                var filterPosition = _context.position.SingleOrDefault(e => e.pos.ToLower().Contains(filter.search.ToLower()));

                if (filterPosition != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.pos == filterPosition.posid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }


                var filterIncharge = listEmployee.Where(lr => lr.estatus == "A")
                                                           .OrderByDescending(lr => lr.rdate)
                                                           .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                           .AsQueryable()
                                                           .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                           .Take(filter.pageSize)
                                                           .ToList();
                if (filterIncharge != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }

                return Ok(listEmployess);

            }


            //seleted department and position and incharge
            if (filter.checkDepartment == true && filter.checkPosition == true && filter.checkIncharge == true
                    && filter.search != "" && filter.search != null)
            {
                var departmentCode = _context.department.SingleOrDefault(e => e.depname.ToLower().Contains(filter.search.ToLower()));
              
                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                         .OrderByDescending(lr => lr.rdate)
                                                         .AsQueryable()
                                                         .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                         .Take(filter.pageSize)
                                                         .ToList();

                if (departmentCode != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.dep == departmentCode.depid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }

                var filterPosition = _context.position.SingleOrDefault(e => e.pos.ToLower().Contains(filter.search.ToLower()));

                if (filterPosition != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.pos == filterPosition.posid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }


                var filterIncharge = listEmployee.Where(lr => lr.estatus == "A")
                                                           .OrderByDescending(lr => lr.rdate)
                                                           .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                           .AsQueryable()
                                                           .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                           .Take(filter.pageSize)
                                                           .ToList();
                if (filterIncharge != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }
                return Ok(listEmployess);
            }


            //seleted branch and department and incharge
            if (filter.checkBranch == true && filter.checkDepartment == true && filter.checkIncharge == true
                    && filter.search != "" && filter.search != null)
            {
                var branchCode = _context.hrBranchClass.SingleOrDefault(e => e.braname.Contains(filter.search));
                var departmentCode = _context.department.SingleOrDefault(e => e.depname.Contains(filter.search));


                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                         .OrderByDescending(lr => lr.rdate)
                                                         .AsQueryable()
                                                         .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                         .Take(filter.pageSize)
                                                         .ToList();
                if (branchCode != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.site == branchCode.braid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();

                    return Ok(listEmployess);
                }
                if (departmentCode != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.dep == departmentCode.depid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }
                var filterIncharge = listEmployee.Where(lr => lr.estatus == "A")
                                                           .OrderByDescending(lr => lr.rdate)
                                                           .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                           .AsQueryable()
                                                           .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                           .Take(filter.pageSize)
                                                           .ToList();
                if (filterIncharge != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }
                return Ok(listEmployess);
            }


            //seleted branch and department and position
            if (filter.checkBranch == true && filter.checkDepartment == true
                && filter.checkPosition == true && filter.search != "" && filter.search != null)
            {
                var branchCode = _context.hrBranchClass.SingleOrDefault(e => e.braname.Contains(filter.search));
                var departmentCode = _context.department.SingleOrDefault(e => e.depname.Contains(filter.search));

                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                          .OrderByDescending(lr => lr.rdate)
                                                          .AsQueryable()
                                                          .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                          .Take(filter.pageSize)
                                                          .ToList();
                if (branchCode != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.site == branchCode.braid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();

                    return Ok(listEmployess);
                }

                if (departmentCode != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.dep == departmentCode.depid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }
                var filterPosition = _context.position.SingleOrDefault(e => e.pos.ToLower().Contains(filter.search.ToLower()));

                if (filterPosition != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.pos == filterPosition.posid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }

                return Ok(listEmployess);

            }


            //seleted position and incharge
            if (filter.checkPosition == true && filter.checkIncharge == true && filter.search != "" && filter.search != null)
            {
                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                       .OrderByDescending(lr => lr.rdate)
                                                       .AsQueryable()
                                                       .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                       .Take(filter.pageSize)
                                                       .ToList();

                var filterPosition = _context.position.SingleOrDefault(e => e.pos.ToLower().Contains(filter.search.ToLower()));

                if (filterPosition != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.pos == filterPosition.posid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }

                var filterIncharge = listEmployee.Where(lr => lr.estatus == "A")
                                                          .OrderByDescending(lr => lr.rdate)
                                                          .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                          .AsQueryable()
                                                          .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                          .Take(filter.pageSize)
                                                          .ToList();
                if (filterIncharge != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }

                return listEmployess;

            }



            //seleted department and incharge
            if (filter.checkDepartment == true && filter.checkIncharge == true && filter.search != "" && filter.search != null)
            {
                var departmentCode = _context.department.SingleOrDefault(e => e.depname.ToLower().Contains(filter.search.ToLower()));

                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                      .OrderByDescending(lr => lr.rdate)
                                                      .AsQueryable()
                                                      .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                      .Take(filter.pageSize)
                                                      .ToList();
                if (departmentCode != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.dep == departmentCode.depid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }
                var filterIncharge = listEmployee.Where(lr => lr.estatus == "A")
                                                         .OrderByDescending(lr => lr.rdate)
                                                         .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                         .AsQueryable()
                                                         .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                         .Take(filter.pageSize)
                                                         .ToList();
                if (filterIncharge != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }
                return listEmployess;

            }

            //seleted department and position
            if (filter.checkDepartment == true && filter.checkPosition == true && filter.search != "" && filter.search != null)
            {
                var departmentCode = _context.department.SingleOrDefault(e => e.depname.ToLower().Contains(filter.search.ToLower()));

                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                      .OrderByDescending(lr => lr.rdate)
                                                      .AsQueryable()
                                                      .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                      .Take(filter.pageSize)
                                                      .ToList();
                if (departmentCode != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.dep == departmentCode.depid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }
                var filterPosition = _context.position.SingleOrDefault(e => e.pos.ToLower().Contains(filter.search.ToLower()));

                if (filterPosition != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.pos == filterPosition.posid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }


                return listEmployess;

            }

            //seleted department and brach
            if (filter.checkDepartment == true && filter.checkBranch == true && filter.search != "" && filter.search != null)
            {
                var departmentCode = _context.department.SingleOrDefault(e => e.depname.ToLower().Contains(filter.search.ToLower()));
                var branchCode = _context.hrBranchClass.SingleOrDefault(e => e.braname.ToLower().Contains(filter.search.ToLower()));
                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                    .OrderByDescending(lr => lr.rdate)
                                                    .AsQueryable()
                                                    .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                    .Take(filter.pageSize)
                                                    .ToList();

                if (departmentCode != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.dep == departmentCode.depid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }
                if (branchCode != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.site == branchCode.braid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();

                    return Ok(listEmployess);
                }
                return Ok(departmentCode);

            }


            //seleted branch and incharge
            if (filter.checkBranch == true && filter.checkIncharge == true && filter.search != "" && filter.search != null)
            {
                var branchCode = _context.hrBranchClass.SingleOrDefault(e => e.braname.ToLower().Contains(filter.search.ToLower()));
                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                    .OrderByDescending(lr => lr.rdate)
                                                    .AsQueryable()
                                                    .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                    .Take(filter.pageSize)
                                                    .ToList();

                if (branchCode != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.site == branchCode.braid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();

                    return Ok(listEmployess);
                }
                var filterIncharge = listEmployee.Where(lr => lr.estatus == "A")
                                                         .OrderByDescending(lr => lr.rdate)
                                                         .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                         .AsQueryable()
                                                         .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                         .Take(filter.pageSize)
                                                         .ToList();
                if (filterIncharge != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.incharge.ToLower().Contains(filter.search.ToLower()))
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }
                return listEmployess;

            }

            //seleted branch and position
            if (filter.checkBranch == true && filter.checkPosition == true && filter.search != "" && filter.search != null)
            {
                var branchCode = _context.hrBranchClass.SingleOrDefault(e => e.braname.ToLower().Contains(filter.search.ToLower()));
                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                     .OrderByDescending(lr => lr.rdate)
                                                     .AsQueryable()
                                                     .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                     .Take(filter.pageSize)
                                                     .ToList();

                if (branchCode != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.site == branchCode.braid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();

                    return Ok(listEmployess);
                }

                var filterPosition = _context.position.SingleOrDefault(e => e.pos.ToLower().Contains(filter.search.ToLower()));

                if (filterPosition != null)
                {
                    listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                            .OrderByDescending(lr => lr.rdate)
                                                            .Where(e => e.ccfemployeeJoinInfo.pos == filterPosition.posid)
                                                            .AsQueryable()
                                                            .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                            .Take(filter.pageSize)
                                                            .ToList();
                    return Ok(listEmployess);

                }
                return listEmployess;

            }


            //seleted branch
            if (filter.checkBranch == true && filter.search != null && filter.search != "")
            {
                var branchCode = _context.hrBranchClass.SingleOrDefault(e => e.braname.ToLower().Contains(filter.search.ToLower()));
                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                             .OrderByDescending(lr => lr.rdate)
                                                             .Where(e => e.ccfemployeeJoinInfo.site == branchCode.braid)
                                                             .AsQueryable()
                                                             .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                             .Take(filter.pageSize)
                                                             .ToList();
                return listEmployess;

            }

            //seleted department 
            if (filter.search != "" && filter.search != null &&
                filter.checkDepartment == true )
            {
                var departmentCode = _context.department.SingleOrDefault(e => e.depname.ToLower().Contains(filter.search.ToLower()));

                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                             .OrderByDescending(lr => lr.rdate)
                                                             .Where(e => e.ccfemployeeJoinInfo.dep == departmentCode.depid)
                                                             .AsQueryable()
                                                             .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                             .Take(filter.pageSize)
                                                             .ToList();
                return listEmployess;

            }

            //seleted position 
            if (filter.checkPosition == true && filter.search != null && filter.search != "")
             {

                    var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                             .OrderByDescending(lr => lr.rdate)
                                                             .Where(e => e.ccfemployeeJoinInfo.pos.Contains(filter.search))
                                                             .AsQueryable()
                                                             .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                             .Take(filter.pageSize)
                                                             .ToList();
                return listEmployess;

            }

            //seleted position 
            if (filter.checkIncharge == true && filter.search != null && filter.search != "")
            {
                var listEmployess = listEmployee.Where(lr => lr.estatus == "A")
                                                             .OrderByDescending(lr => lr.rdate)
                                                             .Where(e => e.ccfemployeeJoinInfo.incharge.Contains(filter.search))
                                                             .AsQueryable()
                                                             .Skip((filter.pageNumber - 1) * filter.pageSize)
                                                             .Take(filter.pageSize)
                                                             .ToList();
                return listEmployess;

            }

            return BadRequest("Not Found");

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<HREmployee>>> GetDetailEmployee(string id)
        {
            var detail = await _context.employee.Where(u => u.eid == id)
              .Include(e => e.ccfemployeeJoinInfo)
              .Include(e => e.ccfemployeeHistory)
              .Include(e => e.ccfuser)
              .Include(e => e.imageProfile)
              .AsQueryable()
              .ToListAsync();
            if (detail == null)
            {
                return NotFound();
            }
            var results =  _context.employee.Where(u => u.eid == id);
            return results.ToList();
        }

        //Get Employee Detail 
        //[HttpGet("{id}")]
        //public async Task<ActionResult<IEnumerable<HREmployee>>> GetEmployeeDetail(string id)
        //{
        //    var detail =  _context.employee.Where(u => u.eid == id)
        //        .Include(e => e.employeeJoinInfo)
        //        .Include(e => e.employeeDocument)
        //        .Include(e => e.employeeFamily)
        //        .Include(e => e.employeeHistory)
        //        .Include(e => e.employeeEducation)
        //        .AsQueryable();
        //    return Ok(detail);
        //}

        //create employee
        [HttpPost("hr/createEmployee")]
        public async Task<IActionResult> CreateUser(HRCreateEmployee employee)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            //Ex-date
            var year = DateTime.Now.ToString("yyyy");
            int plusYear = int.Parse(year) + 1;
            var MM = DateTime.Now.ToString("MM");
            var dd = DateTime.Now.ToString("dd");
            var HH = DateTime.Now.ToString("HH");
            var mm = DateTime.Now.ToString("mm");
            var ss = DateTime.Now.ToString("ss");
            String plusString = $"{plusYear}{"-"}{MM}{"-"}{dd} {HH}{":"}{mm}{":"}{ss}";
            DateTime DOIEx = DateTime.ParseExact((plusString).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                bool exsitingEmployee = false;

                exsitingEmployee = _context.employee.Any(e => e.ecard == employee.ecard);


                if (_context.employee.Any(e => e.ecard == null))
                {
                    exsitingEmployee = false;
                }
                else if (exsitingEmployee == true)
                {
                    exsitingEmployee = true;
                }

                if (exsitingEmployee == true)
                {
                    var listEmployee = _context.employee.Where(u => u.ecard == employee.ecard);
                    return Ok(new KeyValuePair<string, string>("200", "Employee ID already created"));

                }
                else
                {


                    if (employee.ecard != null
                        && employee.fname != null
                        && employee.lname != null
                        && employee.dname != null
                        && employee.mstatus != null
                        && employee.gender != null
                        && employee.nat != null
                        && employee.site != null
                        && employee.dep !=null
                        && employee.pos !=null
                        && employee.stype !=null
                        && employee.con !=null
                        && employee.etype !=null
                        && employee.status !=null)
                    {
                        HREmployee employee1 = new HREmployee(_context);
                        employee1.eid = await GetNextIDEmployee();
                        employee1.ecard = employee.ecard;
                        employee1.fname = employee.fname;
                        employee1.lname = employee.lname;
                        employee1.dname = employee.dname;
                        employee1.dob = employee.dob;
                        employee1.mstatus = employee.mstatus;
                        employee1.gender = employee.gender;
                        employee1.nat = employee.nat;
                        employee1.blo = employee.blo;
                        employee1.reg = employee.reg;
                        employee1.pba = employee.pba;
                        employee1.fnum = employee.fnum;
                        employee1.hnum = employee.hnum;
                        employee1.etn = employee.etn;
                        employee1.rnum = employee.rnum;
                        employee1.village = employee.village;
                        employee1.district = employee.district;
                        employee1.commune = employee.commune;
                        employee1.province = employee.province;
                        employee1.dadd = employee.dadd;
                        employee1.pnum = employee.pnum;
                        employee1.email = employee.email;
                        employee1.rdate = DOI;
                        employee1.estatus = employee.estatus;
                        employee1.photo = employee.photo;
                        employee1.elevel = employee.elevel;

                        _context.employee.Add(employee1);
                        await _context.SaveChangesAsync();

               

                        //auto insert to table user loin
                        HRCcfUserClass _user = new HRCcfUserClass();
                        _user.ucode = await GetNextIDUserLogin();
                        _user.uid = employee1.eid;
                        _user.upassword = "123";
                        _user.ulevel = employee.elevel;
                        _user.bcode = employee.bcode;
                        _user.datecreate = DOI;
                        _user.isapprover = employee.isapprover;
                        _user.ustatus = "A";
                        _user.uname = employee.dname;
                        _user.exdate = DOIEx;
                        _user.changepassword = "N";
                        _context.ccfUserClass.Add(_user);
                        await _context.SaveChangesAsync();



                        //Employee Join Infor
                        HREmployeeJoinInfo _employeeJoinInfo = new HREmployeeJoinInfo(_context);

                        _employeeJoinInfo.jonid = await GetNextIDJonid();
                        _employeeJoinInfo.eid = employee1.eid;
                        _employeeJoinInfo.jdate = DOI;
                        _employeeJoinInfo.site = employee.bcode;
                        _employeeJoinInfo.dep = employee.dep;
                        _employeeJoinInfo.pos = employee.pos;
                        _employeeJoinInfo.edate = employee.edate;
                        _employeeJoinInfo.ecdate = employee.ecdate;
                        _employeeJoinInfo.sup = employee.sup;
                        _employeeJoinInfo.pmsal = employee.pmsal;
                        _employeeJoinInfo.msal = employee.msal;
                        _employeeJoinInfo.stype = employee.stype;
                        _employeeJoinInfo.con = employee.con;
                        _employeeJoinInfo.etype = employee.etype;
                        _employeeJoinInfo.npm = employee.npm;
                        _employeeJoinInfo.status = employee.status;
                        _employeeJoinInfo.remark = employee.remark;
                        _employeeJoinInfo.incharge = employee.incharge;

                        
                        _context.employeeJoinInfo.Add(_employeeJoinInfo);
                        await _context.SaveChangesAsync();

                        return Ok(employee1);
                    }
                    else
                    {

                        return BadRequest("The credential is invalid.");

                    }
                }

                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

        //Edit Employee
        [HttpPut("hr/editEmployee/{eid}")]
        public async Task<IActionResult> EditUser(string eid, HRCreateEmployee employee)

        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            //Ex-date
            var year = DateTime.Now.ToString("yyyy");
            int plusYear = int.Parse(year) + 1;
            var MM = DateTime.Now.ToString("MM");
            var dd = DateTime.Now.ToString("dd");
            var HH = DateTime.Now.ToString("HH");
            var mm = DateTime.Now.ToString("mm");
            var ss = DateTime.Now.ToString("ss");
            String plusString = $"{plusYear}{"-"}{MM}{"-"}{dd} {HH}{":"}{mm}{":"}{ss}";
            DateTime DOIEx = DateTime.ParseExact((plusString).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            try
            {
                bool exsitingEmployee = false;

                exsitingEmployee = _context.employee.Any(e => e.eid == eid);


                if (_context.employee.Any(e => e.ecard == null))
                {
                    exsitingEmployee = false;
                }
                else if (exsitingEmployee == true)
                {
                    exsitingEmployee = true;
                }

                if (exsitingEmployee == true)
                {
                    //var editEmployee = _context.employee.SingleOrDefault(e => e.ecard == ecard);
                    HREmployee editEmployee = await _context.employee.AsNoTracking().FirstOrDefaultAsync(x => x.eid == employee.eid);
                    //return Ok(listEmployee);
                    //return Ok(new KeyValuePair<string, string>("200", "Employee ID already created"));

                    if (employee.ecard != null
                       && employee.fname != null
                       && employee.lname != null
                       && employee.dname != null
                       && employee.mstatus != null
                       && employee.gender != null
                       && employee.nat != null
                       && employee.site != null
                       && employee.dep != null
                       && employee.pos != null
                       && employee.stype != null
                       && employee.con != null
                       && employee.etype != null
                       && employee.status != null

                       )
                    {
                        HREmployee employee1 = new HREmployee(_context);
                        employee1.eid = editEmployee.eid;
                        employee1.ecard = editEmployee.ecard;
                        employee1.fname = employee.fname;
                        employee1.lname = employee.lname;
                        employee1.dname = employee.dname;
                        employee1.dob = employee.dob;
                        employee1.mstatus = employee.mstatus;
                        employee1.gender = employee.gender;
                        employee1.nat = employee.nat;
                        employee1.blo = employee.blo;
                        employee1.reg = employee.reg;
                        employee1.pba = employee.pba;
                        employee1.fnum = employee.fnum;
                        employee1.hnum = employee.hnum;
                        employee1.etn = employee.etn;
                        employee1.rnum = employee.rnum;
                        employee1.village = employee.village;
                        employee1.district = employee.district;
                        employee1.commune = employee.commune;
                        employee1.province = employee.province;
                        employee1.dadd = employee.dadd;
                        employee1.pnum = employee.pnum;
                        employee1.email = employee.email;
                        employee1.rdate = DOI;
                        employee1.estatus = employee.estatus;
                        employee1.photo = employee.photo;
                        employee1.elevel = employee.elevel;

                        _context.Entry(employee1).State = EntityState.Modified;


                        HREmployeeJoinInfo _employeeJoinInfo = await _context.employeeJoinInfo.AsNoTracking().FirstOrDefaultAsync(x => x.jonid == employee.jonid);

                        //Employee Join Infor

                        _employeeJoinInfo.jonid = _employeeJoinInfo.jonid;
                        _employeeJoinInfo.eid = _employeeJoinInfo.eid;
                        _employeeJoinInfo.jdate = DOI;
                        _employeeJoinInfo.site = employee.bcode;
                        _employeeJoinInfo.dep = employee.dep;
                        _employeeJoinInfo.pos = employee.pos;
                        _employeeJoinInfo.edate = employee.edate;
                        _employeeJoinInfo.ecdate = employee.ecdate;
                        _employeeJoinInfo.sup = employee.sup;
                        _employeeJoinInfo.pmsal = employee.pmsal;
                        _employeeJoinInfo.msal = employee.msal;
                        _employeeJoinInfo.stype = employee.stype;
                        _employeeJoinInfo.con = employee.con;
                        _employeeJoinInfo.etype = employee.etype;
                        _employeeJoinInfo.npm = employee.npm;
                        _employeeJoinInfo.status = employee.status;
                        _employeeJoinInfo.remark = employee.remark;


                        _context.Entry(_employeeJoinInfo).State = EntityState.Modified;


                        HRCcfUserClass _employeeUser = await _context.ccfUserClass.AsNoTracking().FirstOrDefaultAsync(x => x.uid == employee.eid);


                        _employeeUser.ucode = _employeeUser.ucode;
                        _employeeUser.uid = editEmployee.eid;
                        _employeeUser.upassword = _employeeUser.upassword;
                        _employeeUser.ulevel = employee.elevel;
                        _employeeUser.bcode = employee.bcode;
                        _employeeUser.datecreate = DOI;
                        _employeeUser.isapprover = employee.isapprover;
                        _employeeUser.ustatus = _employeeUser.ustatus;
                        _employeeUser.uname = employee.dname;
                        _employeeUser.exdate = _employeeUser.exdate;
                        _employeeUser.changepassword = _employeeUser.changepassword;
                        _context.Entry(_employeeUser).State = EntityState.Modified;

                        await _context.SaveChangesAsync();

                        return Ok(employee);
                    }
                    else
                    {
                        return BadRequest("The credential is invalid.");
                    }

                }
                else
                {
                    return BadRequest("The credential is invalid.");
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

        //
        public async Task<string> GetNextIDEmployee()
        {
            var id = await _context.employee.OrderByDescending(u => u.eid).FirstOrDefaultAsync();

            if (id == null)
            {
                return "200000";
            }
            var nextId = int.Parse(id.eid) + 1;
            return nextId.ToString();
        }

        //Get Next ID User
        public async Task<string> GetNextIDUserLogin()
        {
            var id = await _context.ccfUserClass.OrderByDescending(u => u.ucode).FirstOrDefaultAsync();

            if (id == null)
            {
                return "100000";
            }
            var nextId = int.Parse(id.ucode) + 1;
            return nextId.ToString();
        }
        //Get Next ID Jon id
        public async Task<string> GetNextIDJonid()
        {
            var id = await _context.employeeJoinInfo.OrderByDescending(u => u.jonid).FirstOrDefaultAsync();

            if (id == null)
            {
                return "300000";
            }
            var nextId = int.Parse(id.jonid) + 1;
            return nextId.ToString();
        }

        //Edit Employee

        [HttpPut("{ecard}")]
        public async Task<IActionResult> EditEmployee(string ecard, HRCreateEmployee _hrCcfUserClass)
        {
            if (ecard != _hrCcfUserClass.ecard)
            {
                return BadRequest();
            }

            _context.Entry(_hrCcfUserClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(ecard))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool EmployeeExists(string eid)
        {
            return _context.employee.Any(e => e.ecard == eid);
        }

    }
}
